using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class ActivityModel
    {
        public Nullable<System.Guid> CustomerID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public string ClientDeviceID { get; set; }
        public string ActivityType { get; set; }
        public Nullable<int> Count { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TouchpointID { get; set; }
        public string ParentTagID { get; set; }
        public string ParentAssetType { get; set; }
        public string TruckNumber { get; set; }
        public string ProcessType { get; set; }
        public string DRN { get; set; }
        public int WarehouseID { get; set; }
        public List<ActivityDetailsModel> data { get; set; }
    }
    public class ActivityModelItems
    {
        public Nullable<System.Guid> CustomerID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public string ClientDeviceID { get; set; }
        public string ActivityType { get; set; }
        public Nullable<int> Count { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TouchpointID { get; set; }
        public string ParentTagID { get; set; }
        public string ParentAssetType { get; set; }
        public string TruckNumber { get; set; }
        public string ProcessType { get; set; }
        public string DRN { get; set; }
        public int WarehouseID { get; set; }
        public List<ActivityDetailsModelItems> data { get; set; }
    }
    public class ActivityModelInternal
    {
        public Nullable<System.Guid> CustomerID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public string ClientDeviceID { get; set; }
        public string ActivityType { get; set; }
        public Nullable<int> Count { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SourcePalletTagID { get; set; }
        public string DestinationPalletTagID { get; set; }
        public string SourceBinTagID { get; set; }
        public string DestinationBinTagID { get; set; }
        //public string ProcessType { get; set; }
        //public string DRN { get; set; }
        public List<ActivityDetailsModelInternal> data { get; set; }
    }
    public class ItemsQRActivityModel
    {
        public Nullable<System.Guid> CustomerID { get; set; }
        public string UserID { get; set; }
        public string ClientDeviceID { get; set; }
        public string LocationID { get; set; }
        public string PlantID { get; set; }
        public Nullable<int> Count { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public List<ItemsQrActivityDetailsModel> items { get; set; }
    }
}