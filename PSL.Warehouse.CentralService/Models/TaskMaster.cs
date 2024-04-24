using System;

namespace PSL.Laundry.CentralService.Models
{
    public class TaskMaster
    {
        public int? ID { get; set; }
        public string TaskNo { get; set; }
        public DateTime? TaskDate { get; set; }
        public string GateNo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string PO_STONo { get; set; }
        public string SrNo { get; set; }
        public string STOsPONo { get; set; }
        public string STOPoSrNo { get; set; }
        public string PermitNo { get; set; }

        public string Product { get; set; }
        public string Description { get; set; }
        public string TransCode { get; set; }
        public string Transporter { get; set; }
        public string Vendor { get; set; }

        public string RouteCode { get; set; }
        public string SourcePlant { get; set; }
        public string SourceLoc { get; set; }
        public string SourceBatch { get; set; }
        public string DestinationPlant { get; set; }


        public string DestinationLoc { get; set; }
        public string DestinationBatch { get; set; }
        public string Overload { get; set; }
        public string BargeMandatory { get; set; }
        public string CreatedBy { get; set; }

        public DateTime? CreatedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public int? TransType { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public bool IsActive { get; set; }

    }
}