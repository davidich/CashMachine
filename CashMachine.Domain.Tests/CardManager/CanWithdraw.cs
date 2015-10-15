namespace CashMachine.Domain.Tests.CardManager
{
    using System;
    using System.Data;
    using System.Threading.Tasks;

    using CashMachine.Domain.Abstractions;
    using CashMachine.Domain.Entities;
    using CashMachine.Domain.Managers;

    using Microsoft.AspNet.Identity;

    using Moq;

    using Xunit;

    public class CanWithdraw
    {
        private readonly CardIdentity _cardIdentity;

        public CanWithdraw()
        {
            _cardIdentity = new CardIdentity(123, "0123012301230123");
            TestableDateTime.Init();
        }

        [Fact]
        public async Task DoesNotWithdrawFromLockedCard()
        {
            // Arrange
            var builder = new SutBuilder(_cardIdentity.Id, isCardLocked: true);
            var sut = builder.Build();

            // Act
            WithdrawalResult result = await sut.WithdrawAsync(_cardIdentity.Id, 0);

            // Assert
            Assert.Equal(WithdrawalStatus.CardIsLocked, result.Status);
            Assert.Equal(0, result.OperationId);
        }

        [Fact]
        public async Task DoesNotWithdrawMoreThanBalanceIs()
        {
            // Arrange
            var builder = new SutBuilder(_cardIdentity.Id)
                          {
                              InitialCardBalance = 200
                          };

            var sut = builder.Build();

            // Act
            WithdrawalResult result = await sut.WithdrawAsync(_cardIdentity.Id, 201);

            // Assert
            Assert.Equal(WithdrawalStatus.TooBigAmount, result.Status);
            Assert.Equal(0, result.OperationId);
        }

        [Fact]
        public async Task SetProperBalanceAfterWithdrawal()
        {
            // Arrange
            var builder = new SutBuilder(_cardIdentity.Id)
            {
                InitialCardBalance = 200
            };

            var sut = builder.Build();

            // Act
            WithdrawalResult result = await sut.WithdrawAsync(_cardIdentity.Id, 50);

            // Assert
            Assert.Equal(WithdrawalStatus.Success, result.Status);
            builder.CardRepoMock.Verify(cr => cr.SetBalanceAsync(_cardIdentity.Id, 150));
        }

        [Fact]
        public async Task ProperlyLogsOperation()
        {
            // Arrange
            var builder = new SutBuilder(_cardIdentity.Id)
            {
                InitialCardBalance = 200,
                OperationId = 666
            };
            var sut = builder.Build();

            // Act
            var result = await sut.WithdrawAsync(_cardIdentity.Id, 50);

            // Assert
            Assert.Equal(WithdrawalStatus.Success, result.Status);
            Assert.Equal(666, result.OperationId);
            builder.OperRepoMock.Verify(or => or.LogOperationAsync(_cardIdentity.Id, 50, OperationCode.Withdraw, TestableDateTime.SystemNow));
            
        }    

        [Fact]
        public async Task DoesWithdrawalInsideTransaction()
        {
            // Arrange
            var builder = new SutBuilder(_cardIdentity.Id)
            {
                InitialCardBalance = 200,
            };
            var sut = builder.Build();

            // Act
            await sut.WithdrawAsync(_cardIdentity.Id, 50);

            // Assert
            builder.UnitOfWorkMock.Verify(uow => uow.StartTransaction(It.IsAny<IsolationLevel>()), Times.Once);
            builder.UnitOfWorkMock.Verify(uow => uow.CommitTransaction(), Times.Once);
        }

        #region Helpers

        private class SutBuilder
        {
            public int CardId { get; private set; }
            public bool IsCardLocked { get; private set; }
            public decimal InitialCardBalance { get; set; }
            public int OperationId { get; set; }            

            public Mock<ICardRepository> CardRepoMock { get; private set; }
            public Mock<IOperationRepository> OperRepoMock { get; private set; }
            public Mock<IUnitOfWork> UnitOfWorkMock { get; private set; }

            public SutBuilder(int cardId, bool isCardLocked = false)
            {
                CardId = cardId;
                IsCardLocked = isCardLocked;
                CardRepoMock = new Mock<ICardRepository>();
                OperRepoMock = new Mock<IOperationRepository>();
                UnitOfWorkMock = new Mock<IUnitOfWork>();
            }

            public CardManagerTestable Build()
            {
                CardRepoMock.Setup(cr => cr.GetBalanceAsync(It.IsAny<int>()))
                    .Returns(Task.FromResult(InitialCardBalance));

                OperRepoMock.Setup(
                or =>
                    or.LogOperationAsync(
                        It.IsAny<int>(),
                        It.IsAny<decimal>(),
                        It.IsAny<OperationCode>(),
                        It.IsAny<DateTime>())).Returns(Task.FromResult(OperationId));

                return new CardManagerTestable(
                    CardRepoMock.Object,
                    OperRepoMock.Object,
                    UnitOfWorkMock.Object,
                    CardId,
                    IsCardLocked);
            }
        }

        private class CardManagerTestable : CardManager
        {
            private readonly int _cardId;
            private readonly bool _isCardLocked;

            public CardManagerTestable(ICardRepository cardRepo, IOperationRepository operationRepo, IUnitOfWork uow, int cardId, bool isCardLocked)
                : base(Mock.Of<IUserStore<Card, int>>(), cardRepo, operationRepo, uow)
            {
                _cardId = cardId;
                _isCardLocked = isCardLocked;
            }

            public override Task<Card> FindByIdAsync(int cardId)
            {
                var card = new Card
                           {
                               Id = _cardId,
                               IsLocked = _isCardLocked
                           };

                return Task.FromResult(card);
            }
        }
        #endregion
    }
}
