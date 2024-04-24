using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class Configuration
    {
        public int RSSI { get; set; }
        public int Power { get; set; }
        public int PollingTimer { get; set; }
    }
}