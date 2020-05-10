using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class ClientMaster
    {
        [Key]
        public long ClientID { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }

        public long CityID { get; set; }
        public CityMaster CityMaster { get; set; }


        public long DistrictID { get; set; }
        public DistrictMaster DistrictMaster { get; set; }

        public long StateID { get; set; }
        public StateMaster StateMaster { get; set; }

        public long? DistributorID { get; set; }
        public Distributor Distributor { get; set; }
    }
}