using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bai_APP.Entity.ViewModels
{
    public class MessagePermissioniewModel
    {
        public int MessageID { get; set; }
        public int UserID { get; set; }
        public int PermissionLevel { get; set; }
    }
}