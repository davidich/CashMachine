namespace CashMachine.Dal.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using CashMachine.Domain;
    using CashMachine.Domain.Entities;

    using Microsoft.AspNet.Identity;

    public partial class CardRepository : IUserPasswordStore<Card, int>
    {
        public async Task SetPasswordHashAsync(Card card, string pinHash)
        {
            card.PinHash = pinHash;
            _context.Entry(card).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public Task<string> GetPasswordHashAsync(Card user)
        {

            return Task.FromResult(user.PinHash);
        }

        public Task<bool> HasPasswordAsync(Card user)
        {
            bool hasPassword = string.IsNullOrWhiteSpace(user.PinHash) == false;
            return Task.FromResult(hasPassword);
        }
    }
}