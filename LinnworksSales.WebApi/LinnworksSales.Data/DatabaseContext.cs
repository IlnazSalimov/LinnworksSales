using LinnworksSales.Data.Enums;
using LinnworksSales.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace LinnworksSales.Data
{
    /// <summary>
    /// A DatabaseContext instance that can be used to query from a database 
    /// and group together changes that will then be written back to the store as a unit
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }

        public DatabaseContext()
            : base()
        { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
            .Entity<Sale>()
            .Property(e => e.SalesChanel)
            .HasConversion(
                v => v.ToString(),
                v => (SalesChanel)Enum.Parse(typeof(SalesChanel), v));

            modelBuilder
            .Entity<Sale>()
            .Property(e => e.OrderPriority)
            .HasConversion(
                v => v.ToString(),
                v => (OrderPriority)Enum.Parse(typeof(OrderPriority), v));
        }
    }
}
