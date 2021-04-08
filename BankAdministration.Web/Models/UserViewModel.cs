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
        public string BankAccount { get; set; }

        [DisplayName("SafeMode")]
        [Required]
        public bool IsSafeMode { get; set; }
    }

    public class RegisterViewModel
    {
        [DisplayName("UserName")]
        [Required]
        public string UserName { get; set; }

        [DisplayName("FullName")]
        [Required]
        public string FullName { get; set; }

        [DisplayName("Password")]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Pincode")]
        [Required]
        public int Pincode { get; set; }

        [DisplayName("BankAccount")]
        [Required]
        public string BankAccount { get; set; }
    }

}
