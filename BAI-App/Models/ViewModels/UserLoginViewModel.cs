﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Bai_APP.Entity.ViewModels
{
    public class UserLoginViewModel
    {
        [DisplayName("Login")]
        public string Login { get; set; }
        [DisplayName("Hasło")]
        public string Password { get; set; }
        public override string ToString()
        {
            return Login;
        }
    }
}