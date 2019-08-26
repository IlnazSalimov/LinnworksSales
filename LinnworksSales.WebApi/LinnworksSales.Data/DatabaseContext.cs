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
        public DbSet<Sale> Orders { get; set; }
        public DbSet<Country> Countries { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
