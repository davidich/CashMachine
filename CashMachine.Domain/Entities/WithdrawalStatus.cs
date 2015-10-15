namespace CashMachine.Domain.Entities
{
    public enum WithdrawalStatus
    {
        Success,
        CardIsLocked,
        TooBigAmount
    }
}