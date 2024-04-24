using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class WMSPalletData
    {
        public string receivingNo { get; set; }
        public List<WmsItems> items { get; set; }
        public string warehouseId { get; set; }
        public string palletName { get; set; }
        public string binName { get; set; }
    }
    public class WmsItems
    {
        public string serialNumber { get; set; }
        public string skuCode { get; set; }
        public string batchNumber { get; set; }
        public string lotNumber { get; set; }
        public string batchDateTime { get; set; }
        public string expirationDate { get; set; }
        public List<dynamic> qrCodeImage { get; set; }
        public WmsQRInfo qrInfo { get; set; }
        public string grossWeight { get; set; }
        public string grossWeightUnit { get; set; }
        public string netWeight { get; set; }
        public string netWeightUnit { get; set; }
        public string mrp { get; set; }
        public string status { get; set; }
        public double qty { get; set; }

    }

    public class WmsQRInfo
    {
        public string serialNumber { get; set; }
        public string skuCode { get; set; }
        public string batchNumber { get; set; }
        public string grossWeight { get; set; }
        public string grossWeightUnit { get; set; }
        public string netWeight { get; set; }
        public string netWeightUnit { get; set; }
        public string mrp { get; set; }
    }
    public class WMSPalletDataDispatch
    {
        public string dispatchNo { get; set; }
        public string comment { get; set; }
        public List<WmsItems> items { get; set; }
        public string warehouseId { get; set; }
        public string palletName { get; set; }
        public string binName { get; set; }
    }
    //public class WmsItemsDispatch
    //{
    //    public string serialNumber { get; set; }
    //    public string skuCode { get; set; }
    //    public string batchNumber { get; set; }
    //    public string lotNumber { get; set; }
    //    public DateTime batchDateTime { get; set; }
    //    public DateTime expirationDate { get; set; }
    //    public List<dynamic> qrCodeImage { get; set; }
    //    public WmsQRInfoDispatch qrInfo { get; set; }
    //    public string grossWeight { get; set; }
    //    public string grossWeightUnit { get; set; }
    //    public string netWeight { get; set; }
    //    public string netWeightUnit { get; set; }
    //    public string mrp { get; set; }
    //    public string status { get; set; }
    //    public double qty { get; set; }
    //}
    //public class WmsQRInfoDispatch
    //{
    //    public string serialNumber { get; set; }
    //    public string skuCode { get; set; }
    //    public string batchNumber { get; set; }
    //    public string grossWeight { get; set; }
    //    public string grossWeightUnit { get; set; }
    //    public string netWeight { get; set; }
    //    public string netWeightUnit { get; set; }
    //    public string mrp { get; set; }

    //}
}