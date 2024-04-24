﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class TruckDetails
    {
        public string TruckNumber { get; set; }
    }
    public class TruckIDDetails
    {
        public string TruckTagID { get; set; }
    }
    public class TruckResponseDetails
    {
        public string TruckNumber { get; set; }
        public string DRN { get; set; }
        public string ProcessType { get; set; }
        public string LocationName { get; set; }
    }
    public class TruckResponseDetailsByLocation
    {
        public string TruckNumber { get; set; }
        public string DRN { get; set; }
        public string ProcessType { get; set; }
        public List<PalletItemDetails> PalletItemDetails { get; set; }
        public List<ItemDetails> ItemDetailSummary { get; set; }
    }
}