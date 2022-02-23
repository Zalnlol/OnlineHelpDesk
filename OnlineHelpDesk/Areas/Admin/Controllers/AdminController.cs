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



    public class AdminController : Controller
    {

        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext db, UserManager<ApplicationUser> _userManager)
        {
            this.db = db;
            this._userManager = _userManager;

        }

        public IActionResult Index()
        {
            ViewBag.account = db.Users.ToList().Count();
            ViewBag.Facility = db.Facility.ToList().Count();
            ViewBag.Request = db.Request.ToList().Count();
            ViewBag.rs = db.RequestSample.ToList().Count();



            return View();
        }
    }
}
