namespace CashMachine.Dal
{
    using System;
    using System.Data;
    using System.Data.Entity;
    using System.Threading.Tasks;

    using CashMachine.Domain.Abstractions;

    public class EfUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CashMachineContext _dbContext;
        private DbContextTransaction _transaction;

        public EfUnitOfWork(CashMachineContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void StartTransaction(IsolationLevel level)
        {
            _transaction = _dbContext.Database.BeginTransaction(level);
        }

        public void RollbackTransaction()
        {
            if(_transaction == null)
                throw new InvalidOperationException();

            _transaction.Rollback();
        }

        public void CommitTransaction()
        {
            if (_transaction == null)
                throw new InvalidOperationException();

            _transaction.Commit();
        }

        public void Dispose()
        {
            if(_transaction != null)
                _transaction.Dispose();
        }
    }
}