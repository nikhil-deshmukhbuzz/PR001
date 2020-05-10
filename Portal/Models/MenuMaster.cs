using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class MenuMaster
    {
        [Key]
        public long MenuID { get; set; }
        public string MenuName { get; set; }
        public string Url { get; set; }
        public string ImageUrl { get; set; }
        public bool IsMobile { get; set; }
        public bool IsActive { get; set; }
        public int SequenceNo { get; set; }
        public long ParentMenuID { get; set; }
        
    }
}