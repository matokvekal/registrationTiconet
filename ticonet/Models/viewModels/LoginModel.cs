using System.ComponentModel.DataAnnotations;
using Business_Logic;

namespace ticonet
{
    public class LoginModel
    {
        // [Required]
        [Required(ErrorMessage = "שדה חובה")]
        [localizedSystemDisplayName("Login.userName")]
        [DataType(DataType.Text)]
        public string userName { get; set; }
        [Required(ErrorMessage = "שדה חובה")]
        [localizedSystemDisplayName("Login.password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [localizedSystemDisplayName("Login.RemmemberMe")]
        public bool RememberMe { get; set; }
    }
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "שדה חובה")]
        [localizedSystemDisplayName("Login.userName")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [localizedSystemDisplayName("Login.entranceCode")]
        [DataType(DataType.Text)]
        public string entranceCode { get; set; }
    }

    public class NewPasswordViewModel
    {

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }
    }
}
