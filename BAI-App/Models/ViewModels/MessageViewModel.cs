using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bai_APP.Entity.ViewModels
{
    public class MessageViewModel
    {
        [Key]
        [Required]
        public int MessageID { get; set; }
        [Required]
        public string MessageText { get; set; }
        public override string ToString()
        {
            return MessageText;
        }
    }
}