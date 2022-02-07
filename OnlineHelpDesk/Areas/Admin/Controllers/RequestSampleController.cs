using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineHelpDesk.Areas.Admin.Controllers
{
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

    }
}
