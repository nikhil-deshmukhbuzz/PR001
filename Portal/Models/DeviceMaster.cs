using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Portal.Models
{
    public class DeviceMaster
    {
        [Key]
        public long DeviceID { get; set; }
        public int Stock { get; set; }
        public string DeviceName { get; set; }
        public string HardwareType { get; set; }
        public decimal Amount { get; set; }
    }
}
