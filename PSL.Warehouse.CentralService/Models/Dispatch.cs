using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class Dispatch
    {
        public string totalPages { get; set; }
        public string totalElements { get; set; }
        public DispatchPageable pageable { get; set; }
        public bool first { get; set; }
        public bool last { get; set; }
        public string size { get; set; }
        public List<DispatchData> content { get; set; }
        public string number { get; set; }
        public List<DispatchSort> sort { get; set; }
        public string numberOfElements { get; set; }
        public bool empty { get; set; }
    }
    public class DispatchPageable
    {
        public bool unpaged { get; set; }
        public string pageNumber { get; set; }
        public string pageSize { get; set; }
        public bool paged { get; set; }
        public string offset { get; set; }
        public List<DispatchSort> sort { get; set; }

    }
    public class DispatchData
    {
        public int createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public int updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string recordStatus { get; set; }
        public int id { get; set; }
        public string dispatchNo { get; set; }
        public DateTime expectedDeliveryDate { get; set; }
        public WHContent warehouse { get; set; }
        public SupplierType status { get; set; }
        public string billingDocument { get; set; }
        public string billingType { get; set; }
        public DateTime billingDate { get; set; }
        public string payerCode { get; set; }
        public string payerName { get; set; }
        public string customerGroup { get; set; }
        public string truckNumber { get; set; }
        public double freightCharges { get; set; }
        public double otherCharges { get; set; }
        public string pgi { get; set; }
        public List<DispatchItems> items { get; set; }
        //public List<PickSuggestion> pickSuggestions { get; set; }
    }
    public class DispatchSort
    {
        public string direction { get; set; }
        public string property { get; set; }
        public bool ignoreCase { get; set; }
        public string nullHandling { get; set; }
        public bool ascending { get; set; }
        public bool descending { get; set; }
    }
    public class WHContent
    {
        //public string createdBy { get; set; }
        //public DateTime createdAt { get; set; }
        //public string updatedBy { get; set; }
        //public DateTime updatedAt { get; set; }
        public int id { get; set; }
        public string plantNumber { get; set; }
        public string name { get; set; }
        public string name1 { get; set; }
        public string name2 { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string pin { get; set; }
        public string countryCode { get; set; }
        public string regionCode { get; set; }
        //public string warehouseManagerUserId { get; set; }
        //public string contactEmail { get; set; }
        //public string contactPhone { get; set; }

    }
    public class SupplierType
    {
        public string createdBy { get; set; }
        public DateTime createdAt { get; set; }
        //public string updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

    }
    public class DispatchItems
    {
        public int createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public int updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public int id { get; set; }
        //public string orderNo { get; set; }
        //public string orderType { get; set; }
        public string orderItemNo { get; set; }
        //public SalesOrder salesOrder { get; set; } //
        //public ItemsD salesOrderItem { get; set; }//
        public SKUContent sku { get; set; }
        public string skuCode { get; set; }
        public string skuName { get; set; }
        public string recordStatus { get; set; }
        //public string orderQty { get; set; }
        public double qty { get; set; }
        public double loadedQty { get; set; }
        //public string weight { get; set; }
        public double netWeight { get; set; }
        public double grossWeight { get; set; }
        public string volume { get; set; }
        public string volumeUnit { get; set; }
    }
    public class SKUContent
    {
        public int createdBy { get; set; }
        public DateTime createdAt { get; set; }
        public int updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public int id { get; set; }
        public SPU spu { get; set; } //
        public string code { get; set; }
        public string name { get; set; }
        public double qty { get; set; }
        public string recordStatus { get; set; }
        public string unit { get; set; }
        public string height { get; set; }
        public string width { get; set; }
        public string length { get; set; }
        public string volume { get; set; }
        public double weight { get; set; }
        public double qtyInPallet { get; set; }
        public string rotationRate { get; set; }
        public string abcRank { get; set; }
        public string maxShelfLife { get; set; }
        public string qtySale { get; set; }
        public string unitSale { get; set; }
        public string cost { get; set; }
        public string price { get; set; }
    }
    public class SPU
    {
        public int createdBy { get; set; }
        public DateTime createdAt { get; set; }
        //public string updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string recordStatus { get; set; }
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public ProductCategory productCategory { get; set; }
        public string brand { get; set; }
    }
    public class ProductCategory
    {
        public int createdBy { get; set; }
        public DateTime createdAt { get; set; }
        //public string updatedBy { get; set; }
        public DateTime updatedAt { get; set; }
        public string recordStatus { get; set; }
        public int id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        //public string parentId { get; set; }
    }

}