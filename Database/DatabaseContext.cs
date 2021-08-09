using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Database
{
    public class DatabaseContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;
        protected readonly IConfiguration _configuration;

        public DatabaseContext(IConfiguration configuration, ILoggerFactory loggerFactory, ILogger<DatabaseContext> logger)
        {
            _loggerFactory = loggerFactory;
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            base.OnConfiguring(options);
            options.UseSqlServer(_configuration.GetConnectionString("WebApiDatabase"));

            options.UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssemblyWithServiceInjection(typeof(DatabaseContext).Assembly);
        }
        public DbSet<ExchangeTransaction> ExchangeTransactions { get; set; }
    }
}
