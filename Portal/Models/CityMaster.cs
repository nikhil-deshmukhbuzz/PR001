using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class CityMaster
    {
        [Key]
        public long CityID { get; set; }
        public string CityName { get; set; }
        public string Picode { get; set; }

        public long DistrictID { get; set; }
        public DistrictMaster DistrictMaster { get; set; }

        public long? StateID { get; set; }
        public StateMaster StateMaster { get; set; }
    }
}
