using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.UI.ViewModels
{
    public class LoginViewModel
    {
        public class LoginCredentials
        {
            [Required]
            [DisplayName("Email or Username")]
            [StringLength(70, ErrorMessage = "{0} length must be between {1} and {2}.", MinimumLength = 1)]
            public string EmailOrUsername { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [StringLength(100, ErrorMessage = "{0} length must be between {1} and {2}.", MinimumLength = 8)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            public string ReturnUrl { get; set; }
        }

        public LoginCredentials Input { get; set; }

        public bool AllowRememberLogin { get; set; } = true;
        public bool EnableLocalLogin { get; set; } = true;
    }
}
