using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Bai_APP.Entity.ViewModels
{
    public class MessagePermissionViewModel
    {
        [Required]
        public int MessageID { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public string UserLogin { get; set; }
        [Display(ResourceType = typeof(Models.Enums.Permission))]
        [Range(typeof(int),"0","2")]
        public int PermissionLevel { get; set; }
    }
}