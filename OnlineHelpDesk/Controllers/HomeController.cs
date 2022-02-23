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
using System.IO;


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
        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Register(RegisterUserModel registerUserModel)
        {

            //return BadRequest(registerUserModel);

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

                IdentityUserRole<string> roleModel = new IdentityUserRole<string>();

                roleModel.UserId = user.Id;
                roleModel.RoleId = "2";
                db.UserRoles.Add(roleModel);
                db.SaveChanges();

                var result1 = await _signInManager.PasswordSignInAsync(user.Email, registerUserModel.Password, true, false);

                if (result1.Succeeded)
                {
                    var role = db.UserRoles.SingleOrDefault(t => t.UserId.Equals(user.Id)).RoleId;
                    HttpContext.Session.SetString("Role", "2");
                    HttpContext.Session.SetString("userId", user.Id);
                    return RedirectToAction("Index");
                }


                

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
            if (_signInManager.IsSignedIn(User)==false)
            {
                return RedirectToAction("Login");
            }
      
            var userFacility = _userManager.GetUserAsync(User).Result?.FacilityId;
            var id = _userManager.GetUserAsync(User).Result?.Id;
            var userrole = db.UserRoles.SingleOrDefault(t => t.UserId.Equals(id)).RoleId;
            List<Facility>   ds = new List<Facility>();
            if (userrole =="3")
            {
                ds = db.Facility.Where(t => t.Status != 0  && t.FacilityId.ToString().Equals("1")).ToList();
            }
            else
            {
                 ds = db.Facility.Where(t => t.Status != 0 && t.FacilityId.ToString() != userFacility.ToString()).ToList();
            }


            
            var category = db.FacilityCategory.ToList();

            ViewBag.ds = ds;
            ViewBag.category = category;

            return View();


        }

        public IActionResult RoomProfile(string id)
        {

            var ds = db.Facility.SingleOrDefault(t => t.FacilityId == int.Parse(id));

            var user = db.Users.Where(t => t.FacilityId == int.Parse(id)).ToList();

          List<ApplicationUser>   userre = new List<ApplicationUser>();

            if (id=="1")
            {
                 userre = (from userrole in db.UserRoles
                              where userrole.RoleId == "3"
                              join userr in db.Users
                              on userrole.UserId equals userr.Id
                              select userr).ToList();
            
              ViewBag.userre = userre;
            }
            else
            {
                ViewBag.userre = null;
            }
          
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



        public IActionResult EditProfile()
        {
            var userid = _userManager.GetUserAsync(User).Result?.Id;
            var ds = db.Users.SingleOrDefault(t => t.Id.Equals(userid));

            RegisterUserModel registerUserModel = new RegisterUserModel();

            registerUserModel.Id = ds.Id;
            registerUserModel.FullName = ds.FullName;
            registerUserModel.Email = ds.Email;
            registerUserModel.Avatar = ds.Avatar;
            registerUserModel.Gender = ds.Gender;
            registerUserModel.PhoneNumber = ds.PhoneNumber;

            //return BadRequest(registerUserModel);

            ViewBag.ds = registerUserModel;

            return View(registerUserModel);

        }

        [HttpPost]
        public IActionResult EditProfile(RegisterUserModel registerUserModel, IFormFile File)
        {

            var id = _userManager.GetUserAsync(User).Result?.Id;
            var userchange = db.Users.SingleOrDefault(t => t.Id.Equals(id));

            if (File != null)
            {
                var filePath = Path.Combine("wwwroot/image/Userimage/", File.FileName);
                var stream = new FileStream(filePath, FileMode.Create);
                File.CopyToAsync(stream);
                registerUserModel.Avatar = "image/Userimage/" + File.FileName;
            }
            else
            {
                registerUserModel.Avatar = userchange.Avatar;
            }

            userchange.FullName = registerUserModel.FullName;
            userchange.Gender = registerUserModel.Gender;
            userchange.PhoneNumber = registerUserModel.PhoneNumber;
            userchange.Avatar = registerUserModel.Avatar;

            db.SaveChanges();




            return RedirectToAction("UserProfile");

        }

    }
}
