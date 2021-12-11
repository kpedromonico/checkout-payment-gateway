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
    class CardEntityTypeConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.UserId)
                .IsRequired();

            builder.Property(t => t.SecurityNumber)
                .IsRequired();

            builder.Property(t => t.CardNumber)
                .IsRequired();

            builder.Property(t => t.ExpiryYear)
                .IsRequired();

            builder.Property(t => t.ExpiryMonth)
                .IsRequired();            
        }
    }
}
