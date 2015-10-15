namespace CashMachine.Dal
{
    using System.Data.Entity;

    using CashMachine.Dal.Migrations;
    using CashMachine.Domain;
    using CashMachine.Domain.Entities;

    public class CashMachineContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public CashMachineContext()
            : base("DefaultConnection")
        {
            
        }

        public static CashMachineContext Create()
        {
            return new CashMachineContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Account
            modelBuilder
                .Entity<Account>()
                .Property(acc => acc.OwnerName)
                .IsRequired();

            modelBuilder
                .Entity<Account>()
                .Property(acc => acc.Amount)
                .HasPrecision(18, 2);

            // Card
            modelBuilder
                .Entity<Card>()
                .HasRequired(c => c.Account)
                .WithMany()
                .HasForeignKey(c => c.AccountId);

            modelBuilder
                .Entity<Card>()
                .Property(c => c.Number)
                .HasMaxLength(16)
                .IsRequired();

            modelBuilder
                .Entity<Card>()
                .Property(c => c.PinHash)
                .IsRequired();
        }
    }
}