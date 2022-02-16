using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using OnlineHelpDesk.Areas.Admin.Models;

namespace OnlineHelpDesk.Data
{
    public class ApplicationUser : IdentityUser {


        public string FullName { get; set; }
        public string Class { get; set; }
        public string Avatar { get; set; }
        public bool Gender { get; set; }

        public int FacilityId { get; set; }
    }

   


    public class ApplicationDbContext :IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

        public virtual DbSet<Facility> Facility { get; set; }
        public virtual DbSet<FacilityCategory> FacilityCategory { get; set; }
        public virtual DbSet<RequestSample> RequestSample { get; set; }

        public virtual DbSet<Models.Request> Request { get; set; }
    }
}
