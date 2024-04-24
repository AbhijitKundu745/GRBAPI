using System;

namespace PSL.Laundry.CentralService.Models
{
    public class Tagdata
    {
        public string SerialNo { get; set; }
        public string TagType { get; set; }
        public string ReaderIP { get; set; }
        public int AntennaID { get; set; }
        public int RSSI { get; set; }
       // public DateTime TransDatetime { get; set; }
        public string TransDatetime { get; set; }
        public short LocationID { get; set; }
        public short TouchPointID { get; set; }
        public string TouchPointType { get; set; }

        //public DateTime FirstSeen { get; set; }
        //public DateTime LastSeen { get; set; }
        //public int Duration { get; set; }
        //public int TagSeenCount { get; set; }

    }
}