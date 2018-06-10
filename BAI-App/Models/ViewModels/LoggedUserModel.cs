using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bai_APP.Entity.ViewModels
{
    public class LoggedUserViewModel
    {
        [Key]
        public int UserID { get; set; }

        public string Login { get; set; }

        public List<MessageViewModel> Messages { get; set; }

        public override string ToString()
        {
            return Login;
        }

        public DateTime AccountLockedTo { get; set; }
    }
}