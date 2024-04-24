using System;

namespace PSL.Laundry.CentralService.Models
{
    public class PDATripDetail
    {
        public Guid TripId { get; set; }
        public string TouchPointType { get; set; }
        public string TruckID { get; set; }
        public int TaskTypeID { get; set; }
        public string STONumber { get; set; }
        public int RouteID { get; set; }
        public string LotNumber { get; set; }
        public string VesselCode { get; set; }
        public string BundurName { get; set; }
        public string PermitNo { get; set; }
        public int printSrNo { get; set; }
        public string UserID { get; set; }
        public bool ValidatePrevTrip { get; set; }
        public PDATripDetail()
        {
            this.TripId = Guid.NewGuid();
            this.TouchPointType = string.Empty;
            this.TruckID = string.Empty;
            this.TaskTypeID = 0;
            this.STONumber = string.Empty;
            this.RouteID = 0;
            this.LotNumber = string.Empty;
            this.VesselCode = string.Empty;
            this.BundurName = string.Empty;
            this.PermitNo = string.Empty;
            this.printSrNo = 0;
            this.UserID = string.Empty;
            this.ValidatePrevTrip = false;
        }
    }
}