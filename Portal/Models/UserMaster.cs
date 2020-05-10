using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class UserMaster
    {
        [Key]
        public long UserID { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MobileNo { get; set; }
        public string OTP { get; set; }
        public bool IsActive { get; set; }
        public bool IsMobile { get; set; }

        public long ProfileID { get; set; }
        public ProfileMaster ProfileMaster { get; set; }

        public long? ClientID { get; set; }
        public ClientMaster ClientMaster { get; set; }

        public long? DistributorID { get; set; }
        public Distributor Distributor { get; set; }
    }
}