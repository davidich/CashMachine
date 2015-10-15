namespace CashMachine.Domain.Managers
{
    using System;
    using System.Data;
    using System.Security;
    using System.Threading.Tasks;

    using CashMachine.Domain.Abstractions;
    using CashMachine.Domain.Entities;
    using CashMachine.Domain.TestableDateTime;

    using Microsoft.AspNet.Identity;

    /// <summary>
    /// As we use card information to authenticate users
    /// this class will play a role of a UserManager for Indentity Framework
    /// </summary>
    public class CardManager : UserManager<Card, int>
    {
        private readonly ICardRepository _cardRepository;

        private readonly IOperationRepository _operationRepository;

        private readonly IUnitOfWork _unitOfWork;

        public CardManager(IUserStore<Card, int> store, ICardRepository cardRepository, IOperationRepository operationRepository, IUnitOfWork unitOfWork)
            : base(store)
        {
            _cardRepository = cardRepository;
            _operationRepository = operationRepository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Used by IoC to set up all the props after construction of new instance
        /// </summary>
        public void Configure()
        {
            MaxFailedAccessAttemptsBeforeLockout = 4;
        }

        /// <summary>
        /// Checks if provided card number exists in the system
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public async Task<CardNumberValidationResult> ValidateNumberAsync(string number)
        {
            Card card = await FindByCardNumberAsync(number);

            return card == null || card.IsLocked
                ? CardNumberValidationResult.CardNotFoundOrLockedOut
                : CardNumberValidationResult.Ok;
        }

        /// <summary>
        /// Requests basic infrormation about card (card number, balance and request date) and adds a new entry into operation log
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public async Task<BalanceInfo> GetBalanceInfo(CardIdentity card)
        {
            var amount = await _cardRepository.GetBalanceAsync(card.Id);
            var operationTime = MyDateTime.Now;

            await _operationRepository.LogOperationAsync(card.Id, amount, OperationCode.ViewBalance, operationTime);

            return new BalanceInfo
                   {
                       CardNumber = card.Number,
                       Amount = amount,
                       Date = operationTime
                   };
        }
       
        /// <summary>
        /// Decreases ballance by a specified amount and adds a new entry into operation log
        /// </summary>
        /// <param name="cardId">Card id which is linked to an account</param>
        /// <param name="requestedAmount">An amount requested for withdrawal</param>
        /// <returns></returns>
        public async Task<WithdrawalResult> WithdrawAsync(int cardId, decimal requestedAmount)
        {
            Card card = await FindByIdAsync(cardId);

            if (card.IsLocked)
                return new WithdrawalResult { Status = WithdrawalStatus.CardIsLocked };

            _unitOfWork.StartTransaction(IsolationLevel.RepeatableRead);

            var currentAmount = await _cardRepository.GetBalanceAsync(card.Id);

            if (currentAmount < requestedAmount)
                return new WithdrawalResult { Status = WithdrawalStatus.TooBigAmount };

            decimal newAmount = currentAmount - requestedAmount;
            DateTime operationTime = MyDateTime.Now;
            await _cardRepository.SetBalanceAsync(cardId, newAmount);

            var operationId = await _operationRepository.LogOperationAsync(cardId, requestedAmount, OperationCode.Withdraw, operationTime);

            _unitOfWork.CommitTransaction();

            return new WithdrawalResult
                   {
                       Status = WithdrawalStatus.Success,
                       OperationId = operationId
                   };            
        }

        /// <summary>
        /// Requests basic infrormation about cash withdrawal (card number, withdrawal amount, balance and withdrawal date) and adds a new entry into operation log
        /// </summary>
        /// <param name="curCardId"></param>
        /// <param name="operationId"></param>
        /// <exception cref="SecurityException">Is thrown if user tries to view not his own operation</exception>
        /// <exception cref="InvalidOperationException">Is thrown if user tries to not existing operation</exception>
        /// <returns></returns>
        public async Task<WithdrawalReport> BuildWithdrawReportAsync(int curCardId, int operationId)
        {
            var operation = await _operationRepository.GetOperationAsync(operationId);

            if (operation == null || operation.Code != OperationCode.Withdraw)
                throw new InvalidOperationException();

            if (operation.CardId != curCardId)
                throw new SecurityException();

            var balance = await _cardRepository.GetBalanceAsync(curCardId);

            return new WithdrawalReport
                   {
                       CardNumber = operation.Card.Number,
                       RequestedAmount = operation.Amount,
                       Balance = balance,
                       Date = operation.Date,                       
                   };            
        }

        // Wrapper for more convenient usage (with better name in our domain)
        protected virtual async Task<Card> FindByCardNumberAsync(string cardNumber)
        {
            return await FindByNameAsync(cardNumber);
        }
    }
}