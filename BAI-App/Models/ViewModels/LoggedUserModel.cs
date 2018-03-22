using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bai_APP.Entity.ViewModels
{
    public class LoggedUserModel
    {
        [Key]
        public int UserID { get; set; }
        public string Login { get; set; }
        public List<MessageViewModel> Messages { get; set; }
        public override string ToString()
        {
            return Login;
        }
    }
}