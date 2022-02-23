using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineHelpDesk.Areas.Admin.Models
{
    [Table("Facility")]
    public class Facility
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FacilityId { get; set; }
        [Required]
        public string FacilityName { get; set; }
        public string FacilityCategoryId { get; set; } //Foreign Key
        public string Image { get; set; }

        public string Description { get; set; }
        public int Status { get; set; }

        public bool RentalStatus { get; set; }
        
        
    }
}
