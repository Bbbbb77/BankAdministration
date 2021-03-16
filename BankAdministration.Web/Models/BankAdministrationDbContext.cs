using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BankAdministration.Web.Models
{
	public class BankAdministrationDbContext : DbContext
	{
        public BankAdministrationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<BankAccount> BankAccounts { get; set; }
    
        public DbSet<Transaction> Transactions { get; set; }
    }
}
