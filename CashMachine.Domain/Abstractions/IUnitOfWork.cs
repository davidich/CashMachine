namespace CashMachine.Domain.Abstractions
{
    using System.Data;

    public interface IUnitOfWork
    {
        void StartTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

        void RollbackTransaction();

        void CommitTransaction();
    }
}