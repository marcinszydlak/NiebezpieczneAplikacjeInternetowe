﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bai_APP.Helpers
{
    public static class Error
    {
        public const string InsufficientOperationPermission = "Nie masz wystarczających uprawnień do wykonania operacji";
        public const string InsufficientResourcePermission = "Nie masz uprawnień do zasobu";
        public const string InvalidMessage = "Nie ma takiej wiadomości";
        public const string PasswordMustBeSame = "Hasła nie są takie same";
        public const string UserNameExists = "Użytkownik o podanym loginie już istnieje";
    }
}