using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    //public class OrderDetails
    //{
    //    public string PalletName { get; set; }
    //    public string PalletTagID { get; set; }
    //    public DateTime LastUpdatedDateTime { get; set; }
    //    public string TemporaryStorageName { get; set; }
    //    public string TemporaryStorageTagID { get; set; }
    //    public string LoadingAreaName { get; set; }
    //    public string LoadingAreaTagID { get; set; }
    //    public string BinLocation { get; set; }
    //    public string BinLocationTagID { get; set; }
    //    public string ListItemStatus { get; set; }
    //}
    public class OrderDetails
    {
        public string WorkorderNumber { get; set; }
        public string WorkorderType { get; set; }
        public string WorkorderStatus { get; set; }
        public string TruckNumber { get; set; }
        public string CurrentScannedPalletName { get; set; }
        public string CurrentScannedPalletTagID { get; set; }
        public string LocationName { get; set; }
        public string PalletName { get; set; }
        public string PalletTagID { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public string LoadingAreaName { get; set; }
        public string LoadingAreaTagID { get; set; }
        public string TemporaryStorageName { get; set; }
        public string TemporaryStorageTagID { get; set; }
        public string BinLocation { get; set; }
        public string BinLocationTagID { get; set; }
        public string ListItemStatus { get; set; }
    }
    public class OrderDetailsV1
    {
        public string WorkorderNumber { get; set; }
        public string WorkorderType { get; set; }
        public string WorkorderStatus { get; set; }
        public string TruckNumber { get; set; }
        public string CurrentScannedPalletName { get; set; }
        public string CurrentScannedPalletTagID { get; set; }
        public string CurrentScannedBinName { get; set; }
        public string CurrentScannedBinTagID { get; set; }
        public string LocationName { get; set; }
        public string PalletName { get; set; }
        public string PalletTagID { get; set; }
        public DateTime LastUpdatedDateTime { get; set; }
        public string LoadingAreaName { get; set; }
        public string LoadingAreaTagID { get; set; }
        public string TemporaryStorageName { get; set; }
        public string TemporaryStorageTagID { get; set; }
        public string BinLocation { get; set; }
        public string BinLocationTagID { get; set; }
        public string ListItemStatus { get; set; }
    }
}