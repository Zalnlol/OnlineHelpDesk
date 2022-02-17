using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OnlineHelpDesk.Models;
using OnlineHelpDesk.Data;
using OnlineHelpDesk.Areas.Admin.Models;

namespace OnlineHelpDesk.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RequestController : Controller
    {
        private readonly ApplicationDbContext db;

        public RequestController(ApplicationDbContext _db)
        {
            this.db = _db;
        }

        public IActionResult Index()
        {
            List<String> facilityName = new List<String>();
            List<Facility> facilities = db.Facility.ToList();
            for (int i = 0; i < facilities.Count; i++)
            {
                facilityName.Add(facilities[i].FacilityName);
            }
            TempData["facilities"] = facilityName;
            List<Request> model = db.Request.Where(m=>m.Status == "Waiting for approval").ToList();
            return View(model);
        }

        public IActionResult Index1()
        {
            List<Request> model = db.Request.Where(m => m.Status != "Waiting for approval").ToList();
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var model = db.Request.Single(r => r.RequestId == id);
            ApplicationUser user = db.Users.SingleOrDefault(u => u.Id == model.RequestorId);
            ViewBag.userFullName = user.FullName;
            return View(model);
        }
    }
}
