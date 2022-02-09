using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineHelpDesk.Data;
using OnlineHelpDesk.Models;
using OnlineHelpDesk.Areas.Admin.Models;




namespace OnlineHelpDesk.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(ApplicationDbContext db, UserManager<ApplicationUser> _userManager)
        {
            this.db = db;
            this._userManager = _userManager;
        
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }
        

        [HttpPost]
        public IActionResult CreateAccount(RegisterUserModel registerUserModel)
        {
            return View();
        }


        [HttpPost]
        public bool checkemail(string mail)
        {

            var email = db.Users.SingleOrDefault(t => t.Email.ToLower().Equals(mail.Trim().ToLower())).Email;

            if (email !=null)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
