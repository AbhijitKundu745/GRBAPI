//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PSL.Warehouse.CentralService.DBContext
{
    using System;
    
    public partial class Prc_EOLLaundry_Result
    {
        public Nullable<System.DateTime> ServerDatetime { get; set; }
        public System.Guid AssetID { get; set; }
        public string AName { get; set; }
        public string ADescription { get; set; }
        public Nullable<int> ATypeID { get; set; }
        public string ATagID { get; set; }
        public Nullable<long> SID { get; set; }
        public Nullable<long> ASerialNo { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public System.Guid UID { get; set; }
        public Nullable<System.DateTime> AStateDateTime { get; set; }
        public string AState { get; set; }
        public Nullable<System.DateTime> LastInventoryDateTime { get; set; }
        public System.Guid CustomerID { get; set; }
        public Nullable<System.DateTime> TransactionDateTime { get; set; }
        public Nullable<bool> IsRegistered { get; set; }
        public Nullable<int> AssetLife { get; set; }
        public Nullable<int> ExtraUseDays { get; set; }
    }
}
