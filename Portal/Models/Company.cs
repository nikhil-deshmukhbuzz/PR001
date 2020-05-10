using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class Company
    {
        [Key]
        public long CompanyID { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string RegisteredAddress { get; set; }
        public string WarehouseAddress { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }
}
