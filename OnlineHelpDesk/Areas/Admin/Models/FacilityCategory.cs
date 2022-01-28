using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineHelpDesk.Areas.Admin.Models
{
    [Table("FacilityCategory")]
    public class FacilityCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string FacilityCategoryId { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        [StringLength(250, MinimumLength = 2, ErrorMessage = "Name from 2 to 250 characters")]
        public string CategoryName { get; set; }
    }
}
