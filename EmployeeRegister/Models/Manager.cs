using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeRegister.Models
{
    public class Manager
    {
        public int ManagerId {get;set;}
        [Required(ErrorMessage="is required")]
        [Display(Name="First Name")]
        public string FirstName {get;set;}
        [Required(ErrorMessage="is required")]
        [Display(Name="Last Name")]
        public string LastName {get;set;}
        [Required(ErrorMessage="is required")]
        [EmailAddress]
        public string Email {get;set;}
        [Required(ErrorMessage="is required")]
        [MinLength(8, ErrorMessage="must be at least 8 characters")]
        [DataType(DataType.Password)]
        public string Password {get;set;}
        [NotMapped]
        [Required(ErrorMessage="is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="Passwords do not match")]
        [Display(Name="Confirm Password")]
        public string ConfirmPassword {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public List<Employee> Employees {get;set;}
    }
}