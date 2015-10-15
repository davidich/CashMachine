namespace CashMachine.Dal.Migrations
{
    using System.Globalization;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using CashMachine.Domain.Entities;

    using Microsoft.AspNet.Identity;

    public class DataSeeder
    {
        private readonly CashMachineContext _context;

        private DataSeeder(CashMachineContext context)
        {
            _context = context;
        }

        public static void Seed(CashMachineContext context)
        {
            var seeder = new DataSeeder(context);
            seeder.Seed();
        }

        private void Seed()
        {
            var account = SeedAccount();
            SeedCards(account);
        }

        private Account SeedAccount()
        {
            const string UserName = "User 1";

            _context.Accounts.AddOrUpdate(
                acc => acc.OwnerName,
                new Account { OwnerName = UserName, Amount = 100500.00M });

            _context.SaveChanges();

            return _context.Accounts.Single(acc => acc.OwnerName == UserName);
        }

        private void SeedCards(Account account)
        {
            var hasher = new PasswordHasher();

            for (int i = 1; i <= 100; i++)
            {
                var s = i.ToString(CultureInfo.InvariantCulture);
                var number = s.PadLeft(16, '0');
                var pin = s.PadLeft(4, '0');

                _context.Cards.AddOrUpdate(
                p => p.Number,
                new Card
                {
                    AccountId = account.Id,
                    Number = number,
                    PinHash = hasher.HashPassword(pin)
                });
            }
            
            _context.SaveChanges();
        }
    }
}