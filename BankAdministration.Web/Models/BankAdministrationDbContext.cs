using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BankAdministration.Web.Models
{
	public class BankAdministrationDbContext : IdentityDbContext<User>
    {
        public BankAdministrationDbContext(DbContextOptions<BankAdministrationDbContext> options) : base(options) { }

        public DbSet<BankAccount> BankAccounts { get; set; }
    
        public DbSet<Transaction> Transactions { get; set; }
    }
}
