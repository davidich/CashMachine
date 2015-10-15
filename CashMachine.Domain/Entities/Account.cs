namespace CashMachine.Domain.Entities
{
    public class Account
    {
        public int Id { get; set; }

        public string OwnerName { get; set; }

        public decimal Amount { get; set; }
    }
}