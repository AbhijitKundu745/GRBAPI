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
    using System.Collections.Generic;
    
    public partial class ActivityDetail
    {
        public System.Guid ActivityDetailsID { get; set; }
        public System.Guid AssetID { get; set; }
        public System.Guid ActivityID { get; set; }
    
        public virtual Activity Activity { get; set; }
        public virtual AssetMaster AssetMaster { get; set; }
    }
}
