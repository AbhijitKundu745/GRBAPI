using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Models
{
    public class WorkorderCreationDetails
    {
        //public string WorkorderID { get; set; }
        public string TruckNumber { get; set; }
        public int WarehouseID { get; set; }
        //public List<WorkorderCreationItemDetails> WorkorderDetails { get; set; }
    }
    public class WorkorderCreationDetailsU1
    {
        public List<WorkorderCreationItemDetailsU1> data { get; set; }
    }
    public class WorkorderCreationDetailsL0
    {
        public List<WorkorderCreationItemDetailsL0> data { get; set; }
    }
    public class CloseWorkorderRequest
    {
        public string TruckNumber { get; set; }
    }
    public class WorkorderCreationDetailsI0
    {
        public List<WorkorderCreationItemDetailsI0> data {get; set; }
    }
}