using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Laundry.CentralService.Models
{
    public class Location
    {
        public short LocationID { get; set; }
        public string LocationCode { get; set; }
        public string LocationDesc { get; set; }
        public string LocationType { get; set; }
        public bool? IsWeighbridgeRequired { get; set; }
        public string Area { get; set; }
        public string LocationName { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public bool? IsActive { get; set; }
    }
}