using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portal.Models
{
   public class StateMaster
    {
        [Key]
        public long StateID { get; set; }
        public string StateName { get; set; }
    }
}
