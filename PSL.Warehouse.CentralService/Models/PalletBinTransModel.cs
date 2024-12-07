using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class PalletBinTransModel
    {
        public string DeviceID {get; set;}
        public string PalletTagID { get; set; }
        public string PalletName { get; set; }
        public string BinTagID { get; set; }
        public string BinName { get; set; }
    }
}