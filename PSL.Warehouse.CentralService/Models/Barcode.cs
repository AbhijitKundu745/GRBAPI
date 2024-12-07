using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class Barcode
    {
        public string ItemDescription { get; set; }
        public string ClientDeviceID { get; set; }
    }
    public class BarcodeResponse
    {
        public string ItemDescription { get; set; }
    }
}