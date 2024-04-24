using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class WorkorderStatusRequest
    {
        public string ClientDeviceID { get; set; }
        public string WorkorderNumber { get; set; }
        public string WorkorderStatus { get; set; }
    }
    public class PartialWorkorderListRequest
    {
        public string ClientDeviceID { get; set; }
        public string WorkorderNumber { get; set; }
        public string WorkorderType { get; set; }
    }
    public class WorkorderListRequest
    {
        public string ClientDeviceID { get; set; }
    }
}