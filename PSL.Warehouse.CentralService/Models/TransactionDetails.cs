using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class TransactionDetails
    {
        public string TransID { get; set; }
        public string ClientDeviceID { get; set; }
        public int AntennaID { get; set; }
        public int RSSI { get; set; }
        public string TransDatetime { get; set; }
        public string TouchPointType { get; set; }
        public string Count { get; set; }
        public string PalletTagID { get; set; }
        public string ListItemStatus { get; set; }
        public string WorkorderNumber { get; set; }
        public string WorkorderType { get; set; }
        public int CategoryID { get; set; }
        public List<SubTagData> SubTagDetails { get; set; }
    }
    public class TransactionDetailsV1
    {
        public string TransID { get; set; }
        public string ClientDeviceID { get; set; }
        public int AntennaID { get; set; }
        public int RSSI { get; set; }
        public string TransDatetime { get; set; }
        public string Count { get; set; }
        public string PalletTagID { get; set; }
        public string ListItemStatus { get; set; }
        public string WorkorderNumber { get; set; }
        public string WorkorderType { get; set; }
        public int CategoryID { get; set; }
        public List<SubTagDataV1> SubTagDetails { get; set; }
    }
}