namespace CashMachine.Dal.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using CashMachine.Domain;
    using CashMachine.Domain.Entities;

    using Microsoft.AspNet.Identity;

    public partial class CardRepository : IUserTwoFactorStore<Card, int>
    {
        public Task SetTwoFactorEnabledAsync(Card user, bool enabled)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(Card user)
        {
            return Task.FromResult(false);            
        }         
    }
}