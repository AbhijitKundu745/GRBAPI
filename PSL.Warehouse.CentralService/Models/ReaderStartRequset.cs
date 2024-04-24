using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class ReaderStartRequset
    {
        public string ClientDeviceID { get; set; }
        public string ReaderStatus { get; set; }
    }
}