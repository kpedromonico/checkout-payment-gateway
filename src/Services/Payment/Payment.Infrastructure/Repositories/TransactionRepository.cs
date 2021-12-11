using Microsoft.EntityFrameworkCore;
using Payment.Domain.AggregatesModel.TransactionAggregate;
using Payment.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly PaymentDbContext _context;

        public TransactionRepository(PaymentDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public Transaction Add(Transaction transaction)
        {
            return _context.Transactions.Add(transaction).Entity;
        }        

        public async Task<Transaction> FindByIdAsync(Guid id)
        {
            return await _context.Transactions
                .Include(t => t.Card)
                .Where(t => t.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Transaction>> GetByUserIdAsync(string userId)
        {
            return await _context.Transactions
                .Include(t => t.Card)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        public Transaction Update(Transaction transaction)
        {
            return _context.Transactions.Update(transaction).Entity;
        }
    }
}
