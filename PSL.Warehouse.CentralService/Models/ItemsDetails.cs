using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class ItemsDetails
    {
        public string PalletName { get; set; }
        public string ActivityStatus { get; set;}
    }
    public class ItemDetails
    {
        public string ItemCode { get; set; }
        public int Qty { get; set; }
    }
    public class ItemDescriptionDetails
    {
        public string ItemDescription { get; set; }
    }
    public class PickListItemDetails
    {
        public string ItemDescription { get; set; }
        public string BatchID { get; set; }
        public string BinName { get; set; }
        public int PickUpQty { get; set; }
    }
    public class PartialItemDetails
    {
        public string BinName { get; set; }
        public string BatchID { get; set; }
        public string ItemDescription { get; set; }
        public int Qty { get; set; }
    }
}