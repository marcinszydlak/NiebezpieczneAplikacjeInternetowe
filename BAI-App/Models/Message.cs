using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bai_APP.Entity
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }
        [Required]
        [DisplayName("Treść komunikatu")]
        public string Text { get; set; }
        [DisplayName("Data ostatniej modyfikacji")]
        public DateTime Mod { get; set; }
        public ICollection<AllowedMessage> AllowedMessages { get; set; }
    }
}
