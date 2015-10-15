namespace CashMachine.Domain.TestableDateTime
{
    using System;

    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}