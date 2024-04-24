using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Laundry.CentralService.Models
{
    public class LoginResponse
    {
        public Nullable<System.Guid> CustomerID { get; set; }

        public string TagPassword { get; set; }
        public string UserID { get; set; }
        
        
        public Nullable<int> CompanyCode { get; set; }
        //public System.Guid UserID { get; set; }

        //public bool ISLogRequired { get; set; }

        public  ICollection<DashboardData> Dashboard { get; set; }

        public ICollection<AssetInfo> AssetType { get; set; }

        //public  ICollection<AssetTypeLogin> AssetType { get; set; }

    }


    public class VendorLogin
    {
        public int VendorID { get; set; }
        public string Name { get; set; }
    }

    public class RoomMasterLogin
    {
        public Guid RoomID { get; set; }
        public string RoomName { get; set; }
    }


    public class AssetTypeLogin
    {
        public int ATypeID { get; set; }
        public string AssetName { get; set; }
    }




}