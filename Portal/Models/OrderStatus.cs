using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class OrderStatus
    {
        [Key]
        public long OrderStatusID { get; set; }
        public string Status { get; set; }
    }
}
