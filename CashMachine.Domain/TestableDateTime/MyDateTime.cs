namespace CashMachine.Domain.TestableDateTime
{
    using System;

    public static class MyDateTime
    {
        private static IDateTimeProvider provider;

        public static IDateTimeProvider Provider
        {
            private get
            {
                return provider ?? (provider = new DefaultDateTimeProvider());
            }
            set
            {
                provider = value;
            }
        }

        public static DateTime Now
        {
            get
            {
                return Provider.Now;
            }
        }
    }
}