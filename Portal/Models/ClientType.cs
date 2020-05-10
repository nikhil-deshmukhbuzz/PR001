using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class ClientType
    {
        [Key]
        public long ClientTypeID { get; set; }
        public string ClientTypeName { get; set; }

        public long ProductID { get; set; }
        public ProductMaster ProductMaster { get; set; }
    }
}
