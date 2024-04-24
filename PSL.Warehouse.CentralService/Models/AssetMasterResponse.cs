using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Laundry.CentralService.Models
{
    //public class AssetMasterResponse
    //{
    //    public string AssetID { get; set; }

    //    public string AName { get; set; }

    //    public Nullable<int> ATypeID { get; set; }

    //    public Nullable<long> ASerialNo { get; set; }

    //    public Nullable<bool> IsRegistered { get; set; }
    //}

    public class AssetMasterResponse
    {
        public string tagid { get; set; }

        public string name { get; set; }

        public string type { get; set; }
        //public string status { get; set; }
        //public string message { get; set; }

        //public Nullable<long> ASerialNo { get; set; }

        // public Nullable<bool> IsRegistered { get; set; }
    }

    public class LostAssets
    {
        public System.Guid AssetID { get; set; }
        public string TagID { get; set; }

    }
}