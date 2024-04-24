using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class TagDetailsRequest
    {
        public List<TagLoggerDetails> TagDetails { get; set; }
    }
    public class TagLoggerDetails
    {
        public string PalletTagID { get; set; }
        public int RSSI { get; set; }
        public string Count { get; set; }
        public string TransDatetime { get; set; }
        public string ClientDeviceID { get; set; }
        public int AntennaID { get; set; }
    }
}