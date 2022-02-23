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

    public class RequestController : Controller
    {
        private readonly ApplicationDbContext db;

        public RequestController(ApplicationDbContext _db)
        {
            this.db = _db;
        }

        public IActionResult Index(String startDate, String endDate, String _button)
        {
            List<Request> model = db.Request.Where(m=>m.Status == "Request" ||
                                                      m.Status == "Report").ToList();
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

        public IActionResult Index1(String startDate, String endDate, String _button)
        {
            List<Request> model = db.Request.Where(m => m.Status != "Request" &&
                                                        m.Status != "Report").ToList();
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
            var model = db.Request.Single(r => r.RequestId == id);
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

    }
}
