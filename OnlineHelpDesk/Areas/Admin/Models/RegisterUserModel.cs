using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineHelpDesk.Areas.Admin.Models
{
    public class RegisterUserModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "The Email field is not a valid e-mail address.")]
        //Label relevant field on the front end as 'User Name' instead of default 'Email'
        [Display(Name = "User Name")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^.*(?=.{8,})(?=.*[\d])(?=.*[\W])(?=.*[A-Z]).*$", ErrorMessage = "Min 8 chars, min 1 capital letter, min 1 digit, min 1 special char")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        [Required]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public string Id { get; set; }
        public bool Gender { get; set; }
        public string Class { get; set; }



        [Required(ErrorMessage = "Mobile no. is required")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Please enter valid phone no.")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        public string Avatar { get; set; }

    }
}
