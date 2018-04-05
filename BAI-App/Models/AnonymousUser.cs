using System;
using System.ComponentModel.DataAnnotations;

namespace Bai_APP.Models
{
    public class AnonymousUser
    {
        [Key]
        public int Id { get; set; }
        
        public string Login { get; set; }

        public DateTime Time { get; set; }
    }
}