using Payment.API.Infrastructure.Services;
using Payment.Domain.AggregatesModel.TransactionAggregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Payment.API.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IBankService _bankService;

        public PaymentService(ITransactionRepository transactionRepository, IBankService bankService)
        {
            _transactionRepository = transactionRepository;
            _bankService = bankService;
        }

        public async Task<Transaction> FindTransactionAsync(Guid paymentId)
        {
            return await _transactionRepository.FindByIdAsync(paymentId);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsAsync(string userId)
        {
            return await _transactionRepository.GetByUserIdAsync(userId);
        }

        public async Task<Transaction> ProcessTransactionAsync(Transaction transaction, string userId)
        {
            // Call bank service for validation -> This could be a gRPC call
            var approved = await _bankService.ProcessTransaction(transaction);

            if (approved.HasValue) 
            {
                if (approved.Value)
                {
                    transaction.ApproveTransaction();
                }
                else
                {
                    transaction.RejectTransaction();
                }
            }
            else
            {
                // bank service is unresponsive, send it to a message queue for background processing (to attempt it once again)
            }

            transaction.SetTransactionOwner(userId);

            _transactionRepository.Add(transaction);
            await _transactionRepository.UnitOfWork.SaveChangesAsync();

            return transaction;
        }
    }
}
