
using System;

namespace LaundryManagementSystem.Models
{
    public class AssetModel
    {
        public int ATypeID { get; set; }
        public string AssetName { get; set; }
        public string AssetID { get; set; }



    }

    public class AssetMasterModel
    {
        public string AssetID { get; set; }
        public string CustomerID { get; set; }
        public string UserID { get; set; }

        public long SID { get; set; }
        public string AName { get; set; }
        public string AState { get; set; }

        public string ADescription { get; set; }

        public int ATypeID { get; set; }

        public string ATagID { get; set; }

        public long ASerialNo { get; set; }

        public byte IsActive { get; set; }

        public byte IsRegistered { get; set; }
        public DateTime AStateDateTime { get; set; }

        public DateTime LastInventoryDateTime { get; set; }

        public DateTime TransactionDateTime { get; set; }
        public string AssetName { get; set; }
        public int count { get; set; }
        public int AssetLife { get; set; }

    }
}
