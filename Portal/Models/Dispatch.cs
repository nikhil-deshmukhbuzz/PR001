using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class Dispatch
    {
        [Key]
        public long DispatchID { get; set; }
        public DateTime? DispatchDate { get; set; }
        public string ShippingAddress { get; set; }
        public string DispatchNumber { get; set; }
        public bool IsDispatched { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
       
        public ICollection<DispatchDetail> DispatchDetails { get; set; }


        public long ClientID { get; set; }
        public ClientMaster ClientMaster { get; set; }
        

        public ProductMaster ProductMaster { get; set; }
        public long ProductID { get; set; }

        public long OrderID { get; set; }
        public Order Order { get; set; }

        public long CityID { get; set; }
        public CityMaster CityMaster { get; set; }


        public long DistrictID { get; set; }
        public DistrictMaster DistrictMaster { get; set; }

        public long StateID { get; set; }
        public StateMaster StateMaster { get; set; }
    }
}
