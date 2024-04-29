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
        public List<ActivityDetailsModelItems> data { get; set; }
    }


}