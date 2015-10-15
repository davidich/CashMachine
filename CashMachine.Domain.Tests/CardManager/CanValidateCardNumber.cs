namespace CashMachine.Domain.Tests.CardManager
{
    using System.Threading.Tasks;

    using CashMachine.Domain.Entities;
    using CashMachine.Domain.Managers;

    using Microsoft.AspNet.Identity;

    using Moq;

    using Xunit;

    public class CanValidateCardNumber
    {
        private const string ValidCardNumber = "0123012301230123";
        private const string InvalidCardNumber = "4321432143214321";

        [Fact]
        public async Task ProperlyHandlesValidCard()
        {
            // Arrange
            var sut = new CardManagerTestable(isCardLocked: false);

            // Act
            var validationResult = await sut.ValidateNumberAsync(ValidCardNumber);

            // Assert
            Assert.Equal(CardNumberValidationResult.Ok, validationResult);
        }

        [Fact]
        public async Task ProperlyHandlesLockedCard()
        {
            // Arrange
            var sut = new CardManagerTestable(isCardLocked: true);

            // Act
            var validationResult = await sut.ValidateNumberAsync(ValidCardNumber);

            // Assert
            Assert.Equal(CardNumberValidationResult.CardNotFoundOrLockedOut, validationResult);
        }

        [Fact]
        public async Task ProperlyHandlesWrongNumber()
        {
            // Arrange
            var sut = new CardManagerTestable();

            // Act
            var validationResult = await sut.ValidateNumberAsync(InvalidCardNumber);

            // Assert
            Assert.Equal(CardNumberValidationResult.CardNotFoundOrLockedOut, validationResult);
        }

        #region Helpers

        private class CardManagerTestable : CardManager
        {
            private readonly bool _isCardLocked;

            public CardManagerTestable(bool isCardLocked = false)
                : base(Mock.Of<IUserStore<Card, int>>(), null, null, null)
            {
                _isCardLocked = isCardLocked;
            }

            protected override Task<Card> FindByCardNumberAsync(string cardNumber)
            {
                var result = cardNumber == ValidCardNumber
                    ? new Card { Number = cardNumber, IsLocked = _isCardLocked }
                    : null;

                return Task.FromResult(result);
            }
        }

        #endregion
    }
}
