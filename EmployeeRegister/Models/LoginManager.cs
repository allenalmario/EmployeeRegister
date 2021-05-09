using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeRegister.Models
{

    [NotMapped]
    public class LoginManager
    {
        [Required(ErrorMessage="is required")]
        [EmailAddress]
        [Display(Name="Email")]
        public string LoginEmail {get;set;}
        [Required(ErrorMessage="is required")]
        [DataType(DataType.Password)]
        [Display(Name="Password")]
        public string LoginPassword {get;set;}
    }
}