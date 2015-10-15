namespace CashMachine.Dal.Repositories
{
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using CashMachine.Domain;
    using CashMachine.Domain.Abstractions;

    public partial class CardRepository : ICardRepository
    {
        private readonly CashMachineContext _context;

        public CardRepository(CashMachineContext context)
        {
            _context = context;
        }

        // ------------------------------------------------------------------------------------------------------------------------
        // --------------------------------------------- !!! Important !!!---------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------------------
        // CardRepository implements quite a few interfaces (IUserStore<Card,int>, IUserLockoutStore<Card,int> and so on)         |
        // To see actual interface implementations open nested files
        // ------------------------------------------------------------------------------------------------------------------------

        public async Task<decimal> GetBalanceAsync(int cardId)
        {
            return await (
                from card in _context.Cards
                where card.Id == cardId
                select card.Account.Amount).SingleAsync();
        }

        public async Task SetBalanceAsync(int cardId, decimal amount)
        {
            var account = await (from card in _context.Cards
                                 where card.Id == cardId
                                 select card.Account).SingleAsync();

            account.Amount = amount;

            await _context.SaveChangesAsync();
        }
    }
}