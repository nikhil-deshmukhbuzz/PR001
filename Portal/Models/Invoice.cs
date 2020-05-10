using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class Invoice
    {
        [Key]
        public long InvoiceID { get; set; }
        public string InvoiceNumber { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public long ModifiedBy { get; set; }
        public bool IsDraft { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<InvoiceDetail> InvoiceDetails { get; set; }

        public long ClientID { get; set; }
        public ClientMaster ClientMaster { get; set; }

        public long ProductID { get; set; }
        public ProductMaster ProductMaster { get; set; }

        public long OrderID { get; set; }
        public Order Order { get; set; }

        public long? PaymentModeID { get; set; }
        public PaymentMode PaymentMode { get; set; }

        public long PaymentStatusID { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

    }
}
