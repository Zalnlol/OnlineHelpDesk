﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineHelpDesk.Areas.Admin.Controllers
{
    public class FacilityCategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}