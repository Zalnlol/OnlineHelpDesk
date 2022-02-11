﻿using Microsoft.AspNetCore.Authorization;
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




            var ds = (from users in db.Users
                      join userole in db.UserRoles
                      on users.Id equals userole.UserId
                      join roles in db.Roles
                      on userole.RoleId equals roles.Id
                      select new
                      {
                          users.Id,
                          users.Email,
                          users.FullName,
                          users.PhoneNumber,
                          users.Gender,
                          roles.Name
                      }).ToList();

            ViewBag.data = ds;


            //return BadRequest(ViewBag.data);

            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }

        public IActionResult SetRole(string id)
        {


            var ds1 = db.Roles.ToList();
            var ds2 = db.Facility.ToList();


            ViewBag.Roles = ds1;
            ViewBag.Facility = ds2;
            ViewBag.Id = id;



            return View() ;
        }

        [HttpPost]
        public IActionResult SetRole(string Id, string roles,  string Facility)
        {



            IdentityUserRole<string> roleModel = new IdentityUserRole<string>();
            roleModel.UserId = Id;
            roleModel.RoleId = roles;
            

            db.UserRoles.Add(roleModel);
            db.SaveChanges();

            var user = db.Users.SingleOrDefaultAsync(t => t.Id.Equals(Id)).Result;
            user.FacilityId = Facility;

            db.SaveChanges();


            return RedirectToAction("Index");
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
                Gender = registerUserModel.Gender,
                Avatar = "image/Userimage/usericon.png"
            };

            var result = await _userManager.CreateAsync(user, registerUserModel.Password);
            if (result.Succeeded)
            {
               

                return RedirectToAction("SetRole", "Account", new { id = user.Id });
            }

            return View();
        }


        [HttpPost]
        public bool checkemail(string mail)
        {
     
            mail = mail.Trim().ToLower();
            var email = db.Users.SingleOrDefault(t => t.Email.ToLower().Equals(mail));

            //var email = db.Users.ToList();

            if (email != null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }



        [HttpPost]
        public bool ResetPassword(string id)
        {
           
  
            var ds = db.Users.SingleOrDefault(t => t.Id.Equals(id));

            ds.PasswordHash = "AQAAAAEAACcQAAAAEE+NWvLtT1+dAghVjP75K8wk63EFsPIHjmKZfnlj7Xwyzsp0bVEcBxrzSnLdF3EJmQ==";
            db.SaveChanges();

            return true;
         
        }


    }
}
