using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace BankAdministration.Web.Models
{
    public class DeleteBankAccountViewModel
    {
        [DisplayName("DestinationBankAccountNumber")]
        [Required]
        public string DestNumber { get; set; }
    
        [DisplayName("Amount")]
        [Required]
        public  int TransferAmount { get; set; }
    }
}
