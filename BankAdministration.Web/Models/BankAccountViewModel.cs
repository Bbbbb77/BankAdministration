using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankAdministration.Web.Models
{
    public class CreateBankAccountViewModel
    {
        [DisplayName("Bank account number")]
        [Required]
        public String Number { get; set; }

        [DisplayName("Balance")]
        public Int64 Balance { get; set; }

        [DisplayName("IsLocked")]
        public bool IsLocked { get; set; }
    }

    public class CreateTransactionViewModel
    {
        [DisplayName("Transaction type")]
        [Required]
        public TransactionTypeEnum TransactionType { get; set; }

        [DisplayName("Source account number")]
        [Required]
        public string SourceAccountNumber { get; set; }

        [DisplayName("Destination account number")]
        [Required]
        [RegularExpression("^[0-9]{10}$", ErrorMessage =
            "Destination bank account number must be exactly 10 numbers!")]
        public string DestinationAccountNumber { get; set; }

        [DisplayName("Destination account username")]
        [Required]
        public string DestinationAccountUserName { get; set; }

        [DisplayName("Amount")]
        [Required]
        public Int64 Amount { get; set; }
    }
}
