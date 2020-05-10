using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class InvoiceHeader
    {
        [Key]
        public long InvoiceHeaderID { get; set; }
        public string Header { get; set; }
        public decimal Amount { get; set; }
        public int SequenceNo { get; set; }
        public bool IsSoftware { get; set; }
        public bool IsActive { get; set; }

        public long? ProductID { get; set; }
        public ProductMaster ProductMaster { get; set; }
    }
}
