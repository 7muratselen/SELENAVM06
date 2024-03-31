using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SELENAVM06.Entities;
using Serilog;
using System;
using System.Collections.Generic;

namespace SELENAVM06.Data
{
    public class SelenavmDbContext : DbContext
    {
        public DbSet<UrunlerXMLS> UrunlerXMLS { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public SelenavmDbContext(DbContextOptions<SelenavmDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("AzureSQLSelenavmConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UrunlerXMLS>(entity =>
            {
                entity.Property(e => e.UrunKodu).HasMaxLength(50);
                entity.Property(e => e.UrunBarkodu).HasMaxLength(50);
                entity.Property(e => e.ParentChild).IsRequired(); // True False değerini alır.
                entity.Property(e => e.UrunAdi).HasMaxLength(255);
                entity.Property(e => e.UrunMarka).HasMaxLength(50);
                entity.Property(e => e.UrunAdeti).HasColumnType("int");
                entity.Property(e => e.UrunFiyati).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.VaryantKodu).HasMaxLength(50);
                entity.Property(e => e.UrunRengi).HasMaxLength(25);
                entity.Property(e => e.UrunModeli).HasMaxLength(25);
            });

            modelBuilder.Entity<Logs>(entity =>
            {
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.Level).HasMaxLength(10);
                entity.Property(e => e.Message).IsRequired();
                entity.Property(e => e.Source).HasMaxLength(255);
            });
        }
    }
}
