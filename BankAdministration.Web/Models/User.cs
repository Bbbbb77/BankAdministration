using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankAdministration.Web.Models
{
	public class User
	{
		[Key]
		public Int32 Id { get; set; }

		[Required]
		[MaxLength(20)]	
		public string FirstName { get; set; }

		[Required]
		[MaxLength(20)]
		public string LastName { get; set; }
		
		[Required]
		[MaxLength(6)]
		[MinLength(6)]
		public int PinCode { get; set; }

		[Required]
		[MaxLength(10)]
		public string Password { get; set; }

		[Required]
		[MinLength(1)]
		public ICollection<BankAccount> BankAccounts { get; set; }

	}
}
