
using System;

namespace LaundryManagementSystem.Models
{
    public class SerialNumberMaster
    {
        public string serialno { get; set; }
        public string tagID { get; set; }
        public string custiD { get; set; }
        public DateTime transaction { get; set; }
        public string chipID { get; set; }


    }
    public class ChipMaster
    {
        public string tagID { get; set; }
        public string custiD { get; set; }
        public string chipID { get; set; }

    }



}
