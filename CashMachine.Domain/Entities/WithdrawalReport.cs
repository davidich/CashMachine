namespace CashMachine.Domain.Entities
{
    using System;

    public class WithdrawalReport
    {
        public string CardNumber { get; set; } 

        public DateTime Date { get; set; }

        public decimal RequestedAmount { get; set; }

        public decimal Balance { get; set; }
    }
}