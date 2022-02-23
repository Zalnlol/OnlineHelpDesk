using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineHelpDesk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineHelpDesk.Controllers
{
    public class ReceiverController : Controller
    {
        private readonly Data.ApplicationDbContext db;

        public ReceiverController(Data.ApplicationDbContext _db)
        {
            this.db = _db;
        }

        public IActionResult Index(String startDate, String endDate, String _button)
        {
            var model = db.Request.ToList();
            if (HttpContext.Session.GetString("Role") == "2")
            {
                model = db.Request.ToList().FindAll(r => r.RequestorId == HttpContext.Session.GetString("userId"));
            }

            ViewBag.facilities = db.Facility.ToList();
            if (String.IsNullOrEmpty(startDate) && String.IsNullOrEmpty(endDate) || _button == "Reset")
            {
                return View(model);
            }
            else if (String.IsNullOrEmpty(startDate) || String.IsNullOrEmpty(endDate))
            {
                return View(model);
            }
            else
            {
                DateTime _startDate = DateTime.Parse(startDate);
                DateTime _endDate = DateTime.Parse(endDate);
                int startDay = _startDate.Day;
                int startMonth = _startDate.Month;
                int startYear = _startDate.Year;

                int endDay = _endDate.Day;
                int endMonth = _endDate.Month;
                int endYear = _endDate.Year;


                model = model.FindAll(m => m.StartDate.Day >= startDay &&
                                      m.StartDate.Month >= startMonth &&
                                      m.StartDate.Year >= startYear
                                      &&
                                      m.EndDate.Day <= endDay &&
                                      m.EndDate.Month <= endMonth &&
                                      m.EndDate.Year <= endYear);

                ViewBag.startDate = startDate;
                ViewBag.endDate = endDate;
                return View(model);
            }
        }

        public IActionResult Details(int id)
        {
            Request req = db.Request.Find(id);
            ViewBag.facilities = db.Facility.ToList();
            if (req.Status == "Request" || req.Status == "Approved" || req.Status == "Unapproved")
            {
                return View(req);
            }
            else
            {
                return RedirectToAction("Details", "Report", new { _id = id });
            }
        }

        public IActionResult Authorize(int id)
        {
            Request req = db.Request.Find(id);
            ViewBag.facilityList = new SelectList(db.Facility.ToList(), "FacilityId", "FacilityName");
            if(req.Status == "Request" || req.Status == "Approved" || req.Status == "Unapproved")
            {
                return View(req);
            }
            else
            {
                return RedirectToAction("Authorize1", "Receiver", new { _id = id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Authorize(Request req, String authorize)
        {
            try
            {
                req = db.Request.Find(req.RequestId);
                req.Authorize = Boolean.Parse(authorize);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }

        public IActionResult Authorize1(int _id)
        {
            Request req = db.Request.Find(_id);
            ViewBag.facilityList = new SelectList(db.Facility.ToList(), "FacilityId", "FacilityName");
            if (req.Status == "Report" || req.Status == "Resolved" || req.Status == "Unresolved")
            {
                return View(req);
            }
            else
            {
                return RedirectToAction("Authorize", "Receiver", new { id = _id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Authorize1(Request req, String authorize)
        {
            try
            {
                req = db.Request.Find(req.RequestId);
                req.Authorize = Boolean.Parse(authorize);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }
    }
}
