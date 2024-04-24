using System;
using System.ComponentModel.DataAnnotations;

namespace PSL.Laundry.CentralService.Models
{
    public class ApprovalTransaction
    {
        [Required]
        public string TruckID { get; set; }
        [Required]
        public string TransactionNumber { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        public string Reason { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public bool IsApproved { get; set; }
        [Required]
        public string WeighBridgeID { get; set; }
        public int LocationID { get; set; }
        public int TouchPointID { get; set; }
        public int TransactionTypeID { get; set; }
        public string ProcessType { get; set; }
    }
    public class ApprovalTransactionResponse : Response
    {
        public ApprovalTransaction data { get; set; }
    }


    public class ApproveTransaction
    {
        [Required]
        public bool IsApproved { get; set; }
        [Required]
        public string AppovedBy { get; set; }
        [Required]
        public DateTime ApprovedDateTime { get; set; }
    }
}