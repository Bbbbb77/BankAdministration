using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankAdministration.Web.Models
{
    public class SafeModeViewModel
    {
        [DisplayName("UserName")]
        [Required]
        public string UserName { get; set; }

        [DisplayName("Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
