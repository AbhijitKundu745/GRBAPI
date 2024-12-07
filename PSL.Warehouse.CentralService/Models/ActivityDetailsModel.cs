using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class ActivityDetailsModel
    {
        //public Guid ActivityDetailsID { get; set; }
        public string ItemDescription { get; set; }
        //public Guid ActivityID { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string ProcessType { get; set; }
        public string DRN { get; set; }
        public string ItemStatus { get; set; }
    }
    public class ActivityDetailsModelItems
    {
        public string ItemDescription { get; set; }
        public int Qty { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string ProcessType { get; set; }
        public string DRN { get; set; }
    }
    public class ActivityDetailsModelInternal
    {
        public string ItemDescription { get; set; }
        public DateTime TransactionDateTime { get; set; }
    }
    public class ItemsQrActivityDetailsModel
    {
        public string ItemDescription { get; set; }
        public DateTime TransactionDateTime { get; set; }
    }
}