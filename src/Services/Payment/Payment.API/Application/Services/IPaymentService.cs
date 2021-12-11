using Payment.Domain.AggregatesModel.TransactionAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payment.API.Application.Services
{
    public interface IPaymentService
    {
        Task<Transaction> FindTransactionAsync(Guid paymentId);

        Task<IEnumerable<Transaction>> GetTransactionsAsync(string userId);

        Task<Transaction> ProcessTransactionAsync(Transaction transaction, string userId);
    }
}
