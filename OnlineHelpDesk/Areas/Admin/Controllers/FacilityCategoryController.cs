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
    //[Authorize(Roles="Admin")]
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
            var facilityCateList = _context.FacilityCategory.ToList();
            return View(facilityCateList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(FacilityCategory newFacilityCategory) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.FacilityCategory.Add(newFacilityCategory);
                    _context.SaveChanges();
                    return RedirectToAction("FacilityCategoryList");
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

        [HttpGet]
        public IActionResult Edit(string id) 
        {
            var model = _context.FacilityCategory.Find(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(FacilityCategory editFacilityCategory) 
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var ex = _context.FacilityCategory.Find(editFacilityCategory.FacilityCategoryId);
                    ex.CategoryName = editFacilityCategory.CategoryName;
                    _context.SaveChanges();
                    return RedirectToAction("FacilityCategoryList");
                }
                else 
                {
                    return BadRequest();
                }
            }
            catch (Exception e)
            {

                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View();
        }


        public IActionResult Delete(string id)
        {
            var model = _context.FacilityCategory.Find(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(FacilityCategory removeFacilityCategory)
        {
            var model = _context.FacilityCategory.SingleOrDefault(fc => fc.FacilityCategoryId.Equals(removeFacilityCategory.FacilityCategoryId));
            _context.FacilityCategory.Remove(model);
            _context.SaveChanges();
            return RedirectToAction("FacilityCategoryList");
        }
    }
}
