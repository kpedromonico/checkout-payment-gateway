using Microsoft.EntityFrameworkCore;
using Payment.Domain.AggregatesModel.TransactionAggregate;
using Payment.Domain.SeedWork;
using Payment.Infrastructure.EntityConfigurations;

namespace Payment.Infrastructure
{
    public class PaymentDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Card> Cards { get; set; }

        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CardEntityTypeConfiguration());
            builder.ApplyConfiguration(new TransactionEntityTypeConfiguration());
        }
    }
}
