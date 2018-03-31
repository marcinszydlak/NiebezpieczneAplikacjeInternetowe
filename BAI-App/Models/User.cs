using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bai_APP.Entity
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [DisplayName("Login")]
        public string UserLogin { get; set; }

        [DisplayName("Hasło")]
        public string PasswordHash { get; set; }
        public string Salt { get; set; }

        [DisplayName("Data ostatniego logowania")]
        public DateTime LastLogin { get; set; } = DateTime.Now;

        public virtual ICollection<AllowedMessage> AllowedMessages { get; set; }
    }
}
