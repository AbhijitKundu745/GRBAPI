using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class SKUDetails
    {
        public string ClientDeviceID { get; set; }
        public string DCNo { get; set; }
        public string Type { get; set; }
    }
    public class SKUResponse
    {
        public string ItemDesc { get; set; }
        public string SkuCode { get; set; }
        public string BatchID { get; set; }
        public string BinName { get; set; }
        public string PickedQty { get; set; }
    }
    public class SKUData
    {
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
    }
}