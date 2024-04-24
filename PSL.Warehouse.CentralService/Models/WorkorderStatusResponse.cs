using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class WorkorderStatusResponse
    {
        public string WorkorderNumber { get; set; }
        public string WorkorderType { get; set; }
        public string WorkorderStatus { get; set; }
        public string TruckNumber { get; set; }
        public string CurrentScannedPalletName { get; set; }
        public string CurrentScannedPalletTagID { get; set; }
        public bool ReaderStatus { get; set; }
        public bool ApplicationStatus { get; set; }
        public int PollingTimer { get; set; }
        public string LocationName { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }

    }
   // public class WorkorderListResponse
    //{
    //    public string WorkorderNumber { get; set; }
    //    public string WorkorderType { get; set; }
    //    public string WorkorderStatus { get; set; }
    //    public string TruckNumber { get; set; }
    //    public string CurrentScannedPalletName { get; set; }
    //    public string CurrentScannedPalletTagID { get; set; }
    //    public bool ReaderStatus { get; set; }
    //    public bool ApplicationStatus { get; set; }
    //    public int PollingTimer { get; set; }
    //    public string LocationName { get; set; }
    //    public List<OrderDetails> OrderDetails { get; set; }

    //}
    public class WorkorderListResponse
    {
        public bool ReaderStatus { get; set; }
        public bool ApplicationStatus { get; set; }
        public int PollingTimer { get; set; }
        //public List<OrderDetails> OrderDetails { get; set; }
        public List<OrderDetailsV1> OrderDetails { get; set; }

    }

}