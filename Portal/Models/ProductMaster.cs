using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
   public class ProductMaster
    {
        [Key]
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string HostedUrl { get; set; }
        public string HostedSubUrl { get; set; }
        public bool IsActive { get; set; }
        public decimal Commision { get; set; }
    }
}
