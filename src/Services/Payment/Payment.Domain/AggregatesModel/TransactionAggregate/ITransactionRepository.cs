using Payment.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payment.Domain.AggregatesModel.TransactionAggregate
{
    public interface ITransactionRepository : IRepository<Transaction>
    {
        Transaction Add(Transaction transaction);

        Transaction Update(Transaction transaction);

        Task<IEnumerable<Transaction>> GetByUserIdAsync(string userId);

        Task<Transaction> FindByIdAsync(Guid id);
    }
}
