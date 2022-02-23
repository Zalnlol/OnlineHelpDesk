using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Receiver,Room Manager,Student,Admin")]
        public IActionResult Details(int _id)
        {
            Request req = db.Request.Find(_id);
            ViewBag.facilities = db.Facility.ToList();
            ViewBag.requestSamples = db.RequestSample.ToList();
            return View(req);
        }

        [Authorize(Roles = "Receiver,Room Manager,Student,Admin")]

        public IActionResult Create(int id)
        {
            ViewBag.requestSample = new SelectList(db.RequestSample.ToList(), "RequestSampleId", "Content");
            ViewBag.facility = db.Facility.ToList();
            ViewBag.facilityId = db.Facility.Find(id).FacilityId;
            return View();
        }
        [Authorize(Roles = "Receiver,Room Manager,Student,Admin")]

        [HttpPost]
        public async Task<IActionResult> Create(Request req, int id)
        {
            try
            {
                ViewBag.facilityId = db.Facility.Find(id).FacilityId;
                var request = Request.Form;
                if (ModelState.IsValid)
                {
                    req.Status = "Report";
                    req.RequestorId = HttpContext.Session.GetString("userId");
                    req.FacilityId = ViewBag.facilityId;
                    req.RequestSampleId = int.Parse(request["RequestSampleId"]);
                    req.RequestTime = DateTime.Now;
                    req.StartDate = DateTime.Parse(request["StartDate"]);
                    req.EndDate = DateTime.Parse(request["EndDate"]);
                    req.Remark = request["Remark"];
                    req.Authorize = false;
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
        [Authorize(Roles = "Receiver,Room Manager,Student,Admin")]

        public IActionResult Edit(int _id)
        {
            Request req = db.Request.Find(_id);
            ViewBag.requestSample = new SelectList(db.RequestSample.ToList(), "RequestSampleId", "Content");
            ViewBag.facility = db.Facility.ToList();
            ViewBag.facilityId = req.FacilityId;
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
        [Authorize(Roles = "Receiver,Room Manager,Student,Admin")]

        public async Task<IActionResult> Edit(Request req)
        {
            try
            {
                req = db.Request.Find(req.RequestId);
                var request = Request.Form;
                if (ModelState.IsValid)
                {
                    req.RequestSampleId = int.Parse(request["RequestSampleId"]);
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
        [Authorize(Roles = "Receiver,Room Manager,Student,Admin")]

        public IActionResult Edit1(int id)
        {
            Request req = db.Request.Find(id);
            ViewBag.requestSample = new SelectList(db.RequestSample.ToList(), "RequestSampleId", "Content");
            ViewBag.facility = db.Facility.ToList();
            ViewBag.facilityId = req.FacilityId;
            return View(req);
        }
        [Authorize(Roles = "Receiver,Room Manager,Admin")]

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
        [Authorize(Roles = "Receiver,Room Manager,Admin")]

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



        [HttpPost]
        public async Task<IActionResult> Feedback(string RequestId, string Feedback)
        {

            var ds = db.Request.SingleOrDefault(t => t.RequestId == int.Parse(RequestId));

            ds.Feedback = Feedback;
            ds.Status = "Resolved";
            db.SaveChanges();



            return RedirectToAction( "Index", "RoomManager");
        }
    }
}
