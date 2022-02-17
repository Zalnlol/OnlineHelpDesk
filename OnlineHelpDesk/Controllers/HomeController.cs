﻿using Microsoft.AspNetCore.Mvc;
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
using Microsoft.AspNetCore.Http;
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

        public HomeController(ApplicationDbContext db, SignInManager<ApplicationUser> _signInManager, UserManager<ApplicationUser> _userManager, ILogger<HomeController> logger)
        {
            this.db = db;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            _logger = logger;

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
        public async Task<string> Login( string mail, string password)
        {
       


            //Services.Mail mail = new Services.Mail();

            //return password;


            var result = await _signInManager.PasswordSignInAsync(mail, password, true, false);



            if (result.Succeeded==true)
            {

                string id = db.Users.SingleOrDefault(t => t.Email.Equals(mail)).Id;
                //return id;
                var role =  db.UserRoles.SingleOrDefault(t => t.UserId.Equals(id)).RoleId;
                HttpContext.Session.SetString("Role", role);
                HttpContext.Session.SetString("userId", id);

                if (role == "1")
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

            public IActionResult Roomlist()
        {

            var ds = db.Facility.Where(t => t.Status != 0).ToList();
            var category = db.FacilityCategory.ToList();

            ViewBag.ds = ds;
            ViewBag.category = category;

            return View();


        }

        public IActionResult RoomProfile(string id)
        {

            var ds = db.Facility.SingleOrDefault(t => t.FacilityId == int.Parse(id));

            var user = db.Users.Where(t => t.FacilityId == int.Parse(id)).ToList();
            var userre = (from userrole in db.UserRoles
                          where userrole.RoleId == "3"
                          join userr in db.Users
                          on userrole.UserId equals userr.Id
                          select userr).ToList();

           
            ViewBag.userre = userre;
            ViewBag.user = user;

            return View(ds);

        }

        public async Task<IActionResult> Logout(string returnUrl = null)
        {

            await _signInManager.SignOutAsync();

            if (returnUrl != null)
            {

                return LocalRedirect(returnUrl);
            }
            else
            {
                HttpContext.Session.Remove("Role");
                HttpContext.Session.Remove("userId");

                return RedirectToAction("Index", "Home");
            }
        }


        public IActionResult UserProfile(string ? id)
        {
            string iduser = "";

            if (id!=null)
            {
                iduser = id;
            }else
                if (_userManager.GetUserAsync(User).Result?.Id != null)
            {

                iduser = _userManager.GetUserAsync(User).Result?.Id;
            }else
            {
                return BadRequest();
            }


            var user = db.Users.SingleOrDefault(t => t.Id.Equals(iduser));


            ViewBag.user = user;

            ViewBag.Role = (from userrole in db.UserRoles
                            where userrole.UserId.Equals(iduser)
                            join role in db.Roles
                            on userrole.RoleId equals role.Id
                            select role.Name).SingleOrDefault();

            return View();


        }

    }
}
