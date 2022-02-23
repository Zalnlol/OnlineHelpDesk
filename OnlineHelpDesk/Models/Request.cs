using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace OnlineHelpDesk.Models
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestId { get; set; }

        public String RequestorId { get; set; }

        [Required(ErrorMessage = "Facility is required!")]
        public int FacilityId { get; set; }

        public int RequestSampleId { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime RequestTime { get; set; }

        public String Status { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Start date is required!")]
        public DateTime StartDate { get; set; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "End date is required!")]
        public DateTime EndDate { get; set; }

        public String Remark { get; set; }

        public bool Authorize { get; set; }

        public String Feedback { get; set; }
    }
}
