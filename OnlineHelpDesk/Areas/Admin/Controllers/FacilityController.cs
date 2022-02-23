﻿using Microsoft.AspNetCore.Authorization;
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
using Microsoft.AspNetCore.Http;
using System.IO;

namespace OnlineHelpDesk.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    //[Authorize(Roles="Admin")]
    public class FacilityController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacilityController(ApplicationDbContext _context)
        {
            this._context = _context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FacilityList(String data)
        {
            var facilityList = _context.Facility.ToList();
            List<String> facilityName = new List<String>();
            for (int i = 0; i < facilityList.Count; i++)
            {
                facilityName.Add(facilityList[i].FacilityName);
            }
            TempData["facilities"] = facilityName;
            ViewBag.data = data;
            if (String.IsNullOrEmpty(data))
            {
                return View(facilityList);
            }
            else
            {
                facilityList = _context.Facility.ToList().FindAll(f => f.FacilityName.Equals(data));
                return View(facilityList);
            }
        }

        public IActionResult Create()
        {
            ViewBag.data = new SelectList(_context.FacilityCategory.ToList(), "FacilityCategoryId", "CategoryName");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Facility newFacility)
        {
            ViewBag.data = new SelectList(_context.FacilityCategory.ToList(), "FacilityCategoryId", "CategoryName");
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Facility.Add(newFacility);
                    _context.SaveChanges();
                    return RedirectToAction("FacilityList");
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
            var model = _context.Facility.Find(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(Facility updateFacility, IFormFile file)
        {
            ViewBag.data = new SelectList(_context.FacilityCategory.ToList(), "FacilityCategoryId", "CategoryName");
            try
            {
                if (ModelState.IsValid)
                {
                    if (file.Length > 0)
                    {
                        var ex = _context.Facility.Find(updateFacility.FacilityId);
                        var filePath = Path.Combine("wwwroot/image/ImageSystem", file.FileName);
                        var stream = new FileStream(filePath, FileMode.Create);
                        file.CopyToAsync(stream);
                        ex.Image = "image/ImageSystem/" + file.FileName;
                        ex.FacilityName = updateFacility.FacilityName;
                        ex.Description = updateFacility.Description;
                        _context.SaveChanges();
                        return RedirectToAction("FacilityList");
                    }
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
            var model = _context.Facility.Find(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(Facility removeFacility)
        {
            var model = _context.Facility.SingleOrDefault(fc => fc.FacilityId.Equals(removeFacility.FacilityId));
            _context.Facility.Remove(model);
            _context.SaveChanges();
            return RedirectToAction("FacilityList");
        }
    }
}
