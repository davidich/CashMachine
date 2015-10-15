namespace CashMachine.Domain.Entities
{
    /// <summary>
    /// Returns information about withdrawal request
    /// </summary>
    public class WithdrawalResult
    {
        /// <summary>
        /// Operation status
        /// </summary>
        public WithdrawalStatus Status { get; set; }

        /// <summary>
        /// Id of successful operation (otherwise 0)
        /// </summary>
        public int OperationId { get; set; }
    }
}