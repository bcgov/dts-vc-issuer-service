using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Issuer.Configuration
{
    public abstract class SeededTable<T> : IEntityTypeConfiguration<T> where T : class
    {
        public abstract IEnumerable<T> SeedData { get; }

        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasData(SeedData);
        }
    }
}
