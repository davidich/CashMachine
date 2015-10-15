namespace CashMachine.Domain.Entities
{
    using Microsoft.AspNet.Identity;

    /// <summary>
    /// Class which represent a credit card.
    /// It implements IUser in order to be used as Identity
    /// </summary>
    public class Card : IUser<int>
    {
        public int Id { get; set; }

        string IUser<int>.UserName
        {
            get { return Number; }
            set { Number = value; }
        }

        public string Number { get; set; }

        public string PinHash { get; set; }

        public int PinFailures { get; set; }

        public bool IsLocked { get; set; }

        public int AccountId { get; set; }

        public Account Account { get; set; }        
    }
}
