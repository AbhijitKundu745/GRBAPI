using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Laundry.CentralService.Models
{
    public class RackInfo
    {
        public List<PalletsDetails> pallets { get; set; }
        public List<ContainersDetails> containers { get; set; }
    }
}