using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using OnlineHelpDesk.Models;
using OnlineHelpDesk.Areas.Admin.Models;

namespace OnlineHelpDesk.Controllers
{
    public class RequestController : Controller
    {
        private readonly Data.ApplicationDbContext db;

        public RequestController(Data.ApplicationDbContext _db)
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
            if(req.Status == "Request" || req.Status == "Approved" || req.Status == "Unapproved")
            {
                return View(req);
            }
            else
            {
                return RedirectToAction("Details", "Report", new { _id = id });
            }
        }

        public IActionResult Create(int id)
        {
            ViewBag.requestSample = new SelectList(db.RequestSample.ToList(), "RequestSampleId", "Content");
            ViewBag.facility = db.Facility.ToList();
            ViewBag.facilityId = db.Facility.Find(id).FacilityId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Request req, int id)
        {
            try
            {
                List<Request> requestList = db.Request.ToList();
                ViewBag.facilityId = db.Facility.Find(id).FacilityId;
                var request = Request.Form;
                List<RequestSample> requestSample = db.RequestSample.ToList();
                if (ModelState.IsValid)
                {
                    req.Status = "Request";
                    req.RequestorId = HttpContext.Session.GetString("userId");
                    req.FacilityId = ViewBag.facilityId;
                    foreach (var item in requestSample)
                    {
                        req.RequestSampleId = item.RequestSampleId;
                    }
                    req.RequestTime = DateTime.Now;
                    foreach (var item in requestList)
                    {
                        if(item.FacilityId == id)
                        {
                            if (DateTime.Parse(request["StartDate"]) >= item.StartDate && DateTime.Parse(request["EndDate"]) <= item.EndDate)
                            {
                                ViewBag.requestSample = new SelectList(db.RequestSample.ToList(), "RequestSampleId", "Content");
                                ViewBag.facility = db.Facility.ToList();
                                ViewBag.facilityId = db.Facility.Find(id).FacilityId;
                                ViewBag.msg = "This time is taken. Try another!";
                                return View();
                            }
                        }
                    }
                    req.StartDate = DateTime.Parse(request["StartDate"]);
                    req.EndDate = DateTime.Parse(request["EndDate"]);
                    req.Remark = request["Remark"];
                    req.Authorize = false;
                    db.Request.Add(req);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            Request req = db.Request.Find(id);
            ViewBag.facility = db.Facility.ToList();
            ViewBag.facilityId = req.FacilityId;
            if (req.Status == "Request")
            {
                return View(req);
            }
            else if (req.Status == "Approved" || req.Status == "Unapproved")
            {
                return RedirectToAction("Edit1", "Request", new { id = id });
            }
            else
            {
                return RedirectToAction("Edit", "Report", new { _id = id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Request req)
        {
            var request = Request.Form;
            req = db.Request.Find(req.RequestId);
            try
            {    
                req.StartDate = DateTime.Parse(request["StartDate"]);
                req.EndDate = DateTime.Parse(request["EndDate"]);
                req.Remark = request["Remark"];
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
        }

        public IActionResult Edit1(int id)
        {
            Request req = db.Request.Find(id);
            ViewBag.facility = db.Facility.ToList();
            ViewBag.facilityId = db.Facility.Find(id).FacilityId;
            return View(req);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Request req = db.Request.SingleOrDefault(r => r.RequestId == id);
                db.Request.Remove(req);
                await db.SaveChangesAsync();
                ViewBag.msg = "Delete completed successfully!";
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.Message);
            }
            return RedirectToAction("Index");
        }

       

       
    }
}
