
using PSL.Laundry.CentralService.Logger;
using PSL.Laundry.CentralService.Models;
using PSL.Warehouse.CentralService.IDataAccessLayer;
using PSL.Warehouse.CentralService.Logger;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace PSL.Laundry.CentralService.Controllers
{
    public class TransactionDetailsController : ApiController
    {
        #region properties
        ILogger<TransactionDetailsController> Log;
        IDataAccess dataAccessDAO;
        #endregion

        public TransactionDetailsController(ILogger<TransactionDetailsController> logger, IDataAccess dataAccessDAO)
        {
            this.Log = logger;
            this.dataAccessDAO = dataAccessDAO;
        }

        //// POST: api/TransactionDetails
        //public int Post([FromBody]Tagdata tagdata)
        //{
        //    int retval = 0;
        //    try
        //    {
        //        Log.Debug("TransactionDetailsController POst => ");
        //        if (tagdata != null)
        //        {
        //            Response response = dataAccessDAO.InsertTagData(tagdata);
        //            if (response == null)
        //            {
        //                throw new Exception("Transaction failed");
        //            }
        //            else if (response.status)
        //            {
        //                retval = 1;
        //            }
        //            else
        //            {
        //                retval = 0;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        retval = 0;
        //        Log.Error("TransactionDetailsController => " + ex.Message + "\n" + ex.StackTrace);
        //    }
        //    return retval;
        //}

       

        //public ResponseData GetLastTrip(string truckId)
        //{
        //    ResponseData response = new ResponseData();
        //    try
        //    {
        //        if (string.IsNullOrEmpty(truckId))
        //            throw new ArgumentException("truckId is required");

        //        TripDetailsInfo tripDetail = dataAccessDAO.GetLastTripByTruckId(truckId);
        //        if (tripDetail != null)
        //        {
        //            response.status = true;
        //            response.data = new List<Object> { tripDetail };
        //        }
        //        else
        //            throw new ArgumentException($"No Material found for truck {truckId}");

        //    }
        //    catch (ArgumentException ex)
        //    {
        //        response.status = false;
        //        response.message = ex.Message;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.status = false;
        //        response.message = ex.Message;
        //        Log.Error("TransactionDetailsController get= >" + ex.Message + "\n" + ex.StackTrace);

        //    }
        //    return response;
        //}


    }
}
