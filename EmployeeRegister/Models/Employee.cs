using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EmployeeRegister.Models
{
    public class Employee
    {
        public int EmployeeId {get;set;}
        [Required(ErrorMessage="is required")]
        [Display(Name="First Name")]
        public string FirstName {get;set;}
        [Required(ErrorMessage="is required")]
        [Display(Name="Last Name")]
        public string LastName {get;set;}
        [Required(ErrorMessage="is required")]
        public string Position {get;set;}
        [Required(ErrorMessage="is required")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name="Phone Number")]
        public int PhoneNumber {get;set;}
        [Required(ErrorMessage="is required")]
        [EmailAddress]
        public string Email {get;set;}
        [Required(ErrorMessage="is required")]
        [DataType(DataType.Date)]
        [Display(Name="Date of birth")]
        public DateTime DOB {get;set;}
        [Required(ErrorMessage="is required")]
        [DataType(DataType.Date)]
        [Display(Name="Hire Date")]
        public DateTime HireDate {get;set;}
        [Required(ErrorMessage="is required")]
        [Range(0, 1000000, ErrorMessage="must be between 0 and 1000000")]
        public int Salary {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public int ManagerId {get;set;}
        public Manager Manager {get;set;}
        public List<WorkShift> Shifts {get;set;}
    }
}