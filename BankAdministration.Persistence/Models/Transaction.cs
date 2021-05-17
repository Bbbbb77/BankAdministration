using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;


namespace BankAdministration.Persistence.Models
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

        [Required]
        public TransactionTypeEnum TransactionType { get; set; }

        [Required]
        public string SourceAccountNumber { get; set; }

        [Required]
        public string DestinationAccountNumber { get; set; }

        [Required]
        public String DestinationAccountUserName { get; set; }

        [Required]
        public Int64 Amount { get; set; }

        public Int64 OldBalance { get; set; }

        public Int64 NewBalance { get; set; }

        [Required]
        public DateTime TransactionTime { get; set; }

        [Required]
        [DisplayName("BankAccount")]
        public Int32 BankAccountId { get; set; }

        public virtual BankAccount BankAccount { get; set; }
    }
}
