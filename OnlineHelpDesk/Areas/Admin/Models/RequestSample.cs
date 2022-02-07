using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineHelpDesk.Areas.Admin.Models
{
    [Table("RequestSample")]
    public class RequestSample
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestSampleId { get; set; }
        [Required]
        public string FacilityCategoryId { get; set; } //Foreign Key
        [Required]
        public string Content { get; set; }

    }
}
