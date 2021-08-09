using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database
{
    public class ExchangeTransactionConfiguration : IEntityTypeConfiguration<ExchangeTransaction>
    {
        public void Configure(EntityTypeBuilder<ExchangeTransaction> builder)
        {
            builder
                .HasKey(e => e.Id);
        }
    }
}
