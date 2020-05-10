using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class DistributorBill
    {
        [Key]
        public long DistributorBillID { get; set; }
        public string BillNumber { get; set; }
        public decimal ProductAmount { get; set; }
        public decimal PaybleAmount { get; set; }
        public decimal Commision { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }

        public long DistributorID { get; set; }
        public Distributor Distributor { get; set; }

        public long OrderID { get; set; }
        public Order Order { get; set; }

        public long ClientID { get; set; }
        public ClientMaster ClientMaster { get; set; }

        public long ProductID { get; set; }
        public ProductMaster ProductMaster { get; set; }

        public long InvoiceID { get; set; }
        public Invoice Invoice { get; set; }

        public long? PaymentModeID { get; set; }
        public PaymentMode PaymentMode { get; set; }

        public long PaymentStatusID { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
