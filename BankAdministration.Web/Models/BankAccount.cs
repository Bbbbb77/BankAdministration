using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankAdministration.Web.Models
{
	public class BankAccount
	{
		[Key]
		public Int32 Id { get; set; }
		
		[Required]
		[MaxLength(10)]
		[MinLength(10)]
		public String Number { get; set; }

        public Int64 Balance { get; set; }

        public Boolean IsLocked  { get; set; }

		[Required]
        public DateTime CreatedDate { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

		[Required]
		[DisplayName("User")]
		public string UserId { get; set; }
        
		public virtual User User { get; set; }
	}
}
