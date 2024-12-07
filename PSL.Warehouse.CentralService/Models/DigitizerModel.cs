using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
   
        public class DigitizerModel
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
            public List<DigitizerModelItems> items { get; set; }
        }
        public class DigitizerModelItems
        {
            public string ItemDescription { get; set; }
            public string ItemSerialNo { get; set; }
            public string ItemName { get; set; }
            public string ItemCode { get; set; }
            public string BatchID { get; set; }
            public string BatchDateTime { get; set; }
            public string Weight { get; set; }
            public DateTime TransactionDateTime { get; set; }
        }
    }