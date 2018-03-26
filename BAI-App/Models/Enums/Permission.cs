using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bai_APP.Models.Enums
{
    public enum Permission
    {
        Unavailable = 0,
        ReadOnly = 1,
        FullAccess = 2,
        Owner = 3
    }
}