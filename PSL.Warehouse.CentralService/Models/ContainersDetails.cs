using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Laundry.CentralService.Models
{
    public class ContainersDetails
    {
        public string pallet_tag_id { get; set; }
        public string pallet_name { get; set; }
        public string container_tag_id { get; set; }
        public string container_name { get; set; }
        public string status { get; set; }
        public string message { get; set; }
    }

    public class ContainersData
    {
        public UserData UserDatas { get; set; }
        public string ContainerID { get; set; }
        public List<string> Pallets { get; set; }
    }

    public class ContainerCheck
    {
        public PalletsDetails PalletsDetails { get; set;}
        public ContainersDetails ContainersDetails { get; set; }
        public string status { get; set; }
    }
}