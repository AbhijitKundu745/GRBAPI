using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class Workorderlist
    {
       public string WorkorderNumber { get; set; }
       public string WorkorderType { get; set; }
       public string WorkorderStatus { get; set; }
       public string LocationName { get; set; }
    }
}