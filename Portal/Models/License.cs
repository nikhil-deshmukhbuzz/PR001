using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class License
    {
        [Key]
        public long LicenseID { get; set; }
        public int TotalLicense { get; set; }
        public DateTime? LicenseDueDate { get; set; }

        public ClientMaster ClientMaster { get; set; }
        public long ClientID { get; set; }

        public ProductMaster ProductMaster { get; set; }
        public long ProductID { get; set; }
    }
}