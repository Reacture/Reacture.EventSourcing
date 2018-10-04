﻿namespace Khala.EventSourcing.Sql
{
    using Microsoft.EntityFrameworkCore;

    public class EventStoreDbContext : DbContext
    {
        public EventStoreDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Aggregate> Aggregates { get; set; }

        public DbSet<PersistentEvent> PersistentEvents { get; set; }

        public DbSet<PendingEvent> PendingEvents { get; set; }

        public DbSet<UniqueIndexedProperty> UniqueIndexedProperties { get; set; }

        public DbSet<Correlation> Correlations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Aggregate>()
                .HasIndex(x => new
                {
                    x.AggregateType,
                    x.AggregateId,
                })
                .IsUnique();

            modelBuilder
                .Entity<PersistentEvent>()
                .HasIndex(x => new
                {
                    x.AggregateType,
                    x.AggregateId,
                    x.Version,
                })
                .IsUnique();

            modelBuilder
                .Entity<PendingEvent>()
                .HasKey(x => new
                {
                    x.AggregateType,
                    x.AggregateId,
                    x.Version,
                });

            modelBuilder.Entity<UniqueIndexedProperty>().HasKey(nameof(UniqueIndexedProperty.AggregateType), nameof(UniqueIndexedProperty.PropertyName), nameof(UniqueIndexedProperty.PropertyValue));

            modelBuilder.Entity<UniqueIndexedProperty>().HasIndex(nameof(UniqueIndexedProperty.AggregateId), nameof(UniqueIndexedProperty.PropertyName)).IsUnique();

            modelBuilder.Entity<Correlation>().HasKey(nameof(Correlation.AggregateId), nameof(Correlation.CorrelationId));
        }
    }
}
