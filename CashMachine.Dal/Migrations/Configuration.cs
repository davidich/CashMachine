namespace CashMachine.Dal.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<CashMachineContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;            
        }

        //  This method will be called after migrating to the latest version.
        protected override void Seed(CashMachineContext context)
        {
            DataSeeder.Seed(context);
        }
    }
}
