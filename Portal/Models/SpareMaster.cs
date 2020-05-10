using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class SpareMaster
    {
        [Key]
        public long SpareID { get; set; }
        public int Stock { get; set; }
        public string SpareName { get; set; }
        public decimal Amount { get; set; }
    }
}
