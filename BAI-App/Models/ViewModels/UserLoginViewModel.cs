using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BAI_App.Models.ViewModels
{
    public class UserLoginViewModel
    {
        [DisplayName("Login")]
        public string Login { get; set; }
        [DisplayName("Hasło")]
        public string Password { get; set; }
    }
}