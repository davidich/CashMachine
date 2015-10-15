namespace CashMachine.Domain.Tests
{
    using System;

    using CashMachine.Domain.TestableDateTime;

    using Moq;

    public class TestableDateTime
    {
        public static DateTime SystemNow = new DateTime(2001, 2, 3, 4, 5, 6);

        public static void Init()
        {
            var dateTimeProvider = Mock.Of<IDateTimeProvider>(dp => dp.Now == SystemNow);
            MyDateTime.Provider = dateTimeProvider;
        }        
    }
}