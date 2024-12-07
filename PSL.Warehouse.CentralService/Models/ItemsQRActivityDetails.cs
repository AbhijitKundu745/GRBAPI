using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class ItemsQRActivityDetails
    {
        public string Id { get; set; }
        public string ParentActivityId { get; set; }
        public string ItemDescription { get; set; }
        public string ItemSerialNo { get; set; }
        public string ItemCode { get; set; }
        public string BatchId { get; set; }
        public string BatchDateTime { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public DateTime Serverdatetime { get; set; }
        public string ServerDateTimeSTR { get; set; }
    }
    public class ItemsQRActivity
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string DeviceId { get; set; }
        public int TouchPointId { get; set; }
        public string PlantId { get; set; }
        public long Count { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string StartDateTimeSTR { get; set; }
        public string EndDateTimeSTR { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public bool IsRetrieved { get; set; }
        public DateTime ServerDateTime { get; set; }
        public string ServerDateTimeSTR { get; set; }
    }
    public class QRTranscationDetails
    {
        public string TranscationNo { get; set; }
        public string TranscationId { get; set; }
        public ItemsQRActivity activity { get; set; }
        public List<ItemsQRActivityDetails> data { get; set; }
    }
}