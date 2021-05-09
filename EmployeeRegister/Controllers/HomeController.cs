using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EmployeeRegister.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace EmployeeRegister.Controllers
{
    public class HomeController : Controller
    {
        private EmployeeRegisterContext db;

        public HomeController(EmployeeRegisterContext context)
        {
            db = context;
        }

        // Login Page
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        // Registration Page
        [HttpGet]
        [Route("/Registration")]
        public IActionResult RegistrationPage()
        {
            return View("Registration");
        }

        // Login Method
        [HttpPost]
        [Route("/LoginSuccess")]
        public IActionResult Login(LoginManager LoggedManager)
        {
            if (ModelState.IsValid)
            {
                // check to see if email exists in the DB
                Manager managerInDb = db.Managers.FirstOrDefault(m => m.Email == LoggedManager.LoginEmail);
                if (managerInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Login credentials");
                    return View("Index");
                }
                // intialize passwordhasher object
                PasswordHasher<LoginManager> Hasher = new PasswordHasher<LoginManager>();
                var result = Hasher.VerifyHashedPassword(LoggedManager, managerInDb.Password, LoggedManager.LoginPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Invalid Login credentials");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("LoggedManagerId", managerInDb.ManagerId);
                return RedirectToAction("Dashboard", "Register");
            }
            return View("Index");
        }

        // Registration Method
        [HttpPost]
        [Route("/RegistrationSuccess")]
        public IActionResult Registration(Manager newManager)
        {
            if (ModelState.IsValid)
            {
                // check to see if email already exists in the DB
                if (db.Managers.Any(m => m.Email == newManager.Email))
                {
                    ModelState.AddModelError("Email", "already associated with another account");
                    return View("Registration");
                }
                if (ModelState.IsValid)
                {
                    PasswordHasher<Manager> Hasher = new PasswordHasher<Manager>();
                    newManager.Password = Hasher.HashPassword(newManager, newManager.Password);
                    db.Managers.Add(newManager);
                    db.SaveChanges();
                    HttpContext.Session.SetInt32("LoggedManagerId", newManager.ManagerId);
                    return RedirectToAction("Dashboard", "Register");
                }
            }
            return View("Registration");
        }

        // Logout method
        [HttpPost]
        [Route("/Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
