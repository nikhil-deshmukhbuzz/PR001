using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class PaymentMode
    {
        [Key]
        public long PaymentModeID { get; set; }
        public string Mode { get; set; }
    }
}
