using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public DateTime LastLogin { get; set; }
        public virtual ICollection<AllowedMessage> AllowedMessages { get; set; }
    }
}
