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
    
    public partial class Prc_GetDashboardCountData_Result
    {
        public Nullable<int> CheckInCount { get; set; }
        public Nullable<int> CheckOutCount { get; set; }
        public Nullable<int> PendingLaundryCount { get; set; }
        public Nullable<int> MissingLaundryCount { get; set; }
        public Nullable<int> EOLLaundryCount { get; set; }
        public Nullable<int> AvgCheckINCount { get; set; }
        public Nullable<int> AvgCheckOUTCount { get; set; }
    }
}
