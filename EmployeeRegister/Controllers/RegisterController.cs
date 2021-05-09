using System;
using System.Collections.Generic;
using System.Linq;
using EmployeeRegister.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRegister.Controllers
{
    public class RegisterController : Controller
    {

        private EmployeeRegisterContext db;

        public RegisterController(EmployeeRegisterContext context)
        {
            db = context;
        }

        [HttpGet]
        [Route("/Dashboard")]
        public IActionResult Dashboard()
        {
            // check to see if a user is in session
            int? LoggedManager = HttpContext.Session.GetInt32("LoggedManagerId");
            // means no user in session
            // redirect to Login/Reg page
            if (LoggedManager == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Employee> allMyEmployees = db.Employees.Where(e => e.ManagerId == (int)LoggedManager).ToList();
            return View("Dashboard", allMyEmployees);
        }

        [HttpGet]
        [Route("/AllShifts")]
        public IActionResult AllShifts()
        {
            // check to see if a user is in session
            int? LoggedManager = HttpContext.Session.GetInt32("LoggedManagerId");
            // means no user in session
            // redirect to Login/Reg page
            if (LoggedManager == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<WorkShift> allShifts = db.WorkShifts
            .Include(s => s.Employee)
            .ThenInclude(e => e.Manager)
            .Where(m => m.Employee.ManagerId == (int)HttpContext.Session.GetInt32("LoggedManagerId"))
            .OrderBy(s => s.Date)
            .ToList();
            return View("AllShifts", allShifts);
        }

        // New Employee page
        [HttpGet]
        [Route("/NewEmployee")]
        public IActionResult NewEmployeePage()
        {
            // check to see if a user is in session
            int? LoggedManager = HttpContext.Session.GetInt32("LoggedManagerId");
            // means no user in session
            // redirect to Login/Reg page
            if (LoggedManager == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("NewEmployee");
        }

        //New Employee method
        [HttpPost]
        [Route("/NewEmployeeSuccess")]
        public IActionResult NewEmployee(Employee newEmployee)
        {
            // check to see if a user is in session
            int? LoggedManager = HttpContext.Session.GetInt32("LoggedManagerId");
            // means no user in session
            // redirect to Login/Reg page
            if (LoggedManager == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                if (newEmployee.DOB >= DateTime.Now)
                {
                    ModelState.AddModelError("DOB", "must be in the past");
                    return View("NewEmployee");
                }
                if (newEmployee.HireDate > DateTime.Now)
                {
                    ModelState.AddModelError("HireDate", "cannot be in the future, must be today or the past");
                    return View("NewEmployee");
                }
                if (db.Employees.Any(
                    e => e.FirstName == newEmployee.FirstName &&
                    e.LastName == newEmployee.LastName &&
                    e.PhoneNumber == newEmployee.PhoneNumber &&
                    e.Email == newEmployee.Email &&
                    e.DOB == newEmployee.DOB
                    ))
                {
                    ModelState.AddModelError("FirstName", "Employee already exists!");
                    return View("NewEmployee");
                }
            }
            if (ModelState.IsValid)
            {
                newEmployee.ManagerId = (int)HttpContext.Session.GetInt32("LoggedManagerId");
                db.Employees.Add(newEmployee);
                db.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("NewEmployee");
        }

        // edit employee page
        [HttpGet]
        [Route("/Edit/{employeeid}")]
        public IActionResult Edit(int employeeid)
        {
            // check to see if a user is in session
            int? LoggedManager = HttpContext.Session.GetInt32("LoggedManagerId");
            // means no user in session
            // redirect to Login/Reg page
            if (LoggedManager == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Employee thisEmployee = db.Employees.FirstOrDefault(e => e.EmployeeId == employeeid);
            if (thisEmployee.ManagerId != LoggedManager)
            {
                return RedirectToAction("Dashboard");
            }
            return View("Edit", thisEmployee);
        }

        // edit employee method
        [HttpPost]
        [Route("/Edit/{employeeid}/success")]
        public IActionResult EditEmployee(Employee editedEmployee, int employeeid)
        {
            // check to see if a user is in session
            int? LoggedManager = HttpContext.Session.GetInt32("LoggedManagerId");
            // means no user in session
            // redirect to Login/Reg page
            if (LoggedManager == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                if (editedEmployee.DOB >= DateTime.Now)
                {
                    ModelState.AddModelError("DOB", "must be in the past");
                    return View("Edit");
                }
                if (editedEmployee.HireDate > DateTime.Now)
                {
                    ModelState.AddModelError("HireDate", "cannot be in the future, must be today or the past");
                    return View("Edit");
                }
            }
            Employee employeeToEdit = db.Employees.FirstOrDefault(e => e.EmployeeId == employeeid);
            employeeToEdit.FirstName = editedEmployee.FirstName;
            employeeToEdit.LastName = editedEmployee.LastName;
            employeeToEdit.Position = editedEmployee.Position;
            employeeToEdit.PhoneNumber = editedEmployee.PhoneNumber;
            employeeToEdit.Email = editedEmployee.Email;
            employeeToEdit.DOB = editedEmployee.DOB;
            employeeToEdit.Salary = editedEmployee.Salary;
            employeeToEdit.UpdatedAt = DateTime.Now;

            db.Update(employeeToEdit);
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        [Route("/Delete/{employeeid}")]
        public IActionResult DeleteEmployee(int employeeid)
        {
            // check to see if a user is in session
            int? LoggedManager = HttpContext.Session.GetInt32("LoggedManagerId");
            // means no user in session
            // redirect to Login/Reg page
            if (LoggedManager == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Employee employeeToDelete = db.Employees.FirstOrDefault(e => e.EmployeeId == employeeid);
            if (employeeToDelete.ManagerId != LoggedManager)
            {
                return RedirectToAction("Dashboard");
            }
            db.Employees.Remove(employeeToDelete);
            db.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("/NewShift")]
        public IActionResult NewShiftPage()
        {
            // check to see if a user is in session
            int? LoggedManager = HttpContext.Session.GetInt32("LoggedManagerId");
            // means no user in session
            // redirect to Login/Reg page
            if (LoggedManager == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Employee> allMyEmployees = db.Employees.Where(e => e.ManagerId == (int)HttpContext.Session.GetInt32("LoggedManagerId")).ToList();
            ViewBag.allMyEmployees = allMyEmployees;
            return View("WorkShiftForm");
        }

        [HttpPost]
        [Route("/NewShift/Success")]
        public IActionResult NewShift(WorkShift newWorkShift)
        {
            if (ModelState.IsValid)
            {
                if (newWorkShift.Date < DateTime.Now)
                {
                    List<Employee> allEmployees = db.Employees.Where(e => e.ManagerId == (int)HttpContext.Session.GetInt32("LoggedManagerId")).ToList();
                    ViewBag.allMyEmployees = allEmployees;
                    ModelState.AddModelError("Date", "must be in the future");
                    return View("WorkShiftForm");
                }
                db.WorkShifts.Add(newWorkShift);
                db.SaveChanges();
                return RedirectToAction("AllShifts");
            }
            List<Employee> allMyEmployees = db.Employees.Where(e => e.ManagerId == (int)HttpContext.Session.GetInt32("LoggedManagerId")).ToList();
            ViewBag.allMyEmployees = allMyEmployees;
            return View("WorkShiftForm");
        }

        [HttpGet]
        [Route("/EditShift/{shiftid}")]
        public IActionResult EditShiftPage(int shiftid)
        {
            // check to see if a user is in session
            int? LoggedManager = HttpContext.Session.GetInt32("LoggedManagerId");
            // means no user in session
            // redirect to Login/Reg page
            if (LoggedManager == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Employee> allEmployees = db.Employees.Where(e => e.ManagerId == (int)HttpContext.Session.GetInt32("LoggedManagerId")).ToList();
            ViewBag.allMyEmployees = allEmployees;
            WorkShift shiftToEdit = db.WorkShifts.FirstOrDefault(s => s.WorkShiftId == shiftid);
            if (shiftToEdit.Employee.ManagerId != LoggedManager)
            {
                return RedirectToAction("Dashboard");
            }
            return View("EditShift", shiftToEdit);
        }

        [HttpPost]
        [Route("/EditShift/{shiftid}/Success")]
        public IActionResult EditShift(WorkShift editedShift, int shiftid)
        {
            // check to see if a user is in session
            int? LoggedManager = HttpContext.Session.GetInt32("LoggedManagerId");
            // means no user in session
            // redirect to Login/Reg page
            if (LoggedManager == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                if (editedShift.Date < DateTime.Now)
                {
                    List<Employee> allEmployees = db.Employees.Where(e => e.ManagerId == (int)HttpContext.Session.GetInt32("LoggedManagerId")).ToList();
                    ViewBag.allMyEmployees = allEmployees;
                    ModelState.AddModelError("Date", "must be in the future");
                    return View("EditShift");
                }
                WorkShift shiftToEdit = db.WorkShifts.FirstOrDefault(s => s.WorkShiftId == shiftid);
                shiftToEdit.Date = editedShift.Date;
                shiftToEdit.StartTime = editedShift.StartTime;
                shiftToEdit.EndTime = editedShift.EndTime;
                shiftToEdit.UpdatedAt = DateTime.Now;
                db.WorkShifts.Update(shiftToEdit);
                db.SaveChanges();
                return RedirectToAction("Employee", new { employeeid = shiftToEdit.EmployeeId });
            }
            List<Employee> allMyEmployees = db.Employees.Where(e => e.ManagerId == (int)HttpContext.Session.GetInt32("LoggedManagerId")).ToList();
            ViewBag.allMyEmployees = allMyEmployees;
            return View("EditShift");
        }

        [HttpPost]
        [Route("/DeleteShift/{shiftid}")]
        public IActionResult DeleteShift(int shiftid)
        {
            WorkShift shiftToDelete = db.WorkShifts.FirstOrDefault(s => s.WorkShiftId == shiftid);
            db.Remove(shiftToDelete);
            db.SaveChanges();
            return RedirectToAction("AllShifts");
        }

        [HttpGet]
        [Route("/Employee/{employeeid}")]
        public IActionResult Employee(int employeeid)
        {
            // check to see if a user is in session
            int? LoggedManager = HttpContext.Session.GetInt32("LoggedManagerId");
            // means no user in session
            // redirect to Login/Reg page
            if (LoggedManager == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Employee thisEmployee = db.Employees
            .Include(e => e.Shifts)
            .FirstOrDefault(e => e.EmployeeId == employeeid);
            if (thisEmployee.ManagerId != LoggedManager)
            {
                return RedirectToAction("Dashboard");
            }
            return View("Employee", thisEmployee);
        }
    }
}