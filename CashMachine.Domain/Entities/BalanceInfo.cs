namespace CashMachine.Domain.Entities
{
    using System;

    public class BalanceInfo
    {
        public string CardNumber { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }
    }
}