using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Models
{
    public class MenuProfileLink
    {
        [Key]
        public long MenuProfileLinkID { get; set; }

        public long ProfileID { get; set; }
        public ProfileMaster ProfileMaster { get; set; }

        public long MenuID { get; set; }
        public MenuMaster MenuMaster { get; set; }

    }
}
