using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineHelpDesk.Data;
using OnlineHelpDesk.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineHelpDesk.Controllers
{
    public class RoomManagerController : Controller
    {
        private readonly Data.ApplicationDbContext db;

        public RoomManagerController(Data.ApplicationDbContext _db)
        {
            this.db = _db;
        }


        [Authorize(Roles = "Receiver,Room Manager,Student,Admin")]

        public IActionResult Index(String startDate, String endDate, String _button)
        {
            String userId = HttpContext.Session.GetString("userId");
            int facilityIdManager = db.Users.SingleOrDefault(u => u.Id == userId).FacilityId;
            var model = db.Request.ToList().FindAll(m=>m.FacilityId == facilityIdManager);
            
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

        [Authorize(Roles = "Receiver,Room Manager,Student,Admin")]

        public IActionResult Details(int id)
        {
            Request model = db.Request.Find(id);
            ApplicationUser user = db.Users.SingleOrDefault(u => u.Id == model.RequestorId);
            ViewBag.userFullName = user.FullName;
            ViewBag.facilities = db.Facility.ToList();
            ViewBag.requestSamples = null;
            if (model.Status == "Request" || model.Status == "Approved" || model.Status == "Unapproved")
            {
                return View(model);
            }
            else
            {
                ViewBag.requestSamples = db.RequestSample.ToList();
                return View(model);
            }
        }

        public IActionResult Approval(int id)
        {
            Request req = db.Request.Find(id);
            ViewBag.facilities = db.Facility.ToList();
            ViewBag.facilityId = db.Facility.Find(id).FacilityId;
            if (req.Status == "Request" || req.Status == "Approved" || req.Status == "Unapproved")
            {
                return View(req);
            }
            else
            {
                return RedirectToAction("Approval1", "RoomManager", new { _id = id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Approval(Request req, String status)
        {
            try
            {
                req = db.Request.Find(req.RequestId);
                if (HttpContext.Session.GetString("Role") == "4")
                {
                    req.Status = status;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return View();
        }

        public IActionResult Approval1(int _id)
        {
            Request req = db.Request.Find(_id);
            ViewBag.requestSample = new SelectList(db.RequestSample.ToList(), "RequestSampleId", "Content");
            ViewBag.facility = db.Facility.ToList();
            ViewBag.facilityId = db.Facility.Find(_id).FacilityId;
            if (req.Status == "Request" || req.Status == "Approved" || req.Status == "Unapproved")
            {
                return RedirectToAction("Approval", "RoomManager", new { id = _id });
            }
            else
            {
                return View(req);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Approval1(Request req, String status)
        {
            try
            {
                req = db.Request.Find(req.RequestId);
                if (HttpContext.Session.GetString("Role") == "4")
                {
                    req.Status = status;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return View();
        }
    }
}
