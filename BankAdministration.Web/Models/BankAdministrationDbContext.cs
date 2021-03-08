using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BankAdministration.Web.Models
{
	public class BankAdministrationDbContext : DbContext
	{
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
			///optionsBuilder.UseSqlServer();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<BankAccount> BankAccounts { get; set; }
    
        public DbSet<Transaction> TransActions { get; set; }
    }
}
