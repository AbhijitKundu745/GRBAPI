using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class WorkOrderDetails
    {
        public string DeviceID { get; set; }
        public string WONumber { get; set; }
        public string ActivityType { get; set; }
        public string ActivityStatus { get; set; }
        public string ProcessNumber { get; set; }
        public string TruckNumber { get; set; }
    }
    public class PendingWorkOrderDetails
    {
        public string ClientDeviceID { get; set; }
        public string WorkorderNumber { get; set; }
        public string WorkorderType { get; set; }
        public string PalletTagID { get; set; }
        public string PalletName { get; set; }
        public string DestinationTagID { get; set; }
        public string DestinationName { get; set; }
    }
}