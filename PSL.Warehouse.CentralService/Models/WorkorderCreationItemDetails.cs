using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    //public class WorkorderCreationItemDetails
    //{
    //    public string PalletName { get; set; }
    //}
    public class WorkorderCreationItemDetails
    {
        public string TruckNumber { get; set; }
        public string PalletTagID { get; set; }
    }

    public class WorkorderCreationItemDetailsU1
    {
        public string receivingNo { get; set; }
        public string palletName { get; set; }
        public string fromBayName { get; set; }
        public string fromBinName { get; set; }
        public string toBinName { get; set; }
        public int warehouseId { get; set; }
    }
    public class WorkorderCreationItemDetailsL0
    {
        public string dispatchNo { get; set; }
        public string skuCode { get; set; }
        public string skuName { get; set; }
        public string batchNumber { get; set; }
        public string lotNumber { get; set; }
        public DateTime batchDateTime { get; set; }
        public DateTime expirationDate { get; set; }
        public string fromBinName { get; set; }
        public string toBinName { get; set; }
        public string palletName { get; set; }
        public double qty { get; set; }
        public bool isFull { get; set; }
        public int warehouseId { get; set; }
    }
    public class WorkorderCreationItemDetailsI0
    {
        public string palletName { get; set; }
        public string fromBinName { get; set; }
        public string toBinName { get; set; }
    }
}