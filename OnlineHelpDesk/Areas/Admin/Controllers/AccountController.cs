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
    [Authorize(Roles = "Admin")]
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
            if (roles !="1" || roles !="2")
            {
                var user = db.Users.SingleOrDefaultAsync(t => t.Id.Equals(Id)).Result;

                if (Facility !=null && roles=="4")
                {
                    user.FacilityId = int.Parse(Facility);

                    db.SaveChanges();
                }

               


            }




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


        public IActionResult editrole(string id)
        {

           
            var Facility = db.Facility.ToList();

            Facility FacilityUser = new Facility();

            if (db.Users.SingleOrDefault(t => t.Id.Equals(id)).FacilityId != 0)
            {
                 FacilityUser = (from user in db.Users
                                    where user.Id.Equals(id)
                                    join facilitys in db.Facility
                                    on user.FacilityId.ToString() equals facilitys.FacilityId.ToString()
                                 select facilitys).SingleOrDefault();
            }
            else
            {
                FacilityUser = null;
            }


            var RoleUser = (from userrole in db.UserRoles
                            where userrole.UserId.Equals(id)
                            join roles in db.Roles
                            on
                            userrole.RoleId equals roles.Id
                            select roles).SingleOrDefault();
            var Role = db.Roles.ToList();


           

            ViewBag.FacilityUser = FacilityUser;
            ViewBag.Facility = Facility;
            ViewBag.RoleUser = RoleUser;
            ViewBag.Roles = Role;

            ViewBag.Id = id;


            return View();
        }


        [HttpPost]
        public IActionResult editrole(string Id, string roles, string Facility)
        {
            var userrole = db.UserRoles.SingleOrDefault(t => t.UserId.Equals(Id));

            

            var newuserrole = userrole;


            db.UserRoles.Remove(userrole);
            db.SaveChanges();

            newuserrole.RoleId = roles;
            db.UserRoles.Add(newuserrole);


           

            if (roles == "1" || roles == "2")
            {
          
                var user = db.Users.SingleOrDefault(t => t.Id.Equals(Id));
                user.FacilityId = 0;
              

            }
            else
            {

                var user = db.Users.SingleOrDefault(t => t.Id.Equals(Id));
                user.FacilityId = int.Parse(Facility);

            }




            db.SaveChanges();


            return RedirectToAction("Index");


         
        }



        public IActionResult delete(string id)
        {

            var userrole = db.UserRoles.SingleOrDefault(u => u.UserId.Equals(id));

            if (userrole !=null)
            {
                db.UserRoles.Remove(userrole);
                db.SaveChanges();
            }

           

            var request = db.Request.SingleOrDefault(u => u.RequestorId.Equals(id));


            if (request != null)
            {
                db.Request.Remove(request);
                db.SaveChanges();
            }


          


            var useri = db.Users.SingleOrDefault(u => u.Id.Equals(id));

            db.Users.Remove(useri);
            db.SaveChanges();


            return RedirectToAction("Index");
        }

    }
}
