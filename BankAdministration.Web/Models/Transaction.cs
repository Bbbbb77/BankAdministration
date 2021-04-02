using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


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

        public string SourceAccountNumber { get; set; }

        public string DestinationAccountNumber { get; set; }

        public Int64 Amount { get; set; }

        public Int64 OldBalance { get; set; }

        public Int64 NewBalance { get; set; }

        public DateTime TransactionTime { get; set; }

        [Required]
        [DisplayName("BankAccount")]
        public Int32 BankAccountId { get; set; }

        public virtual BankAccount BankAccount { get; set; }
    }
}
