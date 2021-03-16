using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BankAdministration.Web.Models
{
    public enum TransactionTypeEnum
    {
        Transfer,
        Deposit,
        Withdrawn
    }

    public class Transaction
    {
        [Key]
        public Int32 Id { get; set; }

        public TransactionTypeEnum TransactionType { get; set; }

        public Int32 SourceAccountNumber { get; set; }

        public Int32 DestinationAccountNumber { get; set; }

        public Int32 Amount { get; set; }

        public Int32 OldBalance { get; set; }

        public Int32 NewBalance { get; set; }

        public DateTime TransactionTime { get; set; }

        public Int32 BankAccountId { get; set; }

        [Required]
        public virtual BankAccount TransactionOwner { get; set; }
    }
}
