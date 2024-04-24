using Dapper;
//using PSL.Laundry.CentralService.Global;
//using PSL.Laundry.CentralService.IDataAccessLayer;
using PSL.Laundry.CentralService.Models;
using PSL.Warehouse.CentralService.Global;
using PSL.Warehouse.CentralService.IDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PSL.Laundry.CentralService.DataAccessLayer
{
    public class DataAccess : DAL, IDataAccess
    {
        public DataAccess() : base(AppSettings.ConnectionString)
        {

        }

        public Response CreateApprovalTransaction(ApprovalTransaction transactionInput)
        {
            Response response = null;
            using (IDbConnection db = GetConnection())
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@TruckID", transactionInput.TruckID);
                _params.Add("@TransactionNumber", transactionInput.TransactionNumber);
                _params.Add("@Quantity", transactionInput.Quantity);
                _params.Add("@Reason", transactionInput.Reason);
                _params.Add("@TransactionDateTime", transactionInput.TransactionDateTime);
                _params.Add("@WeighBridgeID", transactionInput.WeighBridgeID);
                _params.Add("@LocationID", transactionInput.LocationID);
                _params.Add("@TouchPointID", transactionInput.TouchPointID);
                _params.Add("@TransactionTypeID", transactionInput.TransactionTypeID);
                _params.Add("@ProcessType", transactionInput.ProcessType);
                response = db.Query<Response>(AppSettings.SQLQueryCommand.SP_Insert_ApprovalRecord, _params, commandTimeout: 60, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return response;
        }

        public ApprovalTransaction GetApprovalTransaction(string transactionNumber, string truckID, char GetBy)
        {
            ApprovalTransaction transaction = null;
            using (IDbConnection db = GetConnection())
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@TransactionNumber", transactionNumber);
                _params.Add("@TruckID", truckID);
                _params.Add("@GetBy", GetBy);
                transaction = db.Query<ApprovalTransaction>(AppSettings.SQLQueryCommand.SP_Get_ApprovalRecord, _params, commandTimeout: 60, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return transaction;
        }

        public Response ApproveTransaction(Guid id, ApproveTransaction transaction)
        {
            Response response = null;
            using (IDbConnection db = GetConnection())
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@ID", id);
                _params.Add("@IsApproved", transaction.IsApproved);
                _params.Add("@AppovedBy", transaction.AppovedBy);
                _params.Add("@ApprovedDateTime", transaction.ApprovedDateTime);
                response = db.Query<Response>(AppSettings.SQLQueryCommand.SP_ApproveTransaction, _params, commandTimeout: 60, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return response;
        }

        public int InsertTripDetail(TripDetailsInfo tripDetailsInfo)
        {
            int AR = 0;
            using (IDbConnection db = GetConnection())
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@TripDateTime", tripDetailsInfo.TripDateTime);
                _params.Add("@TripNumber", tripDetailsInfo.TripNumber);
                _params.Add("@PermitNumber", tripDetailsInfo.PermitNumber);
                _params.Add("@PrintSrNo", tripDetailsInfo.PrintSrNo);
                _params.Add("@TruckID", tripDetailsInfo.TruckID);
                _params.Add("@TruckRegNo", tripDetailsInfo.TruckRegNo);
                _params.Add("@IsWeighbridgeRequired", tripDetailsInfo.IsWeighbridgeRequired);
                _params.Add("@IsTripStartPoint", tripDetailsInfo.IsTripStartPoint);
                _params.Add("@IsTripCompletionPoint", tripDetailsInfo.IsTripCompletionPoint);
                _params.Add("@WeighbridgeID", tripDetailsInfo.WeighbridgeID);
                _params.Add("@Weight", tripDetailsInfo.Weight);
                _params.Add("@WeighingDateTime", tripDetailsInfo.WeighingDateTime);
                _params.Add("@TareWeight", tripDetailsInfo.TareWeight);
                _params.Add("@LastLocationID", tripDetailsInfo.LastLocationID);
                _params.Add("@LocationID", tripDetailsInfo.LocationID);
                _params.Add("@DestinationLocationID", tripDetailsInfo.DestinationLocationID);
                _params.Add("@RouteID", tripDetailsInfo.RouteID);
                _params.Add("@LocationOrder", tripDetailsInfo.LocationOrder);
                _params.Add("@MachineID", tripDetailsInfo.MachineID);
                _params.Add("@MaterialID", tripDetailsInfo.MaterialID);
                _params.Add("@ItemID", tripDetailsInfo.ItemID);
                _params.Add("@ProductID", tripDetailsInfo.ProductID);
                _params.Add("@OperationID", tripDetailsInfo.OperationID);
                _params.Add("@ActivityID", tripDetailsInfo.ActivityID);
                _params.Add("@LotNumber", tripDetailsInfo.LotNumber);
                _params.Add("@UserID", tripDetailsInfo.UserID);
                _params.Add("@IsSystemGeneratedRecord", tripDetailsInfo.IsSystemGeneratedRecord);
                _params.Add("@PrintDateTime", tripDetailsInfo.PrintDateTime);
                _params.Add("@ContractorSuffix", tripDetailsInfo.ContractorSuffix);
                _params.Add("@IsERPEntry", tripDetailsInfo.IsERPEntry);
                _params.Add("@TransactionNumber", tripDetailsInfo.TransactionNumber);

                _params.Add("@TouchPointType", tripDetailsInfo.TouchPointType);
                _params.Add("@InvalidLocation", tripDetailsInfo.InvalidLocation);
                _params.Add("@TouchPointID", tripDetailsInfo.TouchPointID);
                _params.Add("@TagTransactionNumber", tripDetailsInfo.TagTransactionNumber);
                _params.Add("@BargeID", tripDetailsInfo.BargeID);
                _params.Add("@VesselCode", tripDetailsInfo.VesselCode);
                _params.Add("@BargeTransactionNumber", tripDetailsInfo.BargeTransactionNumber);
                _params.Add("@SAPDeliveryRequestNumber", tripDetailsInfo.SAPDeliveryRequestNumber);
                _params.Add("@STONumber", tripDetailsInfo.STONumber);

                _params.Add("@STOLineNumber", tripDetailsInfo.STOLineNumber);
                _params.Add("@IsPGI", tripDetailsInfo.IsPGI);
                _params.Add("@IsGRN", tripDetailsInfo.IsGRN);
                _params.Add("@IsSSIG", tripDetailsInfo.IsSSIG);
                _params.Add("@SAPDestinationBatch", tripDetailsInfo.SAPDestinationBatch);
                _params.Add("@BargeTripNumber", tripDetailsInfo.BargeTripNumber);
                _params.Add("@IsPreviousTripClosed", tripDetailsInfo.IsPreviousTripClosed);
                _params.Add("@BargeCode", tripDetailsInfo.BargeCode);

                _params.Add("@TGSN", tripDetailsInfo.TGSN);
                _params.Add("@ShortTripStackID", tripDetailsInfo.ShortTripStackID);
                _params.Add("@ShortTripBatchID", tripDetailsInfo.ShortTripBatchID);
                _params.Add("@WorkCenterID", tripDetailsInfo.WorkCenterID);
                _params.Add("@SourceSLocID", tripDetailsInfo.SourceSLocID);
                _params.Add("@DestinationSLocID", tripDetailsInfo.DestinationSLocID);
                _params.Add("@PlantID", tripDetailsInfo.PlantID);
                _params.Add("@BundurName", tripDetailsInfo.BundurName);

                _params.Add("@JettyName", tripDetailsInfo.JettyName);
                _params.Add("@ShiftCode", tripDetailsInfo.ShiftCode);
                _params.Add("@ComputerIP", tripDetailsInfo.ComputerIP);
                _params.Add("@ComputerMac", tripDetailsInfo.ComputerMac);
                _params.Add("@ReaderIP", tripDetailsInfo.ReaderIP);
                _params.Add("@IsLogicalTouchPoint", tripDetailsInfo.IsLogicalTouchPoint);
                _params.Add("@logicalTagID", tripDetailsInfo.logicalTagID);

                _params.Add("@DriverID", tripDetailsInfo.DriverID);
                _params.Add("@SecurityGateName", tripDetailsInfo.SecurityGateName);
                _params.Add("@IsSecuritySource", tripDetailsInfo.IsSecuritySource);
                _params.Add("@IsTruckLoaded", tripDetailsInfo.IsTruckLoaded);
                _params.Add("@TruckCapacity", tripDetailsInfo.TruckCapacity);
                _params.Add("@ExpectedQty", tripDetailsInfo.ExpectedQty);
                _params.Add("@PermitNo", tripDetailsInfo.PermitNo);
                _params.Add("@GateEntryNo", tripDetailsInfo.GateEntryNo);
                _params.Add("@Remarks", tripDetailsInfo.Remarks);
                _params.Add("@SerialNo", tripDetailsInfo.SerialNo);
                _params.Add("@DocumentNo", tripDetailsInfo.DocumentNo);
                _params.Add("@DeliveryNo", tripDetailsInfo.DeliveryNo);
                _params.Add("@BillingDocNo", tripDetailsInfo.BillingDocNo);
                _params.Add("@GateOutFlag", tripDetailsInfo.GateOutFlag);
                _params.Add("@PersonName", tripDetailsInfo.PersonName);
                _params.Add("@GuestType", tripDetailsInfo.GuestType);
                _params.Add("@CompanyName", tripDetailsInfo.CompanyName);
                _params.Add("@MobileNumber", tripDetailsInfo.MobileNumber);
                _params.Add("@WhomToMeet", tripDetailsInfo.WhomToMeet);
                _params.Add("@TransactionTypeID", tripDetailsInfo.TransactionTypeID);
                _params.Add("@TaskTypeID", tripDetailsInfo.TaskTypeID);
                _params.Add("@TruckTypeID", tripDetailsInfo.TruckTypeID);
                _params.Add("@IsMobileLoading", tripDetailsInfo.IsMobileLoading);
                AR = db.Execute(AppSettings.SQLQueryCommand.SP_Insert_Tripdetail, _params, commandTimeout: 60, commandType: CommandType.StoredProcedure);
            }
            return AR;
        }

        public Response InsertTagData(Tagdata tagdata)
        {
            Response response = null;
            using (IDbConnection db = GetConnection())
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@SerialNo", tagdata.SerialNo);
                _params.Add("@Type", tagdata.TagType);
                _params.Add("@ReaderIP", tagdata.ReaderIP);
                _params.Add("@AntennaID", tagdata.AntennaID);
                _params.Add("@RSSI", tagdata.RSSI);
                _params.Add("@TransDatetime", Convert.ToDateTime(tagdata.TransDatetime));
                _params.Add("@LocationID", tagdata.LocationID);
                _params.Add("@TouchPointID", tagdata.TouchPointID);
                _params.Add("@TouchPointType", tagdata.TouchPointType);
                response = db.Query<Response>(AppSettings.SQLQueryCommand.SP_Insert_Transactiondetail, _params, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return response;
        }

        public PreviousTripDetail GetPreviousTripdetail(string truckId)
        {
            PreviousTripDetail tripDetail = null;
            using (IDbConnection db = GetConnection())
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@TruckId", truckId);
                tripDetail = db
                    .Query<PreviousTripDetail>(AppSettings.SQLQueryCommand.SP_GET_PreviousTripDetails, _params, commandType: CommandType.StoredProcedure)
                    .FirstOrDefault();
            }
            return tripDetail;
        }



        public TripDetailsInfo GetLastTripByTruckId(string truckId)
        {
            TripDetailsInfo tripDetail = null;
            using (IDbConnection db = GetConnection())
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@TruckId", truckId);
                tripDetail = db
                    .Query<TripDetailsInfo>(AppSettings.SQLQueryCommand.SP_GET_LastTripDetailsbyTruckID, _params, commandType: CommandType.StoredProcedure)
                    .FirstOrDefault();
            }
            return tripDetail;
        }

        public List<NotificationConfiguration> GetNotificationConfiguration(string weighbridgeId)
        {
            List<NotificationConfiguration> configurations = null;
            using (IDbConnection db = GetConnection())
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@WeighbridgeId", weighbridgeId);
                configurations = db
                    .Query<NotificationConfiguration>(AppSettings.SQLQueryCommand.SP_GET_NotificationConfiguration, _params, commandType: CommandType.StoredProcedure)
                    .ToList();
            }
            return configurations;
        }
    }
}