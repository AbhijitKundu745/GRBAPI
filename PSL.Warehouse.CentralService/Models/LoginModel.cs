using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Laundry.CentralService.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ClientDeviceID { get; set; }
    }

    public class LoginModelDesktop
    {
        public string CustomerID { get; set; }
        public string UserID { get; set; }
        public string TagPassword { get; set; }
        public string CompanyCode { get; set; }
        public List<DashboardData> Dashboards { get; set; }
        public List<AssetInfo> AssetInfos { get; set; }
        // public string FirstName { get; set; }



        //public string CustomerName { get; set; }



    }
}