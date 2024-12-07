using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class WmsPalletBinInfo
    {
        public string warehouseId { get; set; }
        public string receivingNo { get; set; }
        public string fromBinName { get; set; }
        public string toBinName { get; set; }
        public string palletId { get; set; }
        public string palletName { get; set; }
        public string palletSensorId { get; set; }
    }
    public class WmsPalletBinInfoL0
    {
        public string dispatchNo { get; set; }
        public string toBinName { get; set; }
        public string toPalletName { get; set; }
        public List<WmsItemsList> items { get; set; }
    }
    public class WmsPalletBinInfoL1
    {
        public string warehouseId { get; set; }
        public string fromBinName { get; set; }
        public string toBinName { get; set; }
        public string palletId { get; set; }
        public string palletName { get; set; }
        public string palletSensorId { get; set; }
    }
    public class WmsItemsList
    {
        public int? stockBinId { get; set; }
        public string fromBinName { get; set; }
        public string fromPalletName { get; set; }
        public string skuCode { get; set; }
        public string batchNumber { get; set; }
        public string lotNumber { get; set; }
        public string batchDateTime { get; set; }
        public string expirationDate { get; set; }
        public int qty { get; set; }
    }
    public class WmsPalletBinInfoInternal
    {
       public List<WmsPalletBinInfoInternalDetails> data { get; set; }
    }
    public class WmsPalletBinInfoInternalDetails
    {
        public string warehouseId { get; set; }
        public string fromBinName { get; set; }
        public string toBinName { get; set; }
        public string fromPalletName { get; set; }
        public string toPalletName { get; set; }
        public List<WmsItemsInternal> items { get; set; }
    }
    public class WmsItemsInternal
    {
        public string serialNumber { get; set; }
        public string skuCode { get; set; }
        public string batchNumber { get; set; }
        public string batchDateTime { get; set; }
        public WmsQRInfoInternal qrInfo { get; set; }
        public string status { get; set; }
        public double qty { get; set; }

    }
    public class WmsQRInfoInternal
    {
        public string serialNumber { get; set; }
        public string skuCode { get; set; }
        public string batchNumber { get; set; }
    }
}