using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
   
        public class ScannedItemModel
        {
            public Nullable<System.Guid> CustomerID { get; set; }
            public string ClientDeviceID { get; set; }
            public string ActivityType { get; set; }
            public Nullable<int> Count { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string TouchpointID { get; set; }
            public string ParentTagID { get; set; }
            public string DRN { get; set; }
            public List<ScannedItemDetailsModel> data { get; set; }
        }
        public class ScannedItemDetailsModel
        {
            public string ItemDescription { get; set; }
            public DateTime TransactionDateTime { get; set; }
        }
    public class ManualItemModel
    {
        public Nullable<System.Guid> CustomerID { get; set; }
        public string ClientDeviceID { get; set; }
        public string ActivityType { get; set; }
        public Nullable<int> Count { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TouchpointID { get; set; }
        public string ParentTagID { get; set; }
        public string DRN { get; set; }
        public List<ActivityDetailsModelItems> data { get; set; }
    }
    public class ManualItemDetailsModel
    {
        public string ItemDescription { get; set; }
        public int Qty { get; set; }
        public DateTime TransactionDateTime { get; set; }
    }
    public class PlantActivityResponse
        {
            public bool status { get; set; }
            public bool isUnique { get; set; }
            public bool isItemUnique { get; set; }
            public string message { get; set; }
        }
}