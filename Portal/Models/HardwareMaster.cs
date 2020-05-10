using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Portal.Models
{
   public class HardwareMaster
    {
        [Key]
        public long HardwareID { get; set; }
       
        public string HardwareName { get; set; }
    }
}
