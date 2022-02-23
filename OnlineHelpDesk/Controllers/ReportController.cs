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
    public class ReportController : Controller
    {
        private readonly Data.ApplicationDbContext db;

        public ReportController(Data.ApplicationDbContext _db)
        {
            this.db = _db;
        }

        public IActionResult Details(int _id)
        {
            Request req = db.Request.Find(_id);
            ViewBag.facilities = db.Facility.ToList();
            return View(req);
        }

        public IActionResult Create()
        {
            ViewBag.facilityList = new SelectList(db.Facility.ToList(), "FacilityId", "FacilityName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Request req)
        {
            try
            {
                var request = Request.Form;
                if (ModelState.IsValid)
                {
                    req.Status = "Report";
                    req.RequestorId = HttpContext.Session.GetString("userId");
                    req.FacilityId = int.Parse(request["FacilityId"]);
                    req.RequestTime = DateTime.Now;
                    req.StartDate = DateTime.Parse(request["StartDate"]);
                    req.EndDate = DateTime.Parse(request["EndDate"]);
                    req.Remark = request["Remark"];
                    db.Request.Add(req);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Request");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return View();
        }

        public IActionResult Edit(int _id)
        {
            Request req = db.Request.Find(_id);
            ViewBag.facilityList = new SelectList(db.Facility.ToList(), "FacilityId", "FacilityName");
            if (req.Status == "Report")
            {
                return View(req);
            }
            else if(req.Status == "Resolved" || req.Status == "Unresolved")
            {
                return RedirectToAction("Edit1", "Report", new { id = req.RequestId });
            }
            else
            {
                return RedirectToAction("Edit", "Request", new { id = _id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Request req)
        {
            try
            {
                req = db.Request.Find(req.RequestId);
                var request = Request.Form;
                if (ModelState.IsValid)
                {
                    req.FacilityId = int.Parse(request["FacilityId"]);
                    req.StartDate = DateTime.Parse(request["StartDate"]);
                    req.EndDate = DateTime.Parse(request["EndDate"]);
                    req.Remark = request["Remark"];
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Request");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return View();
        }

        public IActionResult Edit1(int id)
        {
            Request req = db.Request.Find(id);
            ViewBag.facilityList = new SelectList(db.Facility.ToList(), "FacilityId", "FacilityName");
            return View(req);
        }

        public IActionResult Approval(int _id)
        {
            Request req = db.Request.Find(_id);
            ViewBag.facilityList = new SelectList(db.Facility.ToList().FindAll(f => f.RentalStatus == true), "FacilityId", "FacilityName");
            if (req.Status == "Report" || req.Status == "Resolved" || req.Status == "Unresolved")
            {
                ViewBag.Resolved = "Resolved";
                ViewBag.Unresolved = "Unresolved";
                return View(req);
            }
            else
            {
                return RedirectToAction("Approval", "Request", new { id = _id });
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
    }
}
