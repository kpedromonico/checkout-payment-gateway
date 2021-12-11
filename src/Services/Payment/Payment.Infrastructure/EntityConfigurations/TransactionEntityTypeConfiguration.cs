using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.Domain.AggregatesModel.TransactionAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Infrastructure.EntityConfigurations
{
    class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Amount)
                    .IsRequired();

            builder.Property(t => t.UserId)
                    .IsRequired();

            builder.Property(t => t.Currency)
                   .IsRequired();

            builder.Property(t => t.Sucess);

            builder.Property(t => t.CreationDate)                
                    .IsRequired();

            builder.Property<Guid>("_cardId")                    
                    .IsRequired();

            builder.HasOne(t => t.Card)
                    .WithMany()
                    .IsRequired()
                    .HasForeignKey("_cardId");
        }
    }
}
