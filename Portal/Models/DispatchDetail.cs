using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
   public class DispatchDetail
    {
        [Key]
        public long DispatchDetailID { get; set; }
        //public long DispatchID { get; set; }
        

        public long DispatchID { get; set; }
        public Dispatch Dispatch { get; set; }

        public long InventoryID { get; set; }
        public InventoryMaster InventoryMaster { get; set; }
    }
}
