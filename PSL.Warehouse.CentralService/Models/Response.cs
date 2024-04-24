using LaundryManagementSystem.Models;
using System;
using System.Collections.Generic;

namespace PSL.Laundry.CentralService.Models
{
    public class Response
    {
        public bool status { get; set; }
        public string message { get; set; }
    }

    public class ResponseData
    {
        public bool status { get; set; }
        public string message { get; set; }
        public IEnumerable<object> data { get; set; }

       
    }

    public class ResponseObjectData
    {
        public bool status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
    

}