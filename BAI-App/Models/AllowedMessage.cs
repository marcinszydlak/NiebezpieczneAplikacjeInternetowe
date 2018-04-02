using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int PermissionLevel { get; set; }
        public virtual User User { get; set; }
        public virtual Message Message { get; set; }
    }
}
