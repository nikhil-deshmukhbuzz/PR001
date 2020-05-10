using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class Distributor
    {
        [Key]
        public long DistributorID { get; set; }
        public string DistributorName { get; set; }
        public string DistributorCode { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }       
        public DateTime ModifiedOn { get; set; }
        


        public long CityID { get; set; }
        public CityMaster CityMaster { get; set; }


        public long DistrictID { get; set; }
        public DistrictMaster DistrictMaster { get; set; }

        public long StateID { get; set; }
        public StateMaster StateMaster { get; set; }
        
    }
}
