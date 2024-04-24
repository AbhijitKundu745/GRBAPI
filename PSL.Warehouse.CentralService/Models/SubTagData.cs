using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class SubTagData
    {
        public string TagID { get; set; }
        public int Count { get; set; }
        public string RSSI { get; set; }
        public int CategoryID { get; set; }
        public string TagType { get; set; }
        public string TransDatetime { get; set; }
    }
    public class SubTagDataV1
    {
        public string SuggestedDestinationTagID { get; set; }
        public string ActualDestinationTagID { get; set; }
        public int Count { get; set; }
        public string RSSI { get; set; }
        public int CategoryID { get; set; }
        public string TagType { get; set; }
        public string TransDatetime { get; set; }
    }
}