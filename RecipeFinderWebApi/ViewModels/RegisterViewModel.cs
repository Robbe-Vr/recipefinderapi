using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.UI.ViewModels
{
    public class RegisterViewModel
    {
        public class RegisterCredentials
        {
            [Required]
            [StringLength(70, ErrorMessage = "{0} length must be between {1} and {2}.", MinimumLength = 1)]
            [Display(Name = "FullName")]
            public string Name { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "DOB")]
            public DateTime DOB { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public RegisterCredentials Input { get; set; }

        public class ExternalLogin
        {
            public ExternalLogin(string name, string displayName, Type handlerType)
            {
                DisplayName = displayName;
                HandlerType = handlerType;
                Name = name;
            }

            public string DisplayName { get; }

            public Type HandlerType { get; }

            public string Name { get; }
        }

        public List<ExternalLogin> ExternalLogins { get; set; }

        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }
    }
}
