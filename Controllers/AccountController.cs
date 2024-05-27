using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Onlinelibrary.Models;
using Onlinelibrary.Data;
using BCrypt.Net;
using System.Security.Cryptography;
using System.Text;

namespace Onlinelibrary.Controllers;

public class AccountController : Controller
    {
        private readonly LibraryContext _context;

        public AccountController(LibraryContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = _context.Users.FirstOrDefault(u => u.Username == model.Username);
                
                if (user != null)
                {
                    var hash = ComputeHash(model.Password);
                    if (user.PasswordHash == hash)
                    {
                        HttpContext.Session.SetString("UserId", user.Id.ToString());
                        return RedirectToAction("Index", "Home");
                    }
                }
                ViewData["AlertMessage"] = "Invalid login attempt.";
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {  
                var user = new User
                {
                    Username = model.Username,
                    PasswordHash = ComputeHash(model.Password)
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Users()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
    

        private string ComputeHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }