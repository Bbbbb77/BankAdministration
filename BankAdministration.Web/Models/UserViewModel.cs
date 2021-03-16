using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BankAdministration.Web.Models
{
    public class LoginViewModel
    {
        [DisplayName("UserName")]
        [Required]
        public string UserName { get; set; }

        [DisplayName("Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    
        [DisplayName("Pincode")]
        [Required]
        public int Pincode { get; set; }
    
        [DisplayName("BankAccount")]
        [Required]
        public int BankAccount { get; set; }
    }

    public class RegisterViewModel
    {
        [DisplayName("FirstName")]
        [Required]
        public string FirstName { get; set; }
        
        [DisplayName("LastName")]
        [Required]
        public string LastName { get; set; }

        [DisplayName("UserName")]
        [Required]
        public string UserName { get; set; }

        [DisplayName("Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Pincode")]
        [Required]
        public int Pincode { get; set; }

        [DisplayName("BankAccount")]
        [Required]
        public int BankAccount { get; set; }
    }

}
