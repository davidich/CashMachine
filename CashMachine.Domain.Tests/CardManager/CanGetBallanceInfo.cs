namespace CashMachine.Domain.Tests.CardManager
{
    using System.Threading.Tasks;

    using CashMachine.Domain.Abstractions;
    using CashMachine.Domain.Entities;
    using CashMachine.Domain.Managers;

    using Microsoft.AspNet.Identity;

    using Moq;

    using Xunit;

    public class CanGetBalanceInfo
    {
        private readonly CardIdentity _cardIdentity;

        public CanGetBalanceInfo()
        {
            _cardIdentity = new CardIdentity(123, "0123012301230123");
            TestableDateTime.Init();
        }

        [Fact]
        public async Task QueriesBallanceForProperCard()
        {
            // Arrange
            var cardRepo = new Mock<ICardRepository>();
            var sut = CreateSut(cardRepo.Object);

            // Act
            await sut.GetBalanceInfo(_cardIdentity);

            // Assert
            cardRepo.Verify(repo => repo.GetBalanceAsync(_cardIdentity.Id));
        }

        [Fact]
        public async Task ReturnsBallanceForProperCard()
        {
            // Arrange
            const decimal Balance = 100500.00M;

            var cardRepo = new Mock<ICardRepository>();
            cardRepo.Setup(repo => repo.GetBalanceAsync(_cardIdentity.Id))
                .Returns(Task.FromResult(Balance));

            var sut = CreateSut(cardRepo.Object);

            // Act
            var info = await sut.GetBalanceInfo(_cardIdentity);

            // Assert
            Assert.Equal(Balance, info.Amount);
            Assert.Equal(_cardIdentity.Number, info.CardNumber);
        }

        [Fact]
        public async Task ReturnsInfoWithProperDate()
        {
            // Arrange            
            var sut = CreateSut();

            // Act
            var info = await sut.GetBalanceInfo(_cardIdentity);

            // Assert
            Assert.Equal(TestableDateTime.SystemNow, info.Date);
        }

        [Fact]
        public async Task DoesProperLogging()
        {
            // Arrange           
            var cardId = _cardIdentity.Id;
            const decimal Balance = 100500.00M;
            var cardRepo = Mock.Of<ICardRepository>(cr => cr.GetBalanceAsync(cardId) == Task.FromResult(Balance));

            var operationRepo = new Mock<IOperationRepository>();

            var sut = CreateSut(cardRepo, operationRepo.Object);

            // Act
            await sut.GetBalanceInfo(_cardIdentity);

            // Assert
            operationRepo.Verify(o => o.LogOperationAsync(cardId, Balance, OperationCode.ViewBalance, TestableDateTime.SystemNow));
        }

        #region Helpers
        private CardManager CreateSut()
        {
            var cardRepo = Mock.Of<ICardRepository>();
            return CreateSut(cardRepo);
        }

        private CardManager CreateSut(ICardRepository cardRepository)
        {
            var operationRepo = Mock.Of<IOperationRepository>();
            return CreateSut(cardRepository, operationRepo);
        }

        private CardManager CreateSut(ICardRepository cardRepository, IOperationRepository operationRepository)
        {
            var userStore = Mock.Of<IUserStore<Card, int>>();
            var unitOfWork = Mock.Of<IUnitOfWork>();
            return new CardManager(userStore, cardRepository, operationRepository, unitOfWork);
        }
        #endregion
    }
}
