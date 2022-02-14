using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using OnlineHelpDesk.Data;
using OnlineHelpDesk.Models;
using OnlineHelpDesk.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;

namespace OnlineHelpDesk.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(ApplicationDbContext db, SignInManager<ApplicationUser> _signInManager, UserManager<ApplicationUser> _userManager)
        {
            this.db = db;
            this._userManager = _userManager;
            this._signInManager = _signInManager;

        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public  string Login(
            string mail, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException($"'{nameof(password)}' cannot be null or empty.", nameof(password));
            }


            //Services.Mail mail = new Services.Mail();

            //return password;


            var result =  _signInManager.PasswordSignInAsync(mail, password, true, false);

            if (result.Result.Succeeded)
            {

                string id = _userManager.GetUserAsync(User).Result?.Id;
                //return id;

                if (db.UserRoles.SingleOrDefault(t=>t.UserId.Equals(id)).RoleId == "1")
                {
                    return  "admin";
                }

                return "true";

            }
            else
            {
                return "Email or password is incorrect";
            }



        }



    }
}
