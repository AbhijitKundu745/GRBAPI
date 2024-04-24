using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class WorkOrderResponse
    { 
         public bool status { get; set; }
         public string message { get; set; }
        public bool ReaderStatus { get; set; }
        public bool ApplicationStatus { get; set; }
        public IEnumerable<object> data { get; set; }
    }
    public class PartialWorkOrderResponse
    {
        public string WorkorderNumber { get; set; }
        public string WorkorderType { get; set; }
        public string DRN { get; set; }
    }
}