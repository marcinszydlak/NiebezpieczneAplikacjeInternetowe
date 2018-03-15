using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai_APP.Entity
{
    public class AllowedMessage
    {
        [Key]
        public int AllowID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        [ForeignKey("Message")]
        public int MessageID { get; set; }
        public virtual User User { get; set; }
        public virtual Message Message { get; set; }
    }
}
