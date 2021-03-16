using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BankAdministration.Web.Models
{
	public class User : IdentityUser
	{
		[Required]
		[MaxLength(20)]	
		public string FirstName { get; set; }

		[Required]
		[MaxLength(20)]
		public string LastName { get; set; }
		
		[Required]
		[MaxLength(6)]
		[MinLength(6)]
		public int Pincode { get; set; }

        [Required]
		[MinLength(1)]
		public ICollection<BankAccount> BankAccounts { get; set; }
	}
}
