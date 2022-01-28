using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineHelpDesk.Data;
using OnlineHelpDesk.Areas.Admin.Models;

namespace OnlineHelpDesk.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Admin")]
    public class FacilityCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacilityCategoryController(ApplicationDbContext _context) 
        {
            this._context = _context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FacilityCategoryList() 
        {
            var facilityList = _context.FacilityCategory.ToList();
            return View(facilityList);
        }

        public IActionResult CreateNewFacilityCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateNewFacilityCategory(FacilityCategory newFacilityCategory) 
        {
            if (ModelState.IsValid) 
            {
                _context.FacilityCategory.Add(newFacilityCategory);
                _context.SaveChanges();
                return RedirectToAction("FacilityCategoryList");
            }
            return View();
        }
    }
}
