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
    
    public partial class Prc_RoomCheckOutReport_Result
    {
        public Nullable<System.DateTime> ServerDatetime { get; set; }
        public System.Guid ActivityID { get; set; }
        public string ActivityType { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public Nullable<System.DateTime> TransactionDateTime { get; set; }
        public Nullable<int> VendorID { get; set; }
        public Nullable<System.Guid> CustomerID { get; set; }
        public Nullable<int> TouchPointID { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<System.Guid> UID { get; set; }
        public string Vendor { get; set; }
        public string RoomNumber { get; set; }
    }
}
