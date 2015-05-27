using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FisionSSOExample.Models
{
    public class FisionSSOModel
    {
        [Required]
        [DisplayName("User Name")]
        public string FisionUserName { get; set; }

        [Required]
        [DisplayName("User Password")]
        public string FisionUserPassword { get; set; }

        [Required]
        [DisplayName("User Security Token")]
        public string UserToken { get; set; }

        [Required]
        [DisplayName("Admin Security Token")]
        public string AdminToken { get; set; }

        [DisplayName("Landing Page")]
        public FisionLandingPages FisionLandingPage { get; set; }

        public string ErrorMessage { get; set; }
    }

    public enum FisionLandingPages : int
    {
        [Display(Name = "Homepage")]
        Homepage = 0,

        [Display(Name = "Asset Library")]
        Asset_Library = 1,

        [Display(Name = "E-Mail Library")]
        Email_Library = 2,

        [Display(Name = "Print Library")]
        Print_Library = 3
    }
}