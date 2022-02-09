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

          var user= db.Users.ToList();

            List<RegisterUserModel> registerUserModels = new List<RegisterUserModel>();

            foreach (var item in user)
            {
               RegisterUserModel registerUserModel = new RegisterUserModel();
                registerUserModel.Id = item.Id;
                registerUserModel.Email = item.Email;
                registerUserModel.FullName = item.FullName;
                registerUserModel.PhoneNumber = item.PhoneNumber;
                registerUserModel.Gender = item.Gender;

                registerUserModels.Add(registerUserModel);

            }

            return View(registerUserModels);
        }

        public IActionResult CreateAccount()
        {
            return View();
        }
        

        [HttpPost]
        public async Task<IActionResult> CreateAccount(RegisterUserModel registerUserModel)
        {
            
            ApplicationUser user = new ApplicationUser
            {
                UserName = registerUserModel.Email,
                Email = registerUserModel.Email,
                PhoneNumber = registerUserModel.PhoneNumber,
                FullName = registerUserModel.FullName,
                Class = registerUserModel.Class,
                Avatar = "image/Userimage/usericon.png"
            };

            var result = await _userManager.CreateAsync(user, registerUserModel.Password);
            if (result.Succeeded)
            {

                return RedirectToAction("Index");
            }

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
