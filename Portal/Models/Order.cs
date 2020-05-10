using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
   public class Order
    {
        [Key]
        public long OrderID { get; set; }
        public string OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public int HardwareQauntity { get; set; }
        public string Comment { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? ModifiedBy { get; set; }

        public ClientMaster ClientMaster { get; set; }
        public long ClientID { get; set; }

        public ProductMaster ProductMaster { get; set; }
        public long ProductID { get; set; }

        public ClientType ClientType { get; set; }
        public long ClientTypeID { get; set; }

        public OrderStatus OrderStatus { get; set; }
        public long OrderStatusID { get; set; }

    }
}
