using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    //public class CurrentPalletRequest
    //{
    //    public string ClientDeviceID { get; set; }
    //    public string WorkorderNumber { get; set; }
    //    public string CurrentScannedPalletTagID { get; set; }
    //   // public string CurrentScannedPalletName { get; set; }
    //}
    public class CurrentPalletRequest
    {
        public string ClientDeviceID { get; set; }
        public string CurrentScannedPalletTagID { get; set; }
    }
    public class CurrentBinRequest
    {
        public string ClientDeviceID { get; set; }
        public string CurrentScannedBinTagID { get; set; }
    }
}