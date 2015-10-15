namespace CashMachine.Dal.Repositories
{
    using System;
    using System.Threading.Tasks;

    using CashMachine.Domain;
    using CashMachine.Domain.Entities;

    using Microsoft.AspNet.Identity;

    public partial class CardRepository : IUserLockoutStore<Card, int>
    {
        public Task<DateTimeOffset> GetLockoutEndDateAsync(Card card)
        {
            return card.IsLocked
                ? Task.FromResult(DateTimeOffset.MaxValue)
                : Task.FromResult(DateTimeOffset.MinValue);
        }

        public async Task SetLockoutEndDateAsync(Card card, DateTimeOffset lockoutEnd)
        {
            card.IsLocked = true;
            await _context.SaveChangesAsync();            
        }

        public async Task<int> IncrementAccessFailedCountAsync(Card card)
        {
            card.PinFailures = card.PinFailures + 1;
            await _context.SaveChangesAsync();

            return card.PinFailures;
        }

        public async Task ResetAccessFailedCountAsync(Card card)
        {
            card.PinFailures = 0;
            await _context.SaveChangesAsync();
        }

        public Task<int> GetAccessFailedCountAsync(Card card)
        {
            return Task.FromResult(card.PinFailures);
        }

        public Task<bool> GetLockoutEnabledAsync(Card card)
        {
            return Task.FromResult(true);
        }

        public Task SetLockoutEnabledAsync(Card user, bool enabled)
        {
            return Task.FromResult(0);
        }        
    }
}