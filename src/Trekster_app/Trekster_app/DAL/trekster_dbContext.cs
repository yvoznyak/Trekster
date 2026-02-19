// <copyright file="trekster_dbContext.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Trekster_app
{
    using System.Configuration;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Db context class.
    /// </summary>
    public partial class TreksterDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreksterDbContext"/> class.
        /// </summary>
        public TreksterDbContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TreksterDbContext"/> class.
        /// </summary>
        /// <param name="options"> Options. </param>
        public TreksterDbContext(DbContextOptions<TreksterDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets account properties.
        /// </summary>
        public virtual DbSet<Account> Accounts { get; set; } = null!;

        /// <summary>
        /// Gets or sets category properties.
        /// </summary>
        public virtual DbSet<Category> Categories { get; set; } = null!;

        /// <summary>
        /// Gets or sets currency properties.
        /// </summary>
        public virtual DbSet<Currency> Currencies { get; set; } = null!;

        /// <summary>
        /// Gets or sets start balance properties.
        /// </summary>
        public virtual DbSet<Startbalance> Startbalances { get; set; } = null!;

        /// <summary>
        /// Gets or sets transaction properties.
        /// </summary>
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;

        /// <summary>
        /// Override func.
        /// </summary>
        /// <param name="optionsBuilder"> Options builder. </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //var connectionStr = ConfigurationManager.ConnectionStrings["trekster_db"].ToString();
                var connectionStr = "server=localhost;user id=postgres;database=trekster_db;port=5432;password=111;";
                optionsBuilder.UseNpgsql(connectionStr);
            }
        }

        /// <summary>
        /// Override func.
        /// </summary>
        /// <param name="modelBuilder"> Model builder. </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("accounts");

                entity.HasIndex(e => e.Name, "accounts_name_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("categories");

                entity.HasIndex(e => e.Name, "categories_name_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("currencies");

                entity.HasIndex(e => e.Name, "currencies_name_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Startbalance>(entity =>
            {
                entity.ToTable("startbalances");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Idaccount).HasColumnName("idaccount");

                entity.Property(e => e.Idcurrency).HasColumnName("idcurrency");

                entity.Property(e => e.Sum).HasColumnName("sum");

                entity.HasOne(d => d.IdaccountNavigation)
                    .WithMany(p => p.Startbalances)
                    .HasForeignKey(d => d.Idaccount)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("startbalances_idaccount_fkey");

                entity.HasOne(d => d.IdcurrencyNavigation)
                    .WithMany(p => p.Startbalances)
                    .HasForeignKey(d => d.Idcurrency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("startbalances_idcurrency_fkey");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transactions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date");

                entity.Property(e => e.Idaccount).HasColumnName("idaccount");

                entity.Property(e => e.Idcategory).HasColumnName("idcategory");

                entity.Property(e => e.Idcurrency).HasColumnName("idcurrency");

                entity.Property(e => e.Note)
                    .HasMaxLength(100)
                    .HasColumnName("note");

                entity.Property(e => e.Sum).HasColumnName("sum");

                entity.HasOne(d => d.IdaccountNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.Idaccount)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("transactions_idaccount_fkey");

                entity.HasOne(d => d.IdcategoryNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.Idcategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("transactions_idcategory_fkey");

                entity.HasOne(d => d.IdcurrencyNavigation)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.Idcurrency)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("transactions_idcurrency_fkey");
            });

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
