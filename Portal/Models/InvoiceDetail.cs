using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class InvoiceDetail
    {
        [Key]
        public long InvoiceDetailID { get; set; }
        public decimal Amount { get; set; }

        public long InvoiceID { get; set; }
        public Invoice Invoice { get; set; }

        public long? InventoryID { get; set; }
        public InventoryMaster InventoryMaster { get; set; }

        public long? InvoiceHeaderID { get; set; }
        public InvoiceHeader InvoiceHeader { get; set; }
        
    }
}
