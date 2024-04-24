using System;

namespace PSL.Laundry.CentralService.Models
{
    public class TouchPointModel
    {
        public int Id { get; set; }
        public short TouchPointId { get; set; }
        public string TouchpointName { get; set; }
        public short? LocationId { get; set; }
        public bool? IsDistinationPoint { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public bool? IsActive { get; set; }
    }
}