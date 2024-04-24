using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class ActivityResponseData
    {
        public bool status { get; set; }
        public bool isUnique { get; set; }
        public bool isItemUnique { get; set; }
        public string message { get; set; }
        public string wmsData { get; set; }
    }
}