using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineHelpDesk.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineHelpDesk.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles="Admin")]
    public class RequestSampleController : Controller
    {
        private readonly Data.ApplicationDbContext _context;
        public RequestSampleController(Data.ApplicationDbContext _context) 
        {
            this._context = _context;
        
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RequestSampleList()
        {
            var requestList = _context.RequestSample.ToList();
            return View(requestList);
        }

        public IActionResult Create()
        {
            ViewBag.data = new SelectList(_context.FacilityCategory.ToList(), "FacilityCategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        public IActionResult Create(RequestSample requestSample) 
        {
            ViewBag.data = new SelectList(_context.FacilityCategory.ToList(), "FacilityCategoryId", "CategoryName");
            try
            {
                if (ModelState.IsValid)
                {
                    _context.RequestSample.Add(requestSample);
                    _context.SaveChanges();
                    return RedirectToAction("RequestSampleList");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Fail");
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
