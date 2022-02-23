using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using OnlineHelpDesk.Areas.Admin.Models;
using OnlineHelpDesk.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineHelpDesk.Controllers
{
    public class HeadFacilityController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HeadFacilityController(ApplicationDbContext db, SignInManager<ApplicationUser> _signInManager, UserManager<ApplicationUser> _userManager, ILogger<HomeController> logger)
        {
            this.db = db;
            this._userManager = _userManager;
            this._signInManager = _signInManager;
            _logger = logger;

        }
        public IActionResult RoomManager()
        {
            if (_signInManager.IsSignedIn(User) == false)
            {
                return RedirectToAction("Login");
            }

            string iduser = _userManager.GetUserAsync(User).Result?.Id;
            var facid = 0;

            string roleid = db.UserRoles.SingleOrDefault(t => t.UserId.Equals(iduser)).RoleId;

            if (roleid == "3")
            {
                facid = 1;
            }
            else
            {
                var tmp = _userManager.GetUserAsync(User).Result?.FacilityId;
                facid = (int)tmp;
            }

            var facilityManaged = db.Facility.SingleOrDefault(t => t.FacilityId == facid);
            return View(facilityManaged);
        }

        [HttpGet]
        public IActionResult EditStatus(int id) 
        {
            if (_signInManager.IsSignedIn(User) == false)
            {
                return RedirectToAction("Login");
            }
            var facilityManaged = db.Facility.Find(id);
            return View(facilityManaged);
        }

        [HttpPost]
        public IActionResult EditStatus(Facility updateFacility, IFormFile file, string Status)
        {
          

            if (Status=="true")
            {
                updateFacility.Status = 1;

            }
            else
            {
                updateFacility.Status = 0;

            }

         
                var ex = db.Facility.SingleOrDefault(t => t.FacilityId.Equals(updateFacility.FacilityId));

                if (file !=null)
                    {
                    var filePath = Path.Combine("wwwroot/image/ImageSystem", file.FileName);
                    var stream = new FileStream(filePath, FileMode.Create);
                    file.CopyToAsync(stream);
                    ex.Image = "image/ImageSystem/" + file.FileName;
                }
                ex.FacilityName = updateFacility.FacilityName;
                ex.Description = updateFacility.Description;
                ex.Status = updateFacility.Status;
                ex.RentalStatus = updateFacility.RentalStatus;
                db.SaveChanges();
               

         
            return RedirectToAction("RoomManager");
        }
    }
}
