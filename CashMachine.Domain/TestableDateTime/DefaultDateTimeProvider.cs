namespace CashMachine.Domain.TestableDateTime
{
    using System;

    public class DefaultDateTimeProvider : IDateTimeProvider
    {
        DateTime IDateTimeProvider.Now
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}