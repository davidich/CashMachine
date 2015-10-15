namespace CashMachine.Domain.Entities
{
    using System;

    public class Operation
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        public Card Card { get; set; }

        public OperationCode Code { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}