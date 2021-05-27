using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

namespace BankAdministration.Persistence.Models
{
	public class BankAdministrationDbContext : IdentityDbContext<User, IdentityRole<string>, string>
    {
        public BankAdministrationDbContext(DbContextOptions<BankAdministrationDbContext> options) : base(options) { }

        public DbSet<BankAccount> BankAccounts { get; set; }
    
        public DbSet<Transaction> Transactions { get; set; }
    }
}
