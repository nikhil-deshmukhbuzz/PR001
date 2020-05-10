using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Portal.Models
{
   public class InventoryMaster
    {
        [Key]
        public long InventoryID { get; set; }
        public string InventoryName { get; set; }
        public string InventoryType { get; set; }

        public string ReferenceNumber { get; set; }
        public DateTime? WarrantyDate { get; set; }

        public long? DeviceID { get; set; }
        public DeviceMaster DeviceMaster { get; set; }

        public long? SpareID { get; set; }
        public SpareMaster SpareMaster { get; set; }
    }
}
