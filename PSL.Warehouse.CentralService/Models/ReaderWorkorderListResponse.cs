using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    //public class ReaderWorkorderListResponse
    //{
    //    public bool status { get; set; }
    //    public string message { get; set; }
    //    public Configuration configuration { get; set; }
    //    public bool StartWorkOrder { get; set; }
    //    public string WorkorderNumber { get; set; }
    //    public string WorkorderType { get; set; }
    //    public string WorkorderStatus { get; set; }
    //    public List<OrderDetails> data { get; set; }

    //}
    public class ReaderWorkorderListResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public Configuration configuration { get; set; }
        public bool StartWorkOrder { get; set; }
        public List<OrderDetails> data { get; set; }

    }
}