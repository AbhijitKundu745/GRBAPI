using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class PartialTransactionDetails
    {
        public string CustomerID { get; set; }
        public string ClientDeviceID { get; set; }
        public string WorkorderNumber { get; set; }
        public string WorkorderType { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string PalletTagID { get; set; }
        public string PalletName { get; set; }
        public string LocationTagID { get; set; }
        public string LocationCategoryID { get; set; }
        public List<PartialItemDetails> ItemDetails { get; set; }
    }
}