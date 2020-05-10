using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
   public class DistrictMaster
    {
        [Key]
        public long DistrictID { get; set; }
        public string DistrictName { get; set; }

        public long StateID { get; set; }
        public StateMaster StateMaster { get; set; }
    }
}
