namespace CashMachine.Dal.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using CashMachine.Domain;
    using CashMachine.Domain.Entities;

    using Microsoft.AspNet.Identity;

    public partial class CardRepository : IUserStore<Card, int>
    {
        public async Task CreateAsync(Card card)
        {
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Card card)
        {
            _context.Entry(card).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Card card)
        {
            _context.Entry(card).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<Card> FindByIdAsync(int cardId)
        {
            return await _context.Cards.SingleOrDefaultAsync(c => c.Id == cardId);
        }

        public async Task<Card> FindByNameAsync(string cardNumber)
        {
            return await _context.Cards.SingleOrDefaultAsync(c => c.Number == cardNumber);
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }
    }
}