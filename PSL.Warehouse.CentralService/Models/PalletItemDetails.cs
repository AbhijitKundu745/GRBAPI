using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class PalletItemDetails
    {
        
            public string PalletName { get; set; }
            public List<ItemDetails> ItemDetails { get; set; }
    }
}