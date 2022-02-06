using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using OnlineHelpDesk.Areas.Admin.Models;

namespace OnlineHelpDesk.Data
{
    public class ApplicationUser : IdentityUser { }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

        public virtual DbSet<Facility> Facility { get; set; }
        public virtual DbSet<FacilityCategory> FacilityCategory { get; set; }
        public virtual DbSet<RequestSample> RequestSample { get; set; }
    }
}
