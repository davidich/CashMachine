namespace CashMachine.Domain.Abstractions
{
    using System;
    using System.Threading.Tasks;

    using CashMachine.Domain.Entities;

    public interface IOperationRepository
    {
        Task<int> LogOperationAsync(int cardId, decimal amount, OperationCode code, DateTime date);

        Task<Operation> GetOperationAsync(int operationId);
    }
}