using Microsoft.EntityFrameworkCore;

namespace EmployeeRegister.Models
{
    public class EmployeeRegisterContext : DbContext
    {
        public EmployeeRegisterContext(DbContextOptions options) : base(options) { }

        // for every model / entity that is going to be part of the db
        // the names of these properties will be the names of the tables in the db
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<WorkShift> WorkShifts {get;set;}

        // public DbSet<Widget> Widgets { get; set; }
        // public DbSet<Item> Items { get; set; }
    }
}
