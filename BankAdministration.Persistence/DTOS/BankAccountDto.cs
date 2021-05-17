using System;
using System.Collections.Generic;
using System.Text;
using BankAdministration.Persistence.Models;

namespace BankAdministration.Persistence.DTOS
{
    public class BankAccountDto
    {
		public Int32 Id { get; set; }

		public String Number { get; set; }

		public Int64 Balance { get; set; }

		public Boolean IsLocked { get; set; }

		public DateTime CreatedDate { get; set; }

		public string UserId { get; set; }

		public static explicit operator BankAccount(BankAccountDto dto) => new BankAccount
		{
			Id = dto.Id,
			Number = dto.Number,
			Balance = dto.Balance,
			IsLocked = dto.IsLocked,
			CreatedDate = dto.CreatedDate,
			UserId = dto.UserId
        };

		public static explicit operator BankAccountDto(BankAccount b) => new BankAccountDto
		{
			Id = b.Id,
			Number = b.Number,
			Balance = b.Balance,
			IsLocked = b.IsLocked,
			CreatedDate = b.CreatedDate,
			UserId = b.UserId
		};
	}
}
