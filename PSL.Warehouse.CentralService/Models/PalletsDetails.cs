using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Laundry.CentralService.Models
{
    public class PalletsDetails
    {
       public string pallet_tag_id { get; set; }
       public string pallet_name { get; set; }
       public string asset_number { get; set; }
       public string status { get; set; }
       public string message { get; set; }
    }

    public class PalletCheck
    {
        public List<PalletsDetails> Valid { get; set; }
        public List<PalletsDetails> invalid { get; set; }
    }

    public class PalletData
    {
        public UserData userDatas { get; set; }
        public string PalletID { get; set;}
        public List<string> Assets { get; set; }
    }
}