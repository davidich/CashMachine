namespace CashMachine.Dal.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using CashMachine.Domain;
    using CashMachine.Domain.Abstractions;
    using CashMachine.Domain.Entities;

    public class OperationRepository : IOperationRepository
    {
        private readonly CashMachineContext _context;

        public OperationRepository(CashMachineContext context)
        {
            _context = context;
        }

        public async Task<int> LogOperationAsync(int cardId, decimal amount, OperationCode code, DateTime date)
        {
            var operation = new Operation
                            {
                                CardId = cardId,
                                Amount = amount,
                                Code = code,
                                Date = date
                            };

            _context.Operations.Add(operation);

            await _context.SaveChangesAsync();

            return operation.Id;
        }

        public async Task<Operation> GetOperationAsync(int id)
        {
            return await _context.Operations.Include(op => op.Card)
                .SingleOrDefaultAsync(op => op.Id == id);
        }
    }
}