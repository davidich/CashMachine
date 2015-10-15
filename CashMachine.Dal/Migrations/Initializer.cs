namespace CashMachine.Dal.Migrations
{
    using System.Data.Entity;

    public class Initializer : DropCreateDatabaseIfModelChanges<CashMachineContext>
    {
        protected override void Seed(CashMachineContext context)
        {
            DataSeeder.Seed(context);            
        }
    }
}