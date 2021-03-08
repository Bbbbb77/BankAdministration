﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BankAdministration.Web.Models
{
	public class BankAccount
	{
		[Key]
		public Int32 Id { get; set; }
		
		[Required]
		[MaxLength(24)]
		[MinLength(24)]
		public Int32 number { get; set; }

        public Int32 Balance { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

		public Int32 UserId { get; set; }
        [Required]
		public virtual User User { get; set; }
	}
}