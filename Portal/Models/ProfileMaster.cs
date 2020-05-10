using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class ProfileMaster
    {
        [Key]
        public long ProfileID { get; set; }
        public string ProfileName { get; set; }
    }
}
