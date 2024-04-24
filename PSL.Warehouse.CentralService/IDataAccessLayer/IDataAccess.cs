using PSL.Laundry.CentralService.Models;
using System;
using System.Collections.Generic;

namespace PSL.Warehouse.CentralService.IDataAccessLayer
{
    public interface IDataAccess
    {
        Response CreateApprovalTransaction(ApprovalTransaction transactionInput);

        ApprovalTransaction GetApprovalTransaction(string transactionNumber, string truckID, char GetBy);

        Response ApproveTransaction(Guid id, ApproveTransaction transaction);

        int InsertTripDetail(TripDetailsInfo tripDetailsInfo);

        Response InsertTagData(Tagdata tagdata);

        PreviousTripDetail GetPreviousTripdetail(string truckId);

        TripDetailsInfo GetLastTripByTruckId(string truckId);

        List<NotificationConfiguration> GetNotificationConfiguration(string weighbridgeId);
    }
}
