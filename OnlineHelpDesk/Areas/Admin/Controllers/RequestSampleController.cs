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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnlineHelpDesk.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
            ViewBag.data = (from fc in requestList
                                            join abc in _context.FacilityCategory
                                            on fc.FacilityCategoryId
                                            equals abc.FacilityCategoryId
                                            select new RequestSampleMix() {
                                                RequestSampleId =    fc.RequestSampleId ,
                                                FacilityCategoryId=     fc.FacilityCategoryId,
                                                Content=  fc.Content,
                                                CategoryName=   abc.CategoryName }).ToList();
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
        [HttpGet]
        public IActionResult Edit(int id) 
        {
            ViewBag.data = new SelectList(_context.FacilityCategory.ToList(), "FacilityCategoryId", "CategoryName");
            var model = _context.RequestSample.Find(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(RequestSample updateRequestSample) 
        {
            ViewBag.data = new SelectList(_context.FacilityCategory.ToList(), "FacilityCategoryId");
            try
            {
                if (ModelState.IsValid)
                {
                    var ex = _context.RequestSample.Find(updateRequestSample.RequestSampleId);
                    ex.FacilityCategoryId = updateRequestSample.FacilityCategoryId;
                    ex.Content = updateRequestSample.Content;
                    _context.SaveChanges();
                    return RedirectToAction("RequestSampleList");
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


        public IActionResult Delete(int id)
        {
            var model = _context.RequestSample.Find(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(RequestSample removeRequestSample)
        {
            var model = _context.RequestSample.SingleOrDefault(rs => rs.RequestSampleId.Equals(removeRequestSample.RequestSampleId));
            _context.RequestSample.Remove(model);
            _context.SaveChanges();
            return RedirectToAction("RequestSampleList");
        }

    }
}
