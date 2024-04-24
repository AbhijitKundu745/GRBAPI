using System;

namespace PSL.Laundry.CentralService.Models
{
    public class PreviousTripDetail
    {
        public string TruckID { get; set; }
        public DateTime? TripDateTime { get; set; }
        public int? Weight { get; set; }
        public int? TareWeight { get; set; }
        public short? LocationID { get; set; }
        public short? ActivityID { get; set; }
        public string ContractorSuffix { get; set; }
        public bool? IsERPEntry { get; set; }
        public string TransactionNumber { get; set; }
        public string TouchPointType { get; set; }
        public short? TouchPointID { get; set; }
        public string BargeTransactionNumber { get; set; }
        public string SAPDeliveryRequestNumber { get; set; }
        public string BargeTripNumber { get; set; }
        public string GuestType { get; set; }
        public int? TransactionTypeID { get; set; }
        public int? TaskTypeID { get; set; }
        public int? TruckTypeID { get; set; }
        public string TaskNo { get; set; }
        public string PO_STONo { get; set; }
        public string SrNo { get; set; }
        public string PermitNo { get; set; }
        public string Product { get; set; }
        public string Description { get; set; }
        public string RouteCode { get; set; }
        public string SourcePlant { get; set; }
        public string SourceLoc { get; set; }
        public string SourceBatch { get; set; }
        public string DestinationPlant { get; set; }
        public string DestinationLoc { get; set; }
        public string DestinationBatch { get; set; }
        public int? TransType { get; set; }
    }
}