using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class AuthorizationWMS
    {
        public string access_token { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public List<string> roles { get; set; }
    }
}