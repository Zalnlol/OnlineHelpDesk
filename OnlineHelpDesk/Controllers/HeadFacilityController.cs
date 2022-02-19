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

            var id = _userManager.GetUserAsync(User).Result?.FacilityId;
            var facilityManaged = db.Facility.Where(t => t.FacilityId == id);
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
        public IActionResult EditStatus(Facility updateFacility, IFormFile file)
        {
            ViewBag.data = new SelectList(db.FacilityCategory.ToList(), "FacilityCategoryId", "CategoryName");
            try
            {
                if (ModelState.IsValid)
                {
                    if (file.Length > 0)
                    {
                        var ex = db.Facility.Find(updateFacility.FacilityId);
                        var filePath = Path.Combine("wwwroot/image/ImageSystem", file.FileName);
                        var stream = new FileStream(filePath, FileMode.Create);
                        file.CopyToAsync(stream);
                        ex.Image = "image/ImageSystem/" + file.FileName;
                        ex.FacilityName = updateFacility.FacilityName;
                        ex.Description = updateFacility.Description;
                        ex.RentalStatus = updateFacility.RentalStatus;
                        db.SaveChanges();
                        return RedirectToAction("RoomManager");
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View();
        }
    }
}
