﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Laundry.CentralService.Models
{
    public class RegisterModel
    {
        public System.Guid AssetID { get; set; }

        public Nullable<long> SID { get; set; }
        public long ASerialNo { get; set; }
        public Nullable<System.Guid> UID { get; set; }
        public Guid CustomerID { get; set; }
        public string ATagID { get; set; }
    }

    public class RegisterModelMobile
    {
        public System.Guid AssetID { get; set; }
        public long ASerialNo { get; set; }
        public Guid CustomerID { get; set; }
        public string ATagID { get; set; }
        public Nullable<System.Guid> UID { get; set; }
        public string DeviceID { get; set; }
        public string TouchpointID { get; set; }
    }
}