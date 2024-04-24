using System;

namespace PSL.Laundry.CentralService.Models
{
    public class TransactionType
    {
        public int? ID { get; set; }
        public string Description { get; set; }
        public int? LocationType { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public bool IsActive { get; set; }
    }
}