using System;
using System.ComponentModel.DataAnnotations;

namespace EmployeeRegister.Models
{
    public class WorkShift
    {
        public int WorkShiftId {get;set;}
        [Required(ErrorMessage="is required")]
        [DataType(DataType.Date)]
        public DateTime Date {get;set;}
        [Required(ErrorMessage="is required")]
        [DataType(DataType.Time)]
        public DateTime StartTime {get;set;}
        [Required(ErrorMessage="is required")]
        [DataType(DataType.Time)]
        public DateTime EndTime {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        [Display(Name="Employee")]
        public int EmployeeId {get;set;}
        public Employee Employee {get;set;}
    }
}