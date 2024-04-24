using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Laundry.CentralService.Models
{
    public class DashboardData
    {
      public string  Menu_ID {get; set;}
      public string Menu_Name { get; set; }
        public string Menu_Image { get; set; } = string.Empty;
      public bool Menu_Is_Active { get; set; }
      public int Menu_Sequence { get; set; }
        public string Menu_Activity_Name { get; set; }
    }
}