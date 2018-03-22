using Bai_APP.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bai_APP.Entity.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        public string UserLogin { get; set; }
        [Compare("Password2",ErrorMessage = Error.PasswordMustBeSame)]
        public string Password1 { get; set; }
        [Compare("Password1",ErrorMessage = Error.PasswordMustBeSame)]
        public string Password2 { get; set; }
    }
}