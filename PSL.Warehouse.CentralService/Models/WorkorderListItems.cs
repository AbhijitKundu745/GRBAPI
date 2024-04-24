using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class WorkorderListItems
    {
        public string PalletNumber { get; set; }
        public string PickupLocation { get; set; }
        public string BinLocation { get; set; }
        public string ListItemStatus { get; set; }
       
    }
}