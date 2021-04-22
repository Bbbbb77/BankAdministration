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
        [RegularExpression("^[0-9]{6}$", ErrorMessage =
            "Pincode number must be exactly 6 numbers!")]
        public int Pincode { get; set; }
    
        [DisplayName("BankAccount")]
        [Required]
        [RegularExpression("^[0-9]{10}$", ErrorMessage =
            "BankAccount number must be exactly 10 numbers!")]
        public string BankAccount { get; set; }

        [DisplayName("SafeMode")]
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
        [RegularExpression("^[0-9]{6}$", ErrorMessage =
            "Pincode number must be exactly 6 numbers!")]
        public int Pincode { get; set; }

        [DisplayName("BankAccount")]
        [Required]
        public string BankAccount { get; set; }
    }

}
