namespace CashMachine.Domain.Abstractions
{
    using System.Threading.Tasks;

    public interface ICardRepository
    {
        Task<decimal> GetBalanceAsync(int cardId);
        Task SetBalanceAsync(int cardId, decimal amount);
    }
}