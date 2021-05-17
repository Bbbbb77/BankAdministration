using System;
using System.Collections.Generic;
using System.Text;
using BankAdministration.Persistence.Models;

namespace BankAdministration.Persistence.DTOS
{
    public class TransactionDto
    {
        public Int32 Id { get; set; }

        public TransactionTypeEnum TransactionType { get; set; }

        public string SourceAccountNumber { get; set; }

        public string DestinationAccountNumber { get; set; }

        public String DestinationAccountUserName { get; set; }

        public Int64 Amount { get; set; }

        public Int64 OldBalance { get; set; }

        public Int64 NewBalance { get; set; }

        public DateTime TransactionTime { get; set; }

        public Int32 BankAccountId { get; set; }

        public static explicit operator Transaction(TransactionDto dto) => new Transaction
        {
            Id = dto.Id,
            TransactionType = dto.TransactionType,
            SourceAccountNumber = dto.SourceAccountNumber,
            DestinationAccountNumber = dto.DestinationAccountNumber,
            DestinationAccountUserName = dto.DestinationAccountUserName,
            Amount = dto.Amount,
            OldBalance = dto.OldBalance,
            NewBalance = dto.NewBalance,
            TransactionTime = dto.TransactionTime,
            BankAccountId = dto.BankAccountId
        };

        public static explicit operator TransactionDto(Transaction t) => new TransactionDto
        {
            Id = t.Id,
            TransactionType = t.TransactionType,
            SourceAccountNumber = t.SourceAccountNumber,
            DestinationAccountNumber = t.DestinationAccountNumber,
            DestinationAccountUserName = t.DestinationAccountUserName,
            Amount = t.Amount,
            OldBalance = t.OldBalance,
            NewBalance = t.NewBalance,
            TransactionTime = t.TransactionTime,
            BankAccountId = t.BankAccountId
        };
    }
}
