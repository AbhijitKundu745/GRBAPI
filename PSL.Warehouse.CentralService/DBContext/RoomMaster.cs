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
    
    public partial class RoomMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoomMaster()
        {
            this.Activities = new HashSet<Activity>();
        }
    
        public System.Guid RoomID { get; set; }
        public string RoomNo { get; set; }
        public string RoomDesc { get; set; }
        public Nullable<System.Guid> CustomerID { get; set; }
        public string TagID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Activity> Activities { get; set; }
        public virtual CustomerMaster CustomerMaster { get; set; }
    }
}
