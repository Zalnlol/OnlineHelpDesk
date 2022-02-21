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
                    req.Status = "Unresolved";
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

        public IActionResult Edit(int id)
        {
            Request req = db.Request.Find(id);
            ViewBag.facilityList = new SelectList(db.Facility.ToList(), "FacilityId", "FacilityName");
            return View(req);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Request req)
        {
            try
            {
                req = db.Request.Find(req.RequestId);
                var request = Request.Form;
                if (req.Status == "Unresolved")
                {
                    if (ModelState.IsValid)
                    {
                        req.FacilityId = int.Parse(request["FacilityId"]);
                        req.StartDate = DateTime.Parse(request["StartDate"]);
                        req.EndDate = DateTime.Parse(request["EndDate"]);
                        req.Remark = request["Remark"];
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return RedirectToAction("Edit1", "Report", new { id = req.RequestId });
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
    }
}
