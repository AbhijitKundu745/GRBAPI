using Dapper;
using PSL.Warehouse.CentralService.DBContext;
//using PSL.Laundry.CentralService.Global;
//using PSL.Laundry.CentralService.IDataAccessLayer;
using PSL.Laundry.CentralService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using AssetMaster = PSL.Warehouse.CentralService.DBContext.AssetMaster;
using PSL.Warehouse.CentralService.Global;
using PSL.Warehouse.CentralService.IDataAccessLayer;
using PSL.Warehouse.CentralService.Models;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Globalization;
using Psl.Chase.Utils;

namespace PSL.Laundry.CentralService.DataAccessLayer
{
    public class PDADAL : IPDADAL
    {
        private LaundryManagementEntities db = new LaundryManagementEntities();
        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        private string PreviousPalletTagID = "GRB020PSL0823SKU0232";


        private string INSERT_SOLineItems = "INSERT INTO SalesOrderLineItems (ID,SOID,SalesDocumentItem,OrderQty,ItemCode,ItemDescription,LoadedQty,RecordStatus,IsActive,IsProcessed,ModifiedDateTime,ServerDateTime)" +
                                                "VALUES(@ID,@SOID_ITEMS,@SalesDocumentItem,@OrderQty,@ItemCode,@ItemDescription,@LoadedQty,@RecordStatus,@IsActive,@IsProcessed,@ModifiedDateTime,@ServerDateTime)";



        public List<AssetInfo> GetAssetInfo(UserData user)
        {
            List<AssetInfo> assetInfos = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@CustomerID", user.CustomerID);
                _params.Add("@ClientDeviceID", user.ClientDeviceID);
                _params.Add("@UserID", user.UserID);
                assetInfos = db.Query<AssetInfo>(AppSettings.SQLQueryCommand.SP_Get_AssetInfo, commandTimeout: 60, commandType: CommandType.StoredProcedure).ToList();
            }
            return assetInfos;
        }

        public List<PalletsDetails> GetPalletDataByID(PalletData pallet)
        {
            List<PalletsDetails> palletsDetails = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@CustomerID", pallet.userDatas.CustomerID);
                _params.Add("@ClientDeviceID", pallet.userDatas.ClientDeviceID);
                _params.Add("@UserID", pallet.userDatas.UserID);
                _params.Add("@PalletID", pallet.PalletID);

                foreach (string i in pallet.Assets)
                {
                    _params.Add("@TagID", i);
                    PalletsDetails pallets = new PalletsDetails();
                    pallets = db.Query<PalletsDetails>(AppSettings.SQLQueryCommand.SP_Get_PalletDetails, commandTimeout: 60, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    palletsDetails.Add(pallets);
                }
            }
            return palletsDetails;
        }

        public List<ContainersDetails> GetContainerDataByID(ContainersData containers)
        {
            List<ContainersDetails> containersDetails = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@CustomerID", containers.UserDatas.CustomerID);
                _params.Add("@ClientDeviceID", containers.UserDatas.ClientDeviceID);
                _params.Add("@UserID", containers.UserDatas.UserID);
                _params.Add("@ContainerID", containers.ContainerID);
                foreach (string i in containers.Pallets)
                {
                    _params.Add("@Pallet", i);
                    ContainersDetails container = new ContainersDetails();
                    container = db.Query<ContainersDetails>(AppSettings.SQLQueryCommand.SP_Get_ContainerDetails, commandTimeout: 60, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    containersDetails.Add(container);
                }
            }
            return containersDetails;
        }

        //        public LoginResponse MobileLogin(LoginModel login)
        //        {

        //            var loginResponse = new LoginResponse();
        //            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(AppSettings.ConnectionString))
        //            {


        //                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand())
        //                {
        //                    cmd.CommandText = AppSettings.SQLQueryCommand.MobileLogin;
        //                    cmd.Parameters.AddWithValue("@userName", login.UserName);
        //                    cmd.Parameters.AddWithValue("@password", login.Password);
        //                    cmd.Parameters.AddWithValue("@ClientDeviceId", login.ClientDeviceID);
        //                    cmd.Connection = conn;
        //                    cmd.CommandType = CommandType.StoredProcedure;

        //                    conn.Open();

        //                    System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(cmd);

        //                    DataSet ds = new DataSet();
        //                    adapter.Fill(ds);
        //                    DataTable tableA = ds.Tables[0];
        //                    DataTable tableB = ds.Tables[1];
        //                    DataTable tablec = ds.Tables[2];
        //                    loginResponse = ds.Tables[0].AsEnumerable()
        //   .Select(dataRow => new LoginResponse
        //   {
        //       UserID = dataRow.Field<string>("UserName"),
        //       TagPassword = /*dataRow.Field<string>("Password")*/ "0",
        //       CompanyCode = dataRow.Field<int>("CompanyCode"),
        //       CustomerID = dataRow.Field<Guid>("CustomerID"),
        //       //UserID = dataRow.Field<Guid>("UserID"),
        //       //ISLogRequired = dataRow.Field<bool>("ISLogRequired")
        //   }).First();

        //                    loginResponse.Dashboard = (ICollection<DashboardData>)ds.Tables[1].AsEnumerable()
        //.Select(dataRow => new DashboardData
        //{
        //    Menu_ID = dataRow.Field<string>("Menu_ID"),
        //    Menu_Name = dataRow.Field<string>("Menu_Name"),
        //    Menu_Image = dataRow.Field<string>("Menu_Image"),
        //    Menu_Is_Active = dataRow.Field<bool>("Menu_Is_Active"),
        //    Menu_Sequence = dataRow.Field<int>("Menu_Sequence"),
        //    Menu_Activity_Name = dataRow.Field<string>("Menu_Activity_Name")


        //}).ToList();

        //                    loginResponse.AssetType = (ICollection<AssetInfo>)ds.Tables[2].AsEnumerable()
        //.Select(dataRow => new AssetInfo
        //{
        //    AssetName = dataRow.Field<string>("AssetName"),
        //    ATypeID = dataRow.Field<int>("ATypeID")
        //}).ToList();

        //                    //                    loginResponse.Room = ds.Tables[3].AsEnumerable()
        //                    //.Select(dataRow => new RoomMasterLogin
        //                    //{
        //                    //    RoomID = dataRow.Field<Guid>("RoomID"),
        //                    //    RoomName = dataRow.Field<string>("RoomNo")

        //                    //}).ToList();

        //                    conn.Close();
        //                }
        //            }
        //            return loginResponse;


        //        }

        /*Warehouse Code*/
        #region Reader
        //public bool InsertTransactionDetails(TransactionDetails transactionDetails)
        //{
        //    bool retval = false;


        //    using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        var _params = new DynamicParameters();

        //        if (transactionDetails.TransID != null && transactionDetails.SubTagDetails != null && transactionDetails.SubTagDetails.Count > 0)
        //        {
        //            _params.Add("@TransID", transactionDetails.TransID);
        //            _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
        //            _params.Add("@PalletTagID", transactionDetails.PalletTagID);
        //            _params.Add("@CategoryID", transactionDetails.CategoryID);
        //            _params.Add("@AntennaID", transactionDetails.AntennaID);
        //            _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
        //            _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
        //            _params.Add("@WorkorderType", transactionDetails.WorkorderType);
        //            _params.Add("@Count", transactionDetails.Count);
        //            _params.Add("@RSSI", transactionDetails.RSSI);
        //            _params.Add("@TouchPointType", transactionDetails.TouchPointType);
        //            _params.Add("@TransDatetime", transactionDetails.TransDatetime);
        //            _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //            db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTransLog, _params, commandType: CommandType.StoredProcedure);
        //            retval = _params.Get<bool>("@Status");
        //            //if (retval)
        //            //{
        //            //    _params = new DynamicParameters();
        //            //    foreach (SubTagData i in transactionDetails.SubTagDetails)
        //            //    {
        //            //        _params.Add("@ClientDeviceID", transactionDetails.DeviceID);
        //            //        _params.Add("@PalletTagID", transactionDetails.PalletTagID);
        //            //        _params.Add("@TagID", i.TagID);
        //            //        _params.Add("@RSSI", i.RSSI);
        //            //        _params.Add("@Count", i.Count);
        //            //        _params.Add("@CategoryID", i.CategoryID);
        //            //        _params.Add("@TagType", i.TagType);
        //            //        _params.Add("@TransDatetime", i.TransDatetime);
        //            //        _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //            //        db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTranslogitems, _params, commandType: CommandType.StoredProcedure);
        //            //        retval = _params.Get<bool>("@Status");
        //            //    }

        //            //}
        //            if (transactionDetails.SubTagDetails != null && transactionDetails.SubTagDetails.Count != 0)
        //            {
        //                foreach (SubTagData i in transactionDetails.SubTagDetails)
        //                {
        //                    WmsPalletBinInfo wmsPalletBin = new WmsPalletBinInfo();
        //                    WmsPalletBinInfoL0 wmsPalletBinInfoL0 = new WmsPalletBinInfoL0();
        //                    WmsPalletBinInfoL1 wmsPalletBinInfoL1 = new WmsPalletBinInfoL1();
        //                    List<WmsPalletBinInfoInternalDetails> wmsPalletBinInfoI0List = new List<WmsPalletBinInfoInternalDetails>();
        //                    WmsPalletBinInfoInternalDetails wmsPalletBinInfoI0 = new WmsPalletBinInfoInternalDetails();
        //                    WmsPalletBinInfoInternal wmsPalletBinInfoInternal = new WmsPalletBinInfoInternal();
        //                    //_params.Add("@PalletID", transactionDetails.PalletID);
        //                    _params.Add("@TransID", transactionDetails.TransID);
        //                    _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
        //                    _params.Add("@PalletTagID", transactionDetails.PalletTagID);
        //                    _params.Add("@CategoryID", transactionDetails.CategoryID);
        //                    _params.Add("@AntennaID", transactionDetails.AntennaID);
        //                    _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
        //                    _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
        //                    _params.Add("@WorkorderType", transactionDetails.WorkorderType);
        //                    _params.Add("@Count", transactionDetails.Count);
        //                    _params.Add("@RSSI", transactionDetails.RSSI);
        //                    _params.Add("@TouchPointType", transactionDetails.TouchPointType);
        //                    _params.Add("@TransDatetime", transactionDetails.TransDatetime);
        //                    _params.Add("@SubTransID", transactionDetails.TransID);
        //                    _params.Add("@SubTagID", i.TagID);
        //                    _params.Add("@SubRSSI", i.RSSI);
        //                    _params.Add("@SubCount", i.Count);
        //                    _params.Add("@SubCategoryID", i.CategoryID);
        //                    _params.Add("@TagType", i.TagType);
        //                    _params.Add("@SubTransDatetime", i.TransDatetime);
        //                    _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTranslogitems, _params, commandType: CommandType.StoredProcedure);
        //                    retval = _params.Get<bool>("@Status");

        //                    if (retval)
        //                    {
        //                        if (transactionDetails.WorkorderType == "U0" || transactionDetails.WorkorderType == "U1")
        //                        {
        //                            wmsPalletBin = GetPalletBinMappedInfo(transactionDetails.PalletTagID, transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, i.TagID);
        //                            wmsPalletBin.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
        //                            wmsPalletBin.palletSensorId = transactionDetails.PalletTagID;
        //                            string jsonData = JsonConvert.SerializeObject(wmsPalletBin);
        //                            var _wmsParams = new DynamicParameters();
        //                            _wmsParams.Add("@ID", transactionDetails.TransID);
        //                            _wmsParams.Add("@DRN", wmsPalletBin.receivingNo);
        //                            _wmsParams.Add("@JSONData", Encoding.UTF8.GetBytes(jsonData));
        //                            _wmsParams.Add("@ProcessType", "Pallet_Movement_U0U1");
        //                            db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                            //PutPalletBinMappedInfo(wmsPalletBin);
        //                        }
        //                        else if (transactionDetails.WorkorderType == "L0")
        //                        {
        //                            wmsPalletBinInfoL0 = GetPalletBinMappedInfoL0(transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, transactionDetails.PalletTagID, i.TagID);
        //                            string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoL0);
        //                            var _wmsParams = new DynamicParameters();
        //                            _wmsParams.Add("@ID", transactionDetails.TransID);
        //                            _wmsParams.Add("@DRN", wmsPalletBinInfoL0.dispatchNo);
        //                            _wmsParams.Add("@JSONData", Encoding.UTF8.GetBytes(jsonData));
        //                            _wmsParams.Add("@ProcessType", "Pallet_Movement_L0");
        //                            db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                            //PutPalletBinMappedInfoL0(wmsPalletBinInfoL0);
        //                        }
        //                        else if (transactionDetails.WorkorderType == "L1")
        //                        {
        //                            wmsPalletBinInfoL1 = GetPalletBinMappedInfoL1(transactionDetails.PalletTagID, transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, i.TagID);
        //                            wmsPalletBinInfoL1.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
        //                            wmsPalletBinInfoL1.palletSensorId = transactionDetails.PalletTagID;
        //                            string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoL1);
        //                            var query = "SELECT ProcessInfo1 FROM WorkOrder WHERE WONumber = @WorkOrderNumber";
        //                            var dispatchNo = db.Query<string>(query, new { WorkorderNumber = transactionDetails.WorkorderNumber }).FirstOrDefault();
        //                            var _wmsParams = new DynamicParameters();
        //                            _wmsParams.Add("@ID", transactionDetails.TransID);
        //                            _wmsParams.Add("@DRN", dispatchNo);
        //                            _wmsParams.Add("@JSONData", Encoding.UTF8.GetBytes(jsonData));
        //                            _wmsParams.Add("@ProcessType", "Pallet_Movement_L1");
        //                            db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                            //PutPalletBinMappedInfoL1(wmsPalletBinInfoL1);
        //                        }
        //                        else if (transactionDetails.WorkorderType == "I0")
        //                        {

        //                            wmsPalletBinInfoI0 = GetPalletBinMappedInfoI0(transactionDetails.PalletTagID, transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, i.TagID);
        //                            wmsPalletBinInfoI0.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
        //                            wmsPalletBinInfoI0List.Add(wmsPalletBinInfoI0);
        //                            wmsPalletBinInfoInternal.data = wmsPalletBinInfoI0List;
        //                            string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoInternal);
        //                            var _wmsParams = new DynamicParameters();
        //                            _wmsParams.Add("@ID", transactionDetails.TransID);
        //                            _wmsParams.Add("@DRN", "Internal Movement");
        //                            _wmsParams.Add("@JSONData", Encoding.UTF8.GetBytes(jsonData));
        //                            _wmsParams.Add("@ProcessType", "Pallet_Movement_I0");
        //                            db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                //Should never be here..
        //            }

        //        }
        //        else if (transactionDetails.TransID != null && transactionDetails.SubTagDetails != null && transactionDetails.SubTagDetails.Count == 0)
        //        {
        //            //Posting Parent if Subitems count is zero
        //            _params.Add("@TransID", transactionDetails.TransID);
        //            _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
        //            _params.Add("@PalletTagID", transactionDetails.PalletTagID);
        //            _params.Add("@CategoryID", transactionDetails.CategoryID);
        //            _params.Add("@AntennaID", transactionDetails.AntennaID);
        //            _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
        //            _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
        //            _params.Add("@WorkorderType", transactionDetails.WorkorderType);
        //            _params.Add("@Count", transactionDetails.Count);
        //            _params.Add("@RSSI", transactionDetails.RSSI);
        //            _params.Add("@TouchPointType", transactionDetails.TouchPointType);
        //            _params.Add("@TransDatetime", transactionDetails.TransDatetime);
        //            _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //            db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTransLog, _params, commandType: CommandType.StoredProcedure);
        //            retval = _params.Get<bool>("@Status");

        //            //retval = true;
        //        }
        //        else if (transactionDetails.TransID != null && transactionDetails.SubTagDetails == null)
        //        {
        //            //Posting Parent if Subitems come as null...
        //            _params.Add("@TransID", transactionDetails.TransID);
        //            _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
        //            _params.Add("@PalletTagID", transactionDetails.PalletTagID);
        //            _params.Add("@CategoryID", transactionDetails.CategoryID);
        //            _params.Add("@AntennaID", transactionDetails.AntennaID);
        //            _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
        //            _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
        //            _params.Add("@WorkorderType", transactionDetails.WorkorderType);
        //            _params.Add("@Count", transactionDetails.Count);
        //            _params.Add("@RSSI", transactionDetails.RSSI);
        //            _params.Add("@TouchPointType", transactionDetails.TouchPointType);
        //            _params.Add("@TransDatetime", transactionDetails.TransDatetime);
        //            _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //            db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTransLog, _params, commandType: CommandType.StoredProcedure);
        //            retval = _params.Get<bool>("@Status");

        //            //retval = true;
        //        }
        //    }
        //    return retval;
        //}
        public bool InsertTransactionDetails(TransactionDetails transactionDetails)
        {
            bool retval = false;


            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();

                if (transactionDetails.TransID != null && transactionDetails.SubTagDetails != null && transactionDetails.SubTagDetails.Count > 0)
                {
                    _params.Add("@TransID", transactionDetails.TransID);
                    _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
                    _params.Add("@PalletTagID", transactionDetails.PalletTagID);
                    _params.Add("@CategoryID", transactionDetails.CategoryID);
                    _params.Add("@AntennaID", transactionDetails.AntennaID);
                    _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
                    _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
                    _params.Add("@WorkorderType", transactionDetails.WorkorderType);
                    _params.Add("@Count", transactionDetails.Count);
                    _params.Add("@RSSI", transactionDetails.RSSI);
                    _params.Add("@TouchPointType", transactionDetails.TouchPointType);
                    _params.Add("@TransDatetime", transactionDetails.TransDatetime);
                    _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTransLog, _params, commandType: CommandType.StoredProcedure);
                    retval = _params.Get<bool>("@Status");
                    //if (retval)
                    //{
                    //    _params = new DynamicParameters();
                    //    foreach (SubTagData i in transactionDetails.SubTagDetails)
                    //    {
                    //        _params.Add("@ClientDeviceID", transactionDetails.DeviceID);
                    //        _params.Add("@PalletTagID", transactionDetails.PalletTagID);
                    //        _params.Add("@TagID", i.TagID);
                    //        _params.Add("@RSSI", i.RSSI);
                    //        _params.Add("@Count", i.Count);
                    //        _params.Add("@CategoryID", i.CategoryID);
                    //        _params.Add("@TagType", i.TagType);
                    //        _params.Add("@TransDatetime", i.TransDatetime);
                    //        _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    //        db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTranslogitems, _params, commandType: CommandType.StoredProcedure);
                    //        retval = _params.Get<bool>("@Status");
                    //    }

                    //}
                    if (transactionDetails.SubTagDetails != null && transactionDetails.SubTagDetails.Count != 0)
                    {
                        foreach (SubTagData i in transactionDetails.SubTagDetails)
                        {
                            WmsPalletBinInfo wmsPalletBin = new WmsPalletBinInfo();
                            WmsPalletBinInfoL0 wmsPalletBinInfoL0 = new WmsPalletBinInfoL0();
                            WmsPalletBinInfoL1 wmsPalletBinInfoL1 = new WmsPalletBinInfoL1();
                            List<WmsPalletBinInfoInternalDetails> wmsPalletBinInfoI0List = new List<WmsPalletBinInfoInternalDetails>();
                            WmsPalletBinInfoInternalDetails wmsPalletBinInfoI0 = new WmsPalletBinInfoInternalDetails();
                            WmsPalletBinInfoInternal wmsPalletBinInfoInternal = new WmsPalletBinInfoInternal();
                            //_params.Add("@PalletID", transactionDetails.PalletID);
                            _params.Add("@TransID", transactionDetails.TransID);
                            _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
                            _params.Add("@PalletTagID", transactionDetails.PalletTagID);
                            _params.Add("@CategoryID", transactionDetails.CategoryID);
                            _params.Add("@AntennaID", transactionDetails.AntennaID);
                            _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
                            _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
                            _params.Add("@WorkorderType", transactionDetails.WorkorderType);
                            _params.Add("@Count", transactionDetails.Count);
                            _params.Add("@RSSI", transactionDetails.RSSI);
                            _params.Add("@TouchPointType", transactionDetails.TouchPointType);
                            _params.Add("@TransDatetime", transactionDetails.TransDatetime);
                            _params.Add("@SubTransID", transactionDetails.TransID);
                            _params.Add("@SubTagID", i.TagID);
                            _params.Add("@SubRSSI", i.RSSI);
                            _params.Add("@SubCount", i.Count);
                            _params.Add("@SubCategoryID", i.CategoryID);
                            _params.Add("@TagType", i.TagType);
                            _params.Add("@SubTransDatetime", i.TransDatetime);
                            _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                            db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTranslogitems, _params, commandType: CommandType.StoredProcedure);
                            retval = _params.Get<bool>("@Status");

                            if (retval)
                            {
                                if (transactionDetails.WorkorderType == "U0" || transactionDetails.WorkorderType == "U1")
                                {
                                    wmsPalletBin = GetPalletBinMappedInfo(transactionDetails.PalletTagID, transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, i.TagID);
                                    wmsPalletBin.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
                                    wmsPalletBin.palletSensorId = transactionDetails.PalletTagID;
                                    string jsonData = JsonConvert.SerializeObject(wmsPalletBin);
                                    var _wmsParams = new DynamicParameters();
                                    _wmsParams.Add("@ID", transactionDetails.TransID);
                                    _wmsParams.Add("@Type", "Pallet_Movement_U0U1");
                                    _wmsParams.Add("@receivingNo", wmsPalletBin.receivingNo);
                                    _wmsParams.Add("@dispatchNo", null);
                                    _wmsParams.Add("@warehouseId", wmsPalletBin.warehouseId);
                                    _wmsParams.Add("@palletName", wmsPalletBin.palletName);
                                    _wmsParams.Add("@palletSensorId", wmsPalletBin.palletSensorId);
                                    _wmsParams.Add("@binName", null);
                                    _wmsParams.Add("@fromBinName", wmsPalletBin.fromBinName);
                                    _wmsParams.Add("@fromPalletName", null);
                                    _wmsParams.Add("@toPalletName", null);
                                    _wmsParams.Add("@toBinName", wmsPalletBin.toBinName);
                                    _wmsParams.Add("@comment", null);
                                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                                }
                                else if (transactionDetails.WorkorderType == "L0")
                                {
                                    wmsPalletBinInfoL0 = GetPalletBinMappedInfoL0(transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, transactionDetails.PalletTagID, i.TagID);
                                    string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoL0);
                                    var _wmsParams = new DynamicParameters();
                                    _wmsParams.Add("@ID", transactionDetails.TransID);
                                    _wmsParams.Add("@Type", "Pallet_Movement_L0");
                                    _wmsParams.Add("@receivingNo", null);
                                    _wmsParams.Add("@dispatchNo", wmsPalletBinInfoL0.dispatchNo);
                                    _wmsParams.Add("@warehouseId", null);
                                    _wmsParams.Add("@palletName", null);
                                    _wmsParams.Add("@palletSensorId", transactionDetails.PalletTagID);
                                    _wmsParams.Add("@binName", null);
                                    _wmsParams.Add("@fromBinName", null);
                                    _wmsParams.Add("@fromPalletName", null);
                                    _wmsParams.Add("@toPalletName", wmsPalletBinInfoL0.toPalletName);
                                    _wmsParams.Add("@toBinName", wmsPalletBinInfoL0.toBinName);
                                    _wmsParams.Add("@comment", null);
                                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                                    foreach (var wmsData in wmsPalletBinInfoL0.items)
                                    {
                                        var _wmsParamsDetails = new DynamicParameters();
                                        _wmsParamsDetails.Add("@ParentID", transactionDetails.TransID);
                                        _wmsParamsDetails.Add("@Type", "Pallet_Movement_L0");
                                        _wmsParamsDetails.Add("@fromBinName", wmsData.fromBinName);
                                        _wmsParamsDetails.Add("@fromPalletName", wmsData.fromPalletName);
                                        _wmsParamsDetails.Add("@stockBinId", wmsData.stockBinId);
                                        _wmsParamsDetails.Add("@serialNumber", null);
                                        _wmsParamsDetails.Add("@skuCode", wmsData.skuCode);
                                        _wmsParamsDetails.Add("@batchNumber", wmsData.batchNumber);
                                        _wmsParamsDetails.Add("@batchDateTime", null);
                                        _wmsParamsDetails.Add("@status", null);
                                        _wmsParamsDetails.Add("@qty", wmsData.qty);
                                        db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataChild, _wmsParamsDetails, commandType: CommandType.StoredProcedure);
                                    }
                                }
                                else if (transactionDetails.WorkorderType == "L1")
                                {
                                    wmsPalletBinInfoL1 = GetPalletBinMappedInfoL1(transactionDetails.PalletTagID, transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, i.TagID);
                                    wmsPalletBinInfoL1.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
                                    wmsPalletBinInfoL1.palletSensorId = transactionDetails.PalletTagID;
                                    string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoL1);
                                    var query = "SELECT ProcessInfo1 FROM WorkOrder WHERE WONumber = @WorkOrderNumber";
                                    var dispatchNo = db.Query<string>(query, new { WorkorderNumber = transactionDetails.WorkorderNumber }).FirstOrDefault();
                                    var _wmsParams = new DynamicParameters();
                                    _wmsParams.Add("@ID", transactionDetails.TransID);
                                    _wmsParams.Add("@Type", "Pallet_Movement_L1");
                                    _wmsParams.Add("@receivingNo", null);
                                    _wmsParams.Add("@dispatchNo", dispatchNo);
                                    _wmsParams.Add("@warehouseId", wmsPalletBinInfoL1.warehouseId);
                                    _wmsParams.Add("@palletName", wmsPalletBinInfoL1.palletName);
                                    _wmsParams.Add("@palletSensorId", wmsPalletBinInfoL1.palletSensorId);
                                    _wmsParams.Add("@binName", null);
                                    _wmsParams.Add("@fromBinName", wmsPalletBinInfoL1.fromBinName);
                                    _wmsParams.Add("@fromPalletName", null);
                                    _wmsParams.Add("@toPalletName", null);
                                    _wmsParams.Add("@toBinName", wmsPalletBinInfoL1.toBinName);
                                    _wmsParams.Add("@comment", null);
                                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                                }
                                else if (transactionDetails.WorkorderType == "I0")
                                {

                                    wmsPalletBinInfoI0 = GetPalletBinMappedInfoI0(transactionDetails.PalletTagID, transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, i.TagID);
                                    wmsPalletBinInfoI0.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
                                    wmsPalletBinInfoI0List.Add(wmsPalletBinInfoI0);
                                    wmsPalletBinInfoInternal.data = wmsPalletBinInfoI0List;
                                    string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoInternal);
                                    var _wmsParams = new DynamicParameters();
                                    _wmsParams.Add("@ID", transactionDetails.TransID);
                                    _wmsParams.Add("@Type", "Pallet_Movement_I0");
                                    _wmsParams.Add("@receivingNo", null);
                                    _wmsParams.Add("@dispatchNo", null);
                                    _wmsParams.Add("@warehouseId", wmsPalletBinInfoI0.warehouseId);
                                    _wmsParams.Add("@palletName", null);
                                    _wmsParams.Add("@palletSensorId", null);
                                    _wmsParams.Add("@binName", null);
                                    _wmsParams.Add("@fromBinName", wmsPalletBinInfoI0.fromBinName);
                                    _wmsParams.Add("@fromPalletName", wmsPalletBinInfoI0.fromPalletName);
                                    _wmsParams.Add("@toPalletName", wmsPalletBinInfoI0.toPalletName);
                                    _wmsParams.Add("@toBinName", wmsPalletBinInfoI0.toBinName);
                                    _wmsParams.Add("@comment", null);
                                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                                }
                            }
                        }
                    }
                    else
                    {
                        //Should never be here..
                    }

                }
                else if (transactionDetails.TransID != null && transactionDetails.SubTagDetails != null && transactionDetails.SubTagDetails.Count == 0)
                {
                    //Posting Parent if Subitems count is zero
                    _params.Add("@TransID", transactionDetails.TransID);
                    _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
                    _params.Add("@PalletTagID", transactionDetails.PalletTagID);
                    _params.Add("@CategoryID", transactionDetails.CategoryID);
                    _params.Add("@AntennaID", transactionDetails.AntennaID);
                    _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
                    _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
                    _params.Add("@WorkorderType", transactionDetails.WorkorderType);
                    _params.Add("@Count", transactionDetails.Count);
                    _params.Add("@RSSI", transactionDetails.RSSI);
                    _params.Add("@TouchPointType", transactionDetails.TouchPointType);
                    _params.Add("@TransDatetime", transactionDetails.TransDatetime);
                    _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTransLog, _params, commandType: CommandType.StoredProcedure);
                    retval = _params.Get<bool>("@Status");

                    //retval = true;
                }
                else if (transactionDetails.TransID != null && transactionDetails.SubTagDetails == null)
                {
                    //Posting Parent if Subitems come as null...
                    _params.Add("@TransID", transactionDetails.TransID);
                    _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
                    _params.Add("@PalletTagID", transactionDetails.PalletTagID);
                    _params.Add("@CategoryID", transactionDetails.CategoryID);
                    _params.Add("@AntennaID", transactionDetails.AntennaID);
                    _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
                    _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
                    _params.Add("@WorkorderType", transactionDetails.WorkorderType);
                    _params.Add("@Count", transactionDetails.Count);
                    _params.Add("@RSSI", transactionDetails.RSSI);
                    _params.Add("@TouchPointType", transactionDetails.TouchPointType);
                    _params.Add("@TransDatetime", transactionDetails.TransDatetime);
                    _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTransLog, _params, commandType: CommandType.StoredProcedure);
                    retval = _params.Get<bool>("@Status");

                    //retval = true;
                }
            }
            return retval;
        }
        #region ReaderWithBinMaster
        //public Response InsertTransactionDetails(TransactionDetails transactionDetails)
        //{
        //    Response responseData = new Response();
        //    bool retval = false;
        //    bool retval1 = false;
        //    string message = string.Empty;
        //    WmsPalletBinInfo wmsPalletBin = new WmsPalletBinInfo();
        //    WmsPalletBinInfoL0 wmsPalletBinInfoL0 = new WmsPalletBinInfoL0();
        //    WmsPalletBinInfoL1 wmsPalletBinInfoL1 = new WmsPalletBinInfoL1();
        //    List<WmsPalletBinInfoInternalDetails> wmsPalletBinInfoI0List = new List<WmsPalletBinInfoInternalDetails>();
        //    WmsPalletBinInfoInternalDetails wmsPalletBinInfoI0 = new WmsPalletBinInfoInternalDetails();
        //    WmsPalletBinInfoInternal wmsPalletBinInfoInternal = new WmsPalletBinInfoInternal();


        //    using (SqlConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        using (SqlTransaction transaction = db.BeginTransaction())
        //        {
        //            try
        //            {
        //                var _params = new DynamicParameters();

        //                if (transactionDetails.TransID != null && transactionDetails.SubTagDetails != null && transactionDetails.SubTagDetails.Count > 0)
        //                {
        //                    _params.Add("@TransID", transactionDetails.TransID);
        //                    _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
        //                    _params.Add("@PalletTagID", transactionDetails.PalletTagID);
        //                    _params.Add("@CategoryID", transactionDetails.CategoryID);
        //                    _params.Add("@AntennaID", transactionDetails.AntennaID);
        //                    _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
        //                    _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
        //                    _params.Add("@WorkorderType", transactionDetails.WorkorderType);
        //                    _params.Add("@Count", transactionDetails.Count);
        //                    _params.Add("@RSSI", transactionDetails.RSSI);
        //                    _params.Add("@TouchPointType", transactionDetails.TouchPointType);
        //                    _params.Add("@TransDatetime", transactionDetails.TransDatetime);
        //                    _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTransLog, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
        //                    retval = _params.Get<bool>("@Status");

        //                    if (transactionDetails.SubTagDetails != null && transactionDetails.SubTagDetails.Count != 0)
        //                    {
        //                        foreach (SubTagData i in transactionDetails.SubTagDetails)
        //                        {

        //                            //_params.Add("@PalletID", transactionDetails.PalletID);
        //                            _params.Add("@TransID", transactionDetails.TransID);
        //                            _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
        //                            _params.Add("@PalletTagID", transactionDetails.PalletTagID);
        //                            _params.Add("@CategoryID", transactionDetails.CategoryID);
        //                            _params.Add("@AntennaID", transactionDetails.AntennaID);
        //                            _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
        //                            _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
        //                            _params.Add("@WorkorderType", transactionDetails.WorkorderType);
        //                            _params.Add("@Count", transactionDetails.Count);
        //                            _params.Add("@RSSI", transactionDetails.RSSI);
        //                            _params.Add("@TouchPointType", transactionDetails.TouchPointType);
        //                            _params.Add("@TransDatetime", transactionDetails.TransDatetime);
        //                            _params.Add("@SubTransID", transactionDetails.TransID);
        //                            _params.Add("@SubTagID", i.TagID);
        //                            _params.Add("@SubRSSI", i.RSSI);
        //                            _params.Add("@SubCount", i.Count);
        //                            _params.Add("@SubCategoryID", i.CategoryID);
        //                            _params.Add("@TagType", i.TagType);
        //                            _params.Add("@SubTransDatetime", i.TransDatetime);
        //                            _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //                            _params.Add("@Message", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);
        //                            db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTranslogitems, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
        //                            retval1 = _params.Get<bool>("@Status");
        //                            message = _params.Get<string>("@Message");


        //                            if (retval && retval1)
        //                            {
        //                                transaction.Commit();
        //                                if (transactionDetails.WorkorderType == "U0" || transactionDetails.WorkorderType == "U1")
        //                                {
        //                                    wmsPalletBin = GetPalletBinMappedInfo(transactionDetails.PalletTagID, transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, i.TagID);
        //                                    wmsPalletBin.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
        //                                    wmsPalletBin.palletSensorId = transactionDetails.PalletTagID;
        //                                    string jsonData = JsonConvert.SerializeObject(wmsPalletBin);
        //                                    var _wmsParams = new DynamicParameters();
        //                                    _wmsParams.Add("@DRN", wmsPalletBin.receivingNo);
        //                                    _wmsParams.Add("@JSONData", jsonData);
        //                                    _wmsParams.Add("@ProcessType", "Pallet_Movement_U0U1");
        //                                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                                    //PutPalletBinMappedInfo(wmsPalletBin);
        //                                }
        //                                else if (transactionDetails.WorkorderType == "L0")
        //                                {
        //                                    wmsPalletBinInfoL0 = GetPalletBinMappedInfoL0(transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, transactionDetails.PalletTagID, i.TagID);
        //                                    string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoL0);
        //                                    var _wmsParams = new DynamicParameters();
        //                                    _wmsParams.Add("@DRN", wmsPalletBinInfoL0.dispatchNo);
        //                                    _wmsParams.Add("@JSONData", jsonData);
        //                                    _wmsParams.Add("@ProcessType", "Pallet_Movement_L0");
        //                                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                                    //PutPalletBinMappedInfoL0(wmsPalletBinInfoL0);
        //                                }
        //                                else if (transactionDetails.WorkorderType == "L1")
        //                                {
        //                                    wmsPalletBinInfoL1 = GetPalletBinMappedInfoL1(transactionDetails.PalletTagID, transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, i.TagID);
        //                                    wmsPalletBinInfoL1.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
        //                                    wmsPalletBinInfoL1.palletSensorId = transactionDetails.PalletTagID;
        //                                    string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoL1);
        //                                    var query = "SELECT ProcessInfo1 FROM WorkOrder WHERE WONumber = @WorkOrderNumber";
        //                                    var dispatchNo = db.Query<string>(query, new { WorkorderNumber = transactionDetails.WorkorderNumber }).FirstOrDefault();
        //                                    var _wmsParams = new DynamicParameters();
        //                                    _wmsParams.Add("@DRN", dispatchNo);
        //                                    _wmsParams.Add("@JSONData", jsonData);
        //                                    _wmsParams.Add("@ProcessType", "Pallet_Movement_L1");
        //                                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                                    //PutPalletBinMappedInfoL1(wmsPalletBinInfoL1);
        //                                }
        //                                else if (transactionDetails.WorkorderType == "I0")
        //                                {

        //                                    wmsPalletBinInfoI0 = GetPalletBinMappedInfoI0(transactionDetails.PalletTagID, transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, i.TagID);
        //                                    wmsPalletBinInfoI0.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
        //                                    wmsPalletBinInfoI0List.Add(wmsPalletBinInfoI0);
        //                                    wmsPalletBinInfoInternal.data = wmsPalletBinInfoI0List;
        //                                    string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoInternal);
        //                                    var _wmsParams = new DynamicParameters();
        //                                    _wmsParams.Add("@DRN", "Internal Movement");
        //                                    _wmsParams.Add("@JSONData", jsonData);
        //                                    _wmsParams.Add("@ProcessType", "Pallet_Movement_I0");
        //                                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                                }
        //                                responseData.status = retval1;
        //                                responseData.message = message;
        //                            }
        //                            else
        //                            {
        //                                transaction.Rollback();
        //                                responseData.status = retval1;
        //                                responseData.message = message;
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //Should never be here..
        //                    }

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                responseData.status = false;
        //                responseData.message = ex.Message;
        //            }
        //        }
        //    }
        //    return responseData;
        //}
        #endregion
        public bool InsertTagLoggerDetails(TagDetailsRequest transactionDetails)
        {
            bool retval = false;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();

                {
                    foreach (TagLoggerDetails i in transactionDetails.TagDetails)
                    {
                        _params.Add("@PalletTagID", i.PalletTagID);
                        _params.Add("@RSSI", i.RSSI);
                        _params.Add("@Count", i.Count);
                        _params.Add("@DeviceID", i.ClientDeviceID);
                        _params.Add("@AntennaID", i.AntennaID);
                        _params.Add("@TransDatetime", i.TransDatetime);
                        _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                        db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTransLog1, _params, commandType: CommandType.StoredProcedure);
                        retval = _params.Get<bool>("@Status");
                        //retval = true;
                    }
                }
            }
            return retval;
        }

        //public ReaderWorkorderListResponse GetReaderWorkorderList(ReaderWorkorderListRequest readerWorkorderList)
        //{
        //    ReaderWorkorderListResponse responseData = null;
        //    using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        var _params = new DynamicParameters();
        //        _params.Add("@clientDeviceID", readerWorkorderList.ClientDeviceID);
        //        _params.Add("@ReaderStatus", readerWorkorderList.ReaderStatus);
        //        //data = db.Query<ReaderWorkorderListResponse>(AppSettings.SQLQueryCommand.SP_GetReaderWorkorderList, _params, commandType: CommandType.StoredProcedure).FirstOrDefault();
        //        //isValid = detail != null;
        //        using (var result = db.QueryMultiple(AppSettings.SQLQueryCommand.SP_GetReaderWorkorderList, _params, commandType: CommandType.StoredProcedure))
        //        {
        //            responseData = result.Read<ReaderWorkorderListResponse>().FirstOrDefault();
        //            responseData.data = result.Read<OrderDetails>().ToList();
        //        }
        //    }
        //    return responseData;
        //}

        //public bool UploadCurrentPallet(CurrentPalletRequest currentPallet)
        //{
        //    bool data = false;
        //    using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        var _params = new DynamicParameters();
        //        _params.Add("@ClientDeviceID", currentPallet.ClientDeviceID);
        //        _params.Add("@WorkorderNumber", currentPallet.WorkorderNumber);
        //        _params.Add("@CurrentScannedPalletTagID", currentPallet.CurrentScannedPalletTagID);
        //        //_params.Add("@CurrentScannedPalletName", currentPallet.CurrentScannedPalletName);
        //        var data1 = db.Query<CurrentPalletRequest>(AppSettings.SQLQueryCommand.SP_UploadCurrentPallet, _params, commandType: CommandType.StoredProcedure).FirstOrDefault();
        //        data = data1 != null;
        //    }
        //    return data;
        //}
        public ReaderWorkorderListResponse GetReaderWorkorderList(ReaderWorkorderListRequest readerWorkorderList)
        {
            ReaderWorkorderListResponse responseData = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@clientDeviceID", readerWorkorderList.ClientDeviceID);
                _params.Add("@ReaderStatus", readerWorkorderList.ReaderStatus);
                using (var result = db.QueryMultiple(AppSettings.SQLQueryCommand.SP_GetReaderWorkorderList, _params, commandType: CommandType.StoredProcedure))
                {
                    responseData = result.Read<ReaderWorkorderListResponse>().FirstOrDefault();
                    responseData.data = result.Read<OrderDetails>().ToList();
                }
            }
            return responseData;
        }
        public bool UploadCurrentPalletV1(CurrentPalletRequest currentPallet)
        {
            bool data = false;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@ClientDeviceID", currentPallet.ClientDeviceID);
                _params.Add("@CurrentScannedPalletTagID", currentPallet.CurrentScannedPalletTagID);
                var data1 = db.Execute(AppSettings.SQLQueryCommand.SP_UploadCurrentPallet, _params, commandType: CommandType.StoredProcedure);
                data = data1 != null;
            }
            return data;
        }
        public bool UploadCurrentBin(CurrentBinRequest currentBin)
        {
            bool isInserted = false;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@ClientDeviceID", currentBin.ClientDeviceID);
                _params.Add("@CurrentScannedBinTagID", currentBin.CurrentScannedBinTagID);
                db.Execute(AppSettings.SQLQueryCommand.SP_UploadCurrentBin, _params, commandType: CommandType.StoredProcedure);
                isInserted = _params.Get<bool>("@Status");
            }
            return isInserted;
        }

        #endregion

        public List<AssetMaster> GetWorkOrderDetails(string  DeviceID)
        {
            List<AssetMaster> data = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@DeviceID", DeviceID);
                data = db
                    .Query<AssetMaster>(AppSettings.SQLQueryCommand.SP_GetAllRCOAssets, _params, commandType: CommandType.StoredProcedure).ToList();
            }
            return data;
        }

        #region MobileDisplay

        public bool CheckCredentials(LoginRequest request)
        {
            bool isValid = false;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@userName", request.UserName);
                _params.Add("@password", request.Password);
                _params.Add("@DeviceId", request.ClientDeviceID);
                var detail = db.Query<LoginRequest>(AppSettings.SQLQueryCommand.TabLogin, _params, commandType: CommandType.StoredProcedure).FirstOrDefault();
                isValid = detail != null;
            }
            return isValid;
        }

        public ReaderStatusinfo GetReaderStatus(DeviceData deviceData)
        {
            ReaderStatusinfo data = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@clientDeviceID", deviceData.ClientDeviceID);
                data = db.Query<ReaderStatusinfo>(AppSettings.SQLQueryCommand.SP_GetStatusDetails, _params, commandType: CommandType.StoredProcedure).FirstOrDefault();
                //isValid = detail != null;
                //db.Close();
            }
            return data;
        }

        public List<Workorderlist> GetWorkorderlists(DeviceData deviceData)
        {
            List<Workorderlist> data = new List<Workorderlist>();
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@clientDeviceID", deviceData.ClientDeviceID);
                data = db.Query<Workorderlist>(AppSettings.SQLQueryCommand.SP_GetWorkorderLists, _params, commandType: CommandType.StoredProcedure).ToList();
                //isValid = detail != null;
            }
            return data;
        }

        public WorkorderStatusResponse UpdateWorkorderStatus(WorkorderStatusRequest workorderStatus)
        {
            WorkorderStatusResponse data = new WorkorderStatusResponse();
            //List<OrderDetails> details = null;

            DeviceData deviceData = new DeviceData { ClientDeviceID = workorderStatus.ClientDeviceID };
            ReaderStatusinfo readerStatusinfo = GetReaderStatus(deviceData);

            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@clientDeviceID", workorderStatus.ClientDeviceID);
                _params.Add("@WorkorderNumber", workorderStatus.WorkorderNumber);
                _params.Add("@WorkorderStatus", workorderStatus.WorkorderStatus);

                //data = db.Query<WorkorderStatusResponse>(AppSettings.SQLQueryCommand.SP_UpdateWorkorderStatus, _params, commandType: CommandType.StoredProcedure).ToList();
                using (var result = db.QueryMultiple(AppSettings.SQLQueryCommand.SP_UpdateWorkorderStatus, _params, commandType: CommandType.StoredProcedure))
                {

                    //data = result.Read<WorkorderStatusResponse>().ToList();
                    var data1 = result.Read<WorkorderStatusResponse>().ToList();
                    if (data1.Count > 0)
                    {
                        var firstData = data1[0];
                        firstData.PollingTimer = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PollingTimer"]);
                        firstData.ReaderStatus = readerStatusinfo.ReaderStatus;
                        firstData.ApplicationStatus = readerStatusinfo.ApplicationStatus;

                        var orderList = result.Read<OrderDetails>().ToList();
                        firstData.OrderDetails = orderList;

                        data = firstData;
                    }
                    //data[0].PollingTimer = System.Configuration.ConfigurationManager.AppSettings["PollingTimer"];
                    //data[0].ReaderStatus = readerStatusinfo.ReaderStatus;
                    //data[0].ApplicationStatus = readerStatusinfo.ApplicationStatus;
                    //data[0].OrderDetails = result.Read<OrderDetails>().ToList();
                }
                //isValid = detail != null;
            }
            return data;
        }
        //public WorkorderListResponse GetWorkorderListItems(WorkorderListRequest workorderItems)
        //{
        //    WorkorderListResponse data = new WorkorderListResponse();

        //    DeviceData deviceData = new DeviceData { ClientDeviceID = workorderItems.ClientDeviceID };
        //    ReaderStatusinfo readerStatusinfo = GetReaderStatus(deviceData);

        //    using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        var _params = new DynamicParameters();

        //        _params.Add("@clientDeviceID", workorderItems.ClientDeviceID);
        //        _params.Add("@WorkorderNumber", workorderItems.WorkorderNumber);
        //        _params.Add("@WorkorderType", workorderItems.WorkorderType);

        //        using (var result = db.QueryMultiple(AppSettings.SQLQueryCommand.SP_GetWorkorderListItems, _params, commandType: CommandType.StoredProcedure))
        //        {
        //            var data1 = result.Read<WorkorderListResponse>().ToList();
        //            if (data1.Count > 0)
        //            {
        //                var firstData = data1[0];
        //                firstData.PollingTimer = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PollingTimer"]);
        //                firstData.ReaderStatus = readerStatusinfo.ReaderStatus;
        //                firstData.ApplicationStatus = readerStatusinfo.ApplicationStatus;

        //                var orderList = result.Read<OrderDetails>().ToList();
        //                firstData.OrderDetails = orderList;

        //                data = firstData;
        //            }
        //            //data = result.Read<WorkorderListResponse>().ToList();
        //            //data[0].PollingTimer = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PollingTimer"]);
        //            //data[0].ReaderStatus = readerStatusinfo.ReaderStatus;
        //            //data[0].ApplicationStatus = readerStatusinfo.ApplicationStatus;
        //            //data[0].OrderDetails = result.Read<OrderDetails>().ToList();
        //        }
        //        //isValid = detail != null;
        //    }
        //    return data;

        //}
        public WorkorderListResponse GetWorkorderListItemsV1(WorkorderListRequest workorderItems)
        {
            WorkorderListResponse data = new WorkorderListResponse();
            

            DeviceData deviceData = new DeviceData { ClientDeviceID = workorderItems.ClientDeviceID };
            ReaderStatusinfo readerStatusinfo = GetReaderStatus(deviceData);

            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();

                _params.Add("@clientDeviceID", workorderItems.ClientDeviceID);
                List<OrderDetailsV1> orderDetails = new List<OrderDetailsV1>();
                orderDetails = db.Query<OrderDetailsV1>(AppSettings.SQLQueryCommand.SP_GetWorkorderListItems, _params, commandType: CommandType.StoredProcedure).ToList();
                data.PollingTimer = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PollingTimer"]);
                data.ReaderStatus = readerStatusinfo.ReaderStatus;
                data.ApplicationStatus = readerStatusinfo.ApplicationStatus;
                data.OrderDetails = orderDetails;
            }
            return data;

        }
        public bool StartReader(ReaderStartRequset startRequset)
        {
            bool data = false;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@ClientDeviceID", startRequset.ClientDeviceID);
                _params.Add("@ReaderStatus", startRequset.ReaderStatus);
                var data1 = db.Execute(AppSettings.SQLQueryCommand.SP_StartReader, _params, commandType: CommandType.StoredProcedure);
                data = data1 != null;
            }
            return data;
        }
        #endregion

        #region WebApp
        public bool WebLogin(UserModel user)
        {
            bool retVal = false;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@UserName", user.UserName);
                _params.Add("@Password", user.Password);
                _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                db.Execute(AppSettings.SQLQueryCommand.SP_WebLogin, _params, commandType: CommandType.StoredProcedure);
                retVal = _params.Get<bool>("@Status");
            }
            return retVal;
        }

        public List<TruckDetails> GetTruckDetails()
        {
            List<TruckDetails> data = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                data = db.Query<TruckDetails>(AppSettings.SQLQueryCommand.SP_GetTruckDetails, _params, commandType: CommandType.StoredProcedure).ToList();

            }
            return data;
            //return null;
        }

        public List<PalletItemDetails> GetPalletWithItemDetails(TruckDetails truckDetails)
        {
            List<PalletItemDetails> data = new List<PalletItemDetails>();

            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();

                var _params = new DynamicParameters();
                _params.Add("@TruckID", truckDetails.TruckNumber);
                var pallets = db.Query<PalletItemDetails>(AppSettings.SQLQueryCommand.SP_GetPalletDetailsWithTruckID, _params, commandType: CommandType.StoredProcedure).ToList();

                var data1 = data;
                //string PalletName = pallets[0].PalletName;
                //string TruckNumber = 

                foreach (var pallet in pallets)
                {
                    string PalletName = pallet.PalletName;

                    var itemDetails = db.Query<ItemDetails>(AppSettings.SQLQueryCommand.SP_GetPalletWithItemDetails, new { truckDetails.TruckNumber, PalletName }, commandType: CommandType.StoredProcedure).ToList();

                    var item = new PalletItemDetails
                    {
                        PalletName = pallet.PalletName,
                        ItemDetails = itemDetails.ToList()
                    };

                    data1.Add(item);
                }
                
            }
            return data;
        }
        //public bool InsertWorkorderDetails(WorkorderCreationDetails workorderCreation)
        //{
        //    bool retval = false;
        //    using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        Guid obj = Guid.NewGuid();

        //        if (workorderCreation.TruckNumber != null)
        //        {
        //            var _params = new DynamicParameters();
        //            _params.Add("@WorkorderID", obj);
        //            _params.Add("@TruckID", workorderCreation.TruckNumber);
        //            _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //            db.Execute(AppSettings.SQLQueryCommand.SP_CreateWorkOrder, _params, commandType: CommandType.StoredProcedure);

        //            foreach (WorkorderCreationItemDetails i in workorderCreation.WorkorderDetails)
        //            {
        //                var _paramsWorkOrderDetails = new DynamicParameters();
        //                _paramsWorkOrderDetails.Add("@WorkorderID", obj);
        //                _paramsWorkOrderDetails.Add("@PalletName", i.PalletName);
        //                _paramsWorkOrderDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //                db.Execute(AppSettings.SQLQueryCommand.SP_CreateWorkOrderDetails, _paramsWorkOrderDetails, commandType: CommandType.StoredProcedure);


        //                retval = _paramsWorkOrderDetails.Get<bool>("@Status");
        //            }


        //            retval = _params.Get<bool>("@Status");
        //        }
        //    }
        //    return retval;
        //}
        

        #endregion

        #region PDA
        public LoginResponse MobileLogin(LoginModel login)
        {

            var loginResponse = new LoginResponse();
            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(AppSettings.ConnectionString))
            {


                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand())
                {
                    cmd.CommandText = AppSettings.SQLQueryCommand.MobileLogin;
                    cmd.Parameters.AddWithValue("@userName", login.UserName);
                    cmd.Parameters.AddWithValue("@password", login.Password);
                    cmd.Parameters.AddWithValue("@ClientDeviceId", login.ClientDeviceID);
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();

                    System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(cmd);

                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    DataTable tableA = ds.Tables[0];
                    DataTable tableB = ds.Tables[1];
                    DataTable tablec = ds.Tables[2];
                    loginResponse = ds.Tables[0].AsEnumerable()
   .Select(dataRow => new LoginResponse
   {
       UserID = dataRow.Field<string>("UserName"),
       TagPassword = /*dataRow.Field<string>("Password")*/ "0",
       CompanyCode = dataRow.Field<int>("CompanyCode"),
       CustomerID = dataRow.Field<Guid>("CustomerID"),
       //UserID = dataRow.Field<Guid>("UserID"),
       //ISLogRequired = dataRow.Field<bool>("ISLogRequired")
   }).First();

                    loginResponse.Dashboard = (ICollection<DashboardData>)ds.Tables[1].AsEnumerable()
.Select(dataRow => new DashboardData
{
    Menu_ID = dataRow.Field<string>("Menu_ID"),
    Menu_Name = dataRow.Field<string>("Menu_Name"),
    Menu_Image = dataRow.Field<string>("Menu_Image"),
    Menu_Is_Active = dataRow.Field<bool>("Menu_Is_Active"),
    Menu_Sequence = dataRow.Field<int>("Menu_Sequence"),
    Menu_Activity_Name = dataRow.Field<string>("Menu_Activity_Name")


}).ToList();

                    loginResponse.AssetType = (ICollection<AssetInfo>)ds.Tables[2].AsEnumerable()
.Select(dataRow => new AssetInfo
{
    AssetName = dataRow.Field<string>("AssetName"),
    ATypeID = dataRow.Field<int>("ATypeID")
}).ToList();

                    //                    loginResponse.Room = ds.Tables[3].AsEnumerable()
                    //.Select(dataRow => new RoomMasterLogin
                    //{
                    //    RoomID = dataRow.Field<Guid>("RoomID"),
                    //    RoomName = dataRow.Field<string>("RoomNo")

                    //}).ToList();

                    conn.Close();
                }
            }
            return loginResponse;


        }
        public List<AssetMasterResponse> GetAllAssetsforMobile(Guid tenantID)
        {
            List<AssetMasterResponse> locations = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@TenantID", tenantID);
                locations = db
                    .Query<AssetMasterResponse>(AppSettings.SQLQueryCommand.SP_Get_AllAssets_Mobile, _params, commandType: CommandType.StoredProcedure).ToList();
            }
            return locations;
        }
        //public List<AssetMasterResponse> GetAllAssetsforMobile(UserData user) //Master Response_GRB
        //{
        //    List<AssetMasterResponse> locations = new List<AssetMasterResponse>();
        //    //string connectionString = "Server=your_server_name;Database=your_database_name;User Id=your_username;Password=your_password;";
        //    string query = "SELECT AM.ATagID, AM.AName, ATM.AssetName FROM AssetMaster AM,AssetTypeMaster ATM WHERE AM.ATypeID=ATM.ATypeID and AM.CustomerID = @CustomerID "; // Replace YourTableName with the actual table name

        //    using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@CustomerID", user.CustomerID);
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    // Access data using reader.GetString(), reader.GetInt32(), etc.
        //                    AssetMasterResponse asset = new AssetMasterResponse();
        //                    asset.tagid = reader.GetString(0);
        //                    asset.name = reader.GetString(1);
        //                    asset.type = reader.GetString(2);
        //                    //asset.status = "true";
        //                    //asset.message = "Success";

        //                    //Console.WriteLine($"ID: {id}, Name: {name}, Email: {email}");
        //                    locations.Add(asset);
        //                }
        //            }
        //        }
        //    }
        //        return locations;
        //}
        public TruckResponseDetails GetTruckDetailsForPDA(TruckIDDetails truckIDDetails)
        {
            TruckResponseDetails data = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@TruckTagID", truckIDDetails.TruckTagID);
                _params.Add("@WarehouseID", truckIDDetails.WarehouseID);
                data = db.Query<TruckResponseDetails>(AppSettings.SQLQueryCommand.SP_GetTruckDetailsForPDA, _params, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return data;
        }
        //public ActivityResponseData CreateActivityMobile(ActivityModel activityModel) //Mapping_GRB
        //{
        //    ActivityResponseData responsedata = new ActivityResponseData();
        //    WMSPalletData wmsPalletData = new WMSPalletData();
        //    WMSPalletDataDispatch wmsPalletDataDis = new WMSPalletDataDispatch();
        //    List<WmsItems> itemsList = new List<WmsItems>();

        //    //bool retVal = false;
        //    Guid activityID = Guid.NewGuid();

        //    using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        DateTime TransactionDateTime = DateTime.Now;
        //        conn.Open();
        //        WorkorderCreationItemDetails workorderCreationItemDetails = new WorkorderCreationItemDetails();
        //        using (SqlTransaction transaction = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                var _params = new DynamicParameters();
        //                _params.Add("@ActivityID", activityID);
        //                _params.Add("@ClientDeviceID", activityModel.ClientDeviceID);
        //                _params.Add("@ActivityType", activityModel.ActivityType);
        //                _params.Add("@StartDate", activityModel.StartDate);
        //                _params.Add("@EndDate", activityModel.EndDate);
        //                //_params.Add("@TransactionDateTime", TransactionDateTime);
        //                _params.Add("@CustomerID", activityModel.CustomerID);
        //                _params.Add("@TouchPointID", activityModel.TouchpointID);
        //                _params.Add("@Count", activityModel.Count);
        //                _params.Add("@ParentTagID", activityModel.ParentTagID);
        //                _params.Add("@ParentAssetType", activityModel.ParentAssetType);
        //                _params.Add("@TruckNumber", activityModel.TruckNumber);
        //                _params.Add("@ProcessType", activityModel.ProcessType);
        //                _params.Add("@DRN", activityModel.DRN);
        //                _params.Add("@isUnique", dbType: DbType.Boolean, direction: ParameterDirection.Output);

        //                workorderCreationItemDetails.TruckNumber = activityModel.TruckNumber;
        //                workorderCreationItemDetails.PalletTagID = activityModel.ParentTagID;
        //                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertActivity, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
        //                responsedata.isUnique = _params.Get<bool>("@isUnique");
        //                if (responsedata.isUnique)
        //                {
        //                    foreach (var activityDetailsModel in activityModel.data)
        //                    {
        //                        WmsItems items = new WmsItems();
        //                        WmsQRInfo qrInfo = new WmsQRInfo();
        //                        var _paramsActivityDetails = new DynamicParameters();
        //                        _paramsActivityDetails.Add("@ItemDescription", activityDetailsModel.ItemDescription);

        //                        string[] parts;
        //                        if (activityDetailsModel.ItemDescription.Contains(','))
        //                        {
        //                            parts = activityDetailsModel.ItemDescription.Split(',');
        //                            if (parts.Length > 1)
        //                            {
        //                                string itemName = parts[1].TrimStart('0');
        //                                string itemSerialNo = parts[0].TrimStart('0');
        //                                string batchID = parts[3].TrimStart('0');
        //                                string batchDateTime = parts[2];
        //                                _paramsActivityDetails.Add("@ItemName", itemName);
        //                                items.skuCode = itemName;
        //                                qrInfo.skuCode = itemName;

        //                                _paramsActivityDetails.Add("@ItemSerialNo", itemSerialNo);
        //                                items.serialNumber = itemSerialNo;
        //                                qrInfo.serialNumber = itemSerialNo;


        //                                _paramsActivityDetails.Add("@BatchID", batchID);
        //                                items.batchNumber = batchID;
        //                                qrInfo.batchNumber = batchID;

        //                                DateTime date;
        //                                if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");
                                           
        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");
                                           
        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");
                                            
        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");
                                            
        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                _paramsActivityDetails.Add("@BatchDateTime", items.batchDateTime);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            parts = activityDetailsModel.ItemDescription.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //                            if (parts.Length > 1)
        //                            {
        //                                string itemName = parts[1].TrimStart('0');
        //                                string itemSerialNo = parts[0].TrimStart('0');
        //                                string batchID = parts[3].TrimStart('0');
        //                                string batchDateTime = parts[2];
        //                                _paramsActivityDetails.Add("@ItemName", itemName);
        //                                items.skuCode = itemName;
        //                                qrInfo.skuCode = itemName;

        //                                _paramsActivityDetails.Add("@ItemSerialNo", itemSerialNo);
        //                                items.serialNumber = itemSerialNo;
        //                                qrInfo.serialNumber = itemSerialNo;

        //                                _paramsActivityDetails.Add("@BatchID", batchID);
        //                                items.batchNumber = batchID;
        //                                qrInfo.batchNumber = batchID;

        //                                DateTime date;
        //                                if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");
                                        
        //                                }
        //                                // If parsing in "yyyy-MM-dd HH:mm:ss" format fails, try "yyyy-MM-dd" format
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");
                                            
        //                                }
        //                                // If parsing in "yyyy-MM-dd" format fails, try "MMM-yyyy" format
        //                                else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");
                                           
        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");
                                          
        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                _paramsActivityDetails.Add("@BatchDateTime", items.batchDateTime);
        //                            }
        //                        }

        //                        items.qrInfo = qrInfo;
        //                        items.qty = 1.0;

        //                        _paramsActivityDetails.Add("@ActivityID", activityID);
        //                        _paramsActivityDetails.Add("@TransactionDateTime", TransactionDateTime);
        //                        _paramsActivityDetails.Add("@ProcessType", activityModel.ProcessType);
        //                        _paramsActivityDetails.Add("@DRN", activityModel.DRN);
        //                        _paramsActivityDetails.Add("@isItemUnique", dbType: DbType.Boolean, direction: ParameterDirection.Output);

        //                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertActivityDetails, _paramsActivityDetails, commandType: CommandType.StoredProcedure, transaction: transaction);

        //                        responsedata.isItemUnique = _paramsActivityDetails.Get<bool>("@isItemUnique");

        //                        //WMS Data Posting

        //                        if (responsedata.isItemUnique)
        //                        {
        //                            if (items.skuCode != null)
        //                            {
        //                                itemsList.Add(items);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            var duplicateItemDescription = activityDetailsModel.ItemDescription;
        //                            if (duplicateItemDescription != null)
        //                            {
        //                                responsedata.message = $"Item '{duplicateItemDescription}' is already mapped.";
        //                            }
        //                            break;
        //                        }
        //                    }
        //                }
                       

        //                if (responsedata.isUnique && responsedata.isItemUnique)
        //                {
        //                    transaction.Commit();
                            
        //                    if (activityModel.ProcessType == "IN")
        //                    {
        //                        if (workorderCreationItemDetails != null)
        //                        {
        //                            InsertWorkorderItemDetailsU0(workorderCreationItemDetails);
        //                        }
        //                        wmsPalletData = GetPalletInfo(activityModel.TruckNumber, activityModel.ParentTagID);
        //                        wmsPalletData.receivingNo = activityModel.DRN;
        //                        wmsPalletData.items = itemsList;
        //                        wmsPalletData.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
        //                        string jsonRec = JsonConvert.SerializeObject(wmsPalletData);
        //                        responsedata.wmsData = jsonRec;
        //                        //PutPalletInfo(wmsPalletData);
        //                        var _wmsParams = new DynamicParameters();
        //                        _wmsParams.Add("@ID", activityID);
        //                        _wmsParams.Add("@DRN", activityModel.DRN);
        //                        _wmsParams.Add("@JSONData", Encoding.UTF8.GetBytes(jsonRec));
        //                        _wmsParams.Add("@ProcessType", "Asset_Pallet_Mapping_Receiving");
        //                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
                               
        //                    }
        //                    else if (activityModel.ProcessType == "OUT")
        //                    {
        //                        wmsPalletDataDis = GetPalletInfoDispatch(activityModel.DRN, activityModel.ParentTagID);
        //                        wmsPalletDataDis.dispatchNo = activityModel.DRN;
        //                        wmsPalletDataDis.items = itemsList;
        //                        wmsPalletDataDis.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
        //                        wmsPalletDataDis.comment = "Verified and Loaded on Truck";
        //                        string jsonDis = JsonConvert.SerializeObject(wmsPalletDataDis);
        //                        responsedata.wmsData = jsonDis;
        //                        var _wmsParams = new DynamicParameters();
        //                        _wmsParams.Add("@ID", activityID);
        //                        _wmsParams.Add("@DRN", activityModel.DRN);
        //                        _wmsParams.Add("@JSONData", Encoding.UTF8.GetBytes(jsonDis));
        //                        _wmsParams.Add("@ProcessType", "Asset_Pallet_Mapping_Dispatch");
        //                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                        //PutPalletInfoDispatch(wmsPalletDataDis);
        //                    }
        //                }
        //                else
        //                {
        //                        // If any condition is not met, rollback the transaction
        //                        transaction.Rollback();
        //                        if (!responsedata.isUnique)
        //                        {
        //                            responsedata.message = "The pallet has been already mapped.";
        //                        }
                               
        //                }
        //                responsedata.status = true;

        //            }
        //            catch (Exception ex)
        //            {
        //                responsedata.status = false;
        //                responsedata.message = ex.Message;

        //            }
        //        }
        //    }
        //    return responsedata;
        //}
        public ActivityResponseData CreateActivityMobile(ActivityModel activityModel) //Mapping_GRB
        {
            ActivityResponseData responsedata = new ActivityResponseData();
            WMSPalletData wmsPalletData = new WMSPalletData();
            WMSPalletDataDispatch wmsPalletDataDis = new WMSPalletDataDispatch();
            List<WmsItems> itemsList = new List<WmsItems>();

            //bool retVal = false;
            Guid activityID = Guid.NewGuid();

            using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
            {
                DateTime TransactionDateTime = DateTime.Now;
                conn.Open();
                WorkorderCreationItemDetails workorderCreationItemDetails = new WorkorderCreationItemDetails();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var _params = new DynamicParameters();
                        _params.Add("@ActivityID", activityID);
                        _params.Add("@ClientDeviceID", activityModel.ClientDeviceID);
                        _params.Add("@ActivityType", activityModel.ActivityType);
                        _params.Add("@StartDate", activityModel.StartDate);
                        _params.Add("@EndDate", activityModel.EndDate);
                        _params.Add("@CustomerID", activityModel.CustomerID);
                        _params.Add("@TouchPointID", activityModel.TouchpointID);
                        _params.Add("@Count", activityModel.Count);
                        _params.Add("@ParentTagID", activityModel.ParentTagID);
                        _params.Add("@ParentAssetType", activityModel.ParentAssetType);
                        _params.Add("@TruckNumber", activityModel.TruckNumber);
                        _params.Add("@ProcessType", activityModel.ProcessType);
                        _params.Add("@DRN", activityModel.DRN);
                        _params.Add("@WarehouseID", activityModel.WarehouseID);
                        _params.Add("@isUnique", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                        workorderCreationItemDetails.TruckNumber = activityModel.TruckNumber;
                        workorderCreationItemDetails.PalletTagID = activityModel.ParentTagID;
                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertActivity, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
                        responsedata.isUnique = _params.Get<bool>("@isUnique");
                        if (responsedata.isUnique)
                        {
                            foreach (var activityDetailsModel in activityModel.data)
                            {
                                WmsItems items = new WmsItems();
                                WmsQRInfo qrInfo = new WmsQRInfo();
                                var _paramsActivityDetails = new DynamicParameters();
                                _paramsActivityDetails.Add("@ItemDescription", activityDetailsModel.ItemDescription);

                                string[] parts;
                                if (activityDetailsModel.ItemDescription.Contains(','))
                                {
                                    parts = activityDetailsModel.ItemDescription.Split(',');
                                    if (parts.Length > 1)
                                    {
                                        string itemName = parts[1].TrimStart('0');
                                        string itemSerialNo = parts[0].TrimStart('0');
                                        string batchID = parts[3].TrimStart('0');
                                        string batchDateTime = parts[2];
                                        _paramsActivityDetails.Add("@ItemName", itemName);
                                        items.skuCode = itemName;
                                        qrInfo.skuCode = itemName;

                                        _paramsActivityDetails.Add("@ItemSerialNo", itemSerialNo);
                                        items.serialNumber = itemSerialNo;
                                        qrInfo.serialNumber = itemSerialNo;


                                        _paramsActivityDetails.Add("@BatchID", batchID);
                                        items.batchNumber = batchID;
                                        qrInfo.batchNumber = batchID;

                                        DateTime date;
                                        if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        _paramsActivityDetails.Add("@BatchDateTime", items.batchDateTime);
                                    }
                                }
                                else
                                {
                                    parts = activityDetailsModel.ItemDescription.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (parts.Length > 1)
                                    {
                                        string itemName = parts[1].TrimStart('0');
                                        string itemSerialNo = parts[0].TrimStart('0');
                                        string batchID = parts[3].TrimStart('0');
                                        string batchDateTime = parts[2];
                                        _paramsActivityDetails.Add("@ItemName", itemName);
                                        items.skuCode = itemName;
                                        qrInfo.skuCode = itemName;

                                        _paramsActivityDetails.Add("@ItemSerialNo", itemSerialNo);
                                        items.serialNumber = itemSerialNo;
                                        qrInfo.serialNumber = itemSerialNo;

                                        _paramsActivityDetails.Add("@BatchID", batchID);
                                        items.batchNumber = batchID;
                                        qrInfo.batchNumber = batchID;

                                        DateTime date;
                                        if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        // If parsing in "yyyy-MM-dd HH:mm:ss" format fails, try "yyyy-MM-dd" format
                                        else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        // If parsing in "yyyy-MM-dd" format fails, try "MMM-yyyy" format
                                        else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        _paramsActivityDetails.Add("@BatchDateTime", items.batchDateTime);
                                    }
                                }

                                items.qrInfo = qrInfo;
                                items.qty = 1.0;
                                items.status = activityDetailsModel.ItemStatus;

                                _paramsActivityDetails.Add("@ActivityID", activityID);
                                _paramsActivityDetails.Add("@TransactionDateTime", TransactionDateTime);
                                _paramsActivityDetails.Add("@ProcessType", activityModel.ProcessType);
                                _paramsActivityDetails.Add("@DRN", activityModel.DRN);
                                _paramsActivityDetails.Add("@WarehouseID", activityModel.WarehouseID);
                                _paramsActivityDetails.Add("@isItemUnique", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertActivityDetails, _paramsActivityDetails, commandType: CommandType.StoredProcedure, transaction: transaction);

                                responsedata.isItemUnique = _paramsActivityDetails.Get<bool>("@isItemUnique");

                                //WMS Data Posting

                                if (responsedata.isItemUnique)
                                {
                                    if (items.skuCode != null)
                                    {
                                        itemsList.Add(items);
                                    }
                                }
                                else
                                {
                                    var duplicateItemDescription = activityDetailsModel.ItemDescription;
                                    if (duplicateItemDescription != null)
                                    {
                                        responsedata.message = $"Item '{duplicateItemDescription}' is already mapped.";
                                    }
                                    break;
                                }
                            }
                        }


                        if (responsedata.isUnique && responsedata.isItemUnique)
                        {
                            transaction.Commit();

                            if (activityModel.ProcessType == "IN")
                            {
                                if (workorderCreationItemDetails != null)
                                {
                                    InsertWorkorderItemDetailsU0(workorderCreationItemDetails);
                                }
                                wmsPalletData = GetPalletInfo(activityModel.TruckNumber, activityModel.ParentTagID);
                                wmsPalletData.receivingNo = activityModel.DRN;
                                wmsPalletData.items = itemsList;
                                wmsPalletData.warehouseId = activityModel.WarehouseID.ToString();
                                string jsonRec = JsonConvert.SerializeObject(wmsPalletData);
                                responsedata.wmsData = jsonRec;
                                var _wmsParams = new DynamicParameters();
                                _wmsParams.Add("@ID", activityID);
                                _wmsParams.Add("@Type", "ASSET_PALLET_MAPPING_RECEIVING");
                                _wmsParams.Add("@receivingNo", wmsPalletData.receivingNo);
                                _wmsParams.Add("@dispatchNo", null);
                                _wmsParams.Add("@warehouseId", wmsPalletData.warehouseId);
                                _wmsParams.Add("@palletName", wmsPalletData.palletName);
                                _wmsParams.Add("@palletSensorId", activityModel.ParentTagID);
                                _wmsParams.Add("@binName", wmsPalletData.binName);
                                _wmsParams.Add("@fromBinName", null);
                                _wmsParams.Add("@fromPalletName", null);
                                _wmsParams.Add("@toPalletName", null);
                                _wmsParams.Add("@toBinName", null);
                                _wmsParams.Add("@comment", null);
                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                                foreach (var wmsData in wmsPalletData.items)
                                {
                                    var _wmsParamsDetails = new DynamicParameters();
                                    _wmsParamsDetails.Add("@ParentID", activityID);
                                    _wmsParamsDetails.Add("@Type", "ASSET_PALLET_MAPPING_RECEIVING");
                                    _wmsParamsDetails.Add("@fromBinName", null);
                                    _wmsParamsDetails.Add("@fromPalletName", null);
                                    _wmsParamsDetails.Add("@stockBinId", null);
                                    _wmsParamsDetails.Add("@serialNumber", wmsData.serialNumber);
                                    _wmsParamsDetails.Add("@skuCode", wmsData.skuCode);
                                    _wmsParamsDetails.Add("@batchNumber", wmsData.batchNumber);
                                    _wmsParamsDetails.Add("@batchDateTime", wmsData.batchDateTime);
                                    _wmsParamsDetails.Add("@status", wmsData.status);
                                    _wmsParamsDetails.Add("@qty", wmsData.qty);
                                    conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataChild, _wmsParamsDetails, commandType: CommandType.StoredProcedure);
                                }
                                

                            }
                            else if (activityModel.ProcessType == "OUT")
                            {
                                wmsPalletDataDis = GetPalletInfoDispatch(activityModel.DRN, activityModel.ParentTagID);
                                wmsPalletDataDis.dispatchNo = activityModel.DRN;
                                wmsPalletDataDis.items = itemsList;
                                wmsPalletDataDis.warehouseId = activityModel.WarehouseID.ToString();
                                wmsPalletDataDis.comment = "Verified and Loaded on Truck";
                                string jsonDis = JsonConvert.SerializeObject(wmsPalletDataDis);
                                responsedata.wmsData = jsonDis;
                                var _wmsParams = new DynamicParameters();
                                _wmsParams.Add("@ID", activityID);
                                _wmsParams.Add("@Type", "ASSET_PALLET_MAPPING_DISPATCH");
                                _wmsParams.Add("@receivingNo", null);
                                _wmsParams.Add("@dispatchNo", wmsPalletDataDis.dispatchNo);
                                _wmsParams.Add("@warehouseId", wmsPalletDataDis.warehouseId);
                                _wmsParams.Add("@palletName", wmsPalletDataDis.palletName);
                                _wmsParams.Add("@palletSensorId", activityModel.ParentTagID);
                                _wmsParams.Add("@binName", wmsPalletDataDis.binName);
                                _wmsParams.Add("@fromBinName", null);
                                _wmsParams.Add("@fromPalletName", null);
                                _wmsParams.Add("@toPalletName", null);
                                _wmsParams.Add("@toBinName", null);
                                _wmsParams.Add("@comment", wmsPalletDataDis.comment);
                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                                foreach (var wmsData in wmsPalletDataDis.items)
                                {
                                    var _wmsParamsDetails = new DynamicParameters();
                                    _wmsParamsDetails.Add("@ParentID", activityID);
                                    _wmsParamsDetails.Add("@Type", "ASSET_PALLET_MAPPING_DISPATCH");
                                    _wmsParamsDetails.Add("@fromBinName", null);
                                    _wmsParamsDetails.Add("@fromPalletName", null);
                                    _wmsParamsDetails.Add("@stockBinId", null);
                                    _wmsParamsDetails.Add("@serialNumber", wmsData.serialNumber);
                                    _wmsParamsDetails.Add("@skuCode", wmsData.skuCode);
                                    _wmsParamsDetails.Add("@batchNumber", wmsData.batchNumber);
                                    _wmsParamsDetails.Add("@batchDateTime", wmsData.batchDateTime);
                                    _wmsParamsDetails.Add("@status", null);
                                    _wmsParamsDetails.Add("@qty", wmsData.qty);
                                    conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataChild, _wmsParamsDetails, commandType: CommandType.StoredProcedure);
                                }
                           
                            }
                        }
                        else
                        {
                            // If any condition is not met, rollback the transaction
                            transaction.Rollback();
                            if (!responsedata.isUnique)
                            {
                                responsedata.message = "The pallet has been already mapped.";
                            }

                        }
                        responsedata.status = true;

                    }
                    catch (Exception ex)
                    {
                        responsedata.status = false;
                        responsedata.message = ex.Message;

                    }
                }
            }
            return responsedata;
        }
        public List<ItemDescriptionDetails> GetSTOLineItemsByTruckID(TruckDetails truckDetails)
        {
            List<ItemDescriptionDetails> data = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@TruckNumber", truckDetails.TruckNumber);
                _params.Add("@DRN", truckDetails.DRN);
                _params.Add("@ProcessType", truckDetails.ProcessType);
                data = db.Query<ItemDescriptionDetails>(AppSettings.SQLQueryCommand.SP_GetSTOLineItemsByTruckID, _params, commandType: CommandType.StoredProcedure).ToList();
            }
            return data;
        }
        //public ActivityResponseData InsertTransactionForItems(ActivityModelItems activityItems)
        //{
        //    //bool retVal = false;
        //    ActivityResponseData responseData = new ActivityResponseData();
        //    Guid activityID = Guid.NewGuid();
        //    Guid activityDetailsID = Guid.NewGuid();
        //    WMSPalletData wmsPalletData = new WMSPalletData();
        //    WMSPalletDataDispatch wmsPalletDataDis = new WMSPalletDataDispatch();
        //    List<WmsItems> itemsList = new List<WmsItems>();

        //    using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        DateTime TransactionDateTime = DateTime.Now;
        //        conn.Open();
        //        WorkorderCreationItemDetails workorderCreationItemDetails = new WorkorderCreationItemDetails();
        //        using (SqlTransaction transaction = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                var _params = new DynamicParameters();
        //                _params.Add("@ActivityID", activityID);
        //                _params.Add("@ClientDeviceID", activityItems.ClientDeviceID);
        //                _params.Add("@ActivityType", activityItems.ActivityType);
        //                _params.Add("@StartDate", activityItems.StartDate);
        //                _params.Add("@EndDate", activityItems.EndDate);
        //                //_params.Add("@TransactionDateTime", TransactionDateTime);
        //                _params.Add("@CustomerID", activityItems.CustomerID);
        //                _params.Add("@TouchPointID", activityItems.TouchpointID);
        //                _params.Add("@Count", activityItems.Count);
        //                _params.Add("@ParentTagID", activityItems.ParentTagID);
        //                _params.Add("@ParentAssetType", activityItems.ParentAssetType);
        //                _params.Add("@TruckNumber", activityItems.TruckNumber);
        //                _params.Add("@ProcessType", activityItems.ProcessType);
        //                _params.Add("@DRN", activityItems.DRN);
        //                _params.Add("@isUnique", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertActivity, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
        //                workorderCreationItemDetails.TruckNumber = activityItems.TruckNumber;
        //                workorderCreationItemDetails.PalletTagID = activityItems.ParentTagID;
        //                responseData.isUnique = _params.Get<bool>("@isUnique");
        //                if (responseData.isUnique) {
        //                    foreach (var activityItemDetails in activityItems.data)
        //                    {
        //                        WmsItems items = new WmsItems();
        //                        WmsQRInfo qrInfo = new WmsQRInfo();
        //                        var _paramsActivityDetails = new DynamicParameters();
        //                        //_paramsActivityDetails.Add("@ActivityDetailsID", activityDetailsID);

        //                        var ItemDesc = activityItemDetails.ItemDescription.Split(',');
        //                        items.skuCode = ItemDesc[2];
        //                        items.batchNumber = ItemDesc[1] == "null" ? null: ItemDesc[1];
        //                        qrInfo.skuCode = ItemDesc[2];
        //                        qrInfo.batchNumber = ItemDesc[1];
        //                        items.qrInfo = qrInfo;
        //                        items.qty = 1.0;
        //                        _paramsActivityDetails.Add("@ItemDescription", ItemDesc[0]);
        //                        _paramsActivityDetails.Add("@ItemName", ItemDesc[2]);
        //                        _paramsActivityDetails.Add("@BatchID", ItemDesc[1]);
        //                        _paramsActivityDetails.Add("@Qty", activityItemDetails.Qty);
        //                        _paramsActivityDetails.Add("@ActivityID", activityID);
        //                        _paramsActivityDetails.Add("@ProcessType", activityItems.ProcessType);
        //                        _paramsActivityDetails.Add("@DRN", activityItems.DRN);
        //                        _paramsActivityDetails.Add("@TransactionDateTime", TransactionDateTime);
        //                        _paramsActivityDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertActivityDetailsForItems, _paramsActivityDetails, commandType: CommandType.StoredProcedure, transaction: transaction);
        //                        responseData.isItemUnique = _paramsActivityDetails.Get<bool>("@Status");

        //                        //WMS Data Posting
        //                        if (responseData.isItemUnique)
        //                        {
        //                            if (items.skuCode != null)
        //                            {
        //                                for (int i = 0; i < activityItemDetails.Qty; i++)
        //                                {
        //                                    itemsList.Add(items);
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            responseData.message = "";
        //                        }
        //                    }
        //                }

        //                //WMS Data Posting
        //                if (responseData.isUnique && responseData.isItemUnique)
        //                {
        //                    transaction.Commit();
        //                    if (activityItems.ProcessType == "IN")
        //                    {
        //                        if (workorderCreationItemDetails != null)
        //                        {
        //                            InsertWorkorderItemDetailsU0(workorderCreationItemDetails);
        //                        }
        //                        wmsPalletData = GetPalletInfo(activityItems.TruckNumber, activityItems.ParentTagID);
        //                        wmsPalletData.receivingNo = activityItems.DRN;
        //                        wmsPalletData.items = itemsList;
        //                        wmsPalletData.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
        //                        string jsonRec = JsonConvert.SerializeObject(wmsPalletData);
        //                        responseData.wmsData = jsonRec;
        //                        var _wmsParams = new DynamicParameters();
        //                        _wmsParams.Add("@ID", activityID);
        //                        _wmsParams.Add("@DRN", activityItems.DRN);
        //                        _wmsParams.Add("@JSONData", Encoding.UTF8.GetBytes(jsonRec));
        //                        _wmsParams.Add("@ProcessType", "Asset_Pallet_Mapping_Receiving");
        //                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                        //PutPalletInfo(wmsPalletData);

        //                    }
        //                    else if (activityItems.ProcessType == "OUT")
        //                    {
        //                        wmsPalletDataDis = GetPalletInfoDispatch(activityItems.DRN, activityItems.ParentTagID);
        //                        wmsPalletDataDis.dispatchNo = activityItems.DRN;
        //                        wmsPalletDataDis.items = itemsList;
        //                        wmsPalletDataDis.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
        //                        wmsPalletDataDis.comment = "Verified and Loaded on Truck";
        //                        string jsonDis = JsonConvert.SerializeObject(wmsPalletDataDis);
        //                        responseData.wmsData = jsonDis;
        //                        var JsonByte = Encoding.UTF8.GetBytes(jsonDis);
        //                        var JsonString = Encoding.UTF8.GetString(JsonByte);
        //                        var _wmsParams = new DynamicParameters();
        //                        _wmsParams.Add("@ID", activityID);
        //                        _wmsParams.Add("@DRN", activityItems.DRN);
        //                        _wmsParams.Add("@JSONData", Encoding.UTF8.GetBytes(jsonDis));
        //                        _wmsParams.Add("@ProcessType", "Asset_Pallet_Mapping_Dispatch");
        //                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                        //PutPalletInfoDispatch(wmsPalletDataDis);

        //                    }
        //                }
        //                else
        //                { 
        //                        // If any condition is not met, rollback the transaction
        //                        transaction.Rollback();
        //                        if (!responseData.isUnique)
        //                        {
        //                            responseData.message = "The pallet has been already mapped.";
        //                        }
        //                }
        //                responseData.status = true;

        //            }
        //            catch (Exception ex)
        //            {
        //                responseData.status = false;
        //                responseData.message = ex.Message;

        //            }
        //        }
        //    }
        //    return responseData;
        //}
        public ActivityResponseData InsertTransactionForItems(ActivityModelItems activityItems)
        {
            //bool retVal = false;
            ActivityResponseData responseData = new ActivityResponseData();
            Guid activityID = Guid.NewGuid();
            Guid activityDetailsID = Guid.NewGuid();
            WMSPalletData wmsPalletData = new WMSPalletData();
            WMSPalletDataDispatch wmsPalletDataDis = new WMSPalletDataDispatch();
            List<WmsItems> itemsList = new List<WmsItems>();

            using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
            {
                DateTime TransactionDateTime = DateTime.Now;
                conn.Open();
                WorkorderCreationItemDetails workorderCreationItemDetails = new WorkorderCreationItemDetails();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var _params = new DynamicParameters();
                        _params.Add("@ActivityID", activityID);
                        _params.Add("@ClientDeviceID", activityItems.ClientDeviceID);
                        _params.Add("@ActivityType", activityItems.ActivityType);
                        _params.Add("@StartDate", activityItems.StartDate);
                        _params.Add("@EndDate", activityItems.EndDate);
                        //_params.Add("@TransactionDateTime", TransactionDateTime);
                        _params.Add("@CustomerID", activityItems.CustomerID);
                        _params.Add("@TouchPointID", activityItems.TouchpointID);
                        _params.Add("@Count", activityItems.Count);
                        _params.Add("@ParentTagID", activityItems.ParentTagID);
                        _params.Add("@ParentAssetType", activityItems.ParentAssetType);
                        _params.Add("@TruckNumber", activityItems.TruckNumber);
                        _params.Add("@ProcessType", activityItems.ProcessType);
                        _params.Add("@DRN", activityItems.DRN);
                        _params.Add("@WarehouseID", activityItems.WarehouseID);
                        _params.Add("@isUnique", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertActivity, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
                        workorderCreationItemDetails.TruckNumber = activityItems.TruckNumber;
                        workorderCreationItemDetails.PalletTagID = activityItems.ParentTagID;
                        responseData.isUnique = _params.Get<bool>("@isUnique");
                        if (responseData.isUnique)
                        {
                            foreach (var activityItemDetails in activityItems.data)
                            {
                                WmsItems items = new WmsItems();
                                WmsQRInfo qrInfo = new WmsQRInfo();
                                var _paramsActivityDetails = new DynamicParameters();
                                //_paramsActivityDetails.Add("@ActivityDetailsID", activityDetailsID);

                                var ItemDesc = activityItemDetails.ItemDescription.Split(',');
                                items.skuCode = ItemDesc[2];
                                items.batchNumber = ItemDesc[1] == "null" ? null : ItemDesc[1];
                                qrInfo.skuCode = ItemDesc[2];
                                qrInfo.batchNumber = ItemDesc[1];
                                items.qrInfo = qrInfo;
                                items.qty = 1.0;
                                _paramsActivityDetails.Add("@ItemDescription", ItemDesc[0]);
                                _paramsActivityDetails.Add("@ItemName", ItemDesc[2]);
                                _paramsActivityDetails.Add("@BatchID", ItemDesc[1]);
                                _paramsActivityDetails.Add("@Qty", activityItemDetails.Qty);
                                _paramsActivityDetails.Add("@ActivityID", activityID);
                                _paramsActivityDetails.Add("@ProcessType", activityItems.ProcessType);
                                _paramsActivityDetails.Add("@DRN", activityItems.DRN);
                                _paramsActivityDetails.Add("@WarehouseID", activityItems.WarehouseID);
                                _paramsActivityDetails.Add("@TransactionDateTime", TransactionDateTime);
                                _paramsActivityDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertActivityDetailsForItems, _paramsActivityDetails, commandType: CommandType.StoredProcedure, transaction: transaction);
                                responseData.isItemUnique = _paramsActivityDetails.Get<bool>("@Status");

                                //WMS Data Posting
                                if (responseData.isItemUnique)
                                {
                                    if (items.skuCode != null)
                                    {
                                        for (int i = 0; i < activityItemDetails.Qty; i++)
                                        {
                                            itemsList.Add(items);
                                        }
                                    }
                                }
                                else
                                {
                                    responseData.message = "";
                                }
                            }
                        }

                        //WMS Data Posting
                        if (responseData.isUnique && responseData.isItemUnique)
                        {
                            transaction.Commit();
                            if (activityItems.ProcessType == "IN")
                            {
                                if (workorderCreationItemDetails != null)
                                {
                                    InsertWorkorderItemDetailsU0(workorderCreationItemDetails);
                                }
                                wmsPalletData = GetPalletInfo(activityItems.TruckNumber, activityItems.ParentTagID);
                                wmsPalletData.receivingNo = activityItems.DRN;
                                wmsPalletData.items = itemsList;
                                wmsPalletData.warehouseId = activityItems.WarehouseID.ToString();
                                string jsonRec = JsonConvert.SerializeObject(wmsPalletData);
                                responseData.wmsData = jsonRec;
                                var _wmsParams = new DynamicParameters();
                                _wmsParams.Add("@ID", activityID);
                                _wmsParams.Add("@Type", "ASSET_PALLET_MAPPING_RECEIVING");
                                _wmsParams.Add("@receivingNo", wmsPalletData.receivingNo);
                                _wmsParams.Add("@dispatchNo", null);
                                _wmsParams.Add("@warehouseId", wmsPalletData.warehouseId);
                                _wmsParams.Add("@palletName", wmsPalletData.palletName);
                                _wmsParams.Add("@palletSensorId", activityItems.ParentTagID);
                                _wmsParams.Add("@binName", wmsPalletData.binName);
                                _wmsParams.Add("@fromBinName", null);
                                _wmsParams.Add("@fromPalletName", null);
                                _wmsParams.Add("@toPalletName", null);
                                _wmsParams.Add("@toBinName", null);
                                _wmsParams.Add("@comment", null);
                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                                foreach (var wmsData in wmsPalletData.items)
                                {
                                    var _wmsParamsDetails = new DynamicParameters();
                                    _wmsParamsDetails.Add("@ParentID", activityID);
                                    _wmsParamsDetails.Add("@Type", "ASSET_PALLET_MAPPING_RECEIVING");
                                    _wmsParamsDetails.Add("@fromBinName", null);
                                    _wmsParamsDetails.Add("@fromPalletName", null);
                                    _wmsParamsDetails.Add("@stockBinId", null);
                                    _wmsParamsDetails.Add("@serialNumber", null);
                                    _wmsParamsDetails.Add("@skuCode", wmsData.skuCode);
                                    _wmsParamsDetails.Add("@batchNumber", wmsData.batchNumber);
                                    _wmsParamsDetails.Add("@batchDateTime", null);
                                    _wmsParamsDetails.Add("@status", null);
                                    _wmsParamsDetails.Add("@qty", wmsData.qty);
                                    conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataChild, _wmsParamsDetails, commandType: CommandType.StoredProcedure);
                                }
                            }
                            else if (activityItems.ProcessType == "OUT")
                            {
                                wmsPalletDataDis = GetPalletInfoDispatch(activityItems.DRN, activityItems.ParentTagID);
                                wmsPalletDataDis.dispatchNo = activityItems.DRN;
                                wmsPalletDataDis.items = itemsList;
                                wmsPalletDataDis.warehouseId = activityItems.WarehouseID.ToString();
                                wmsPalletDataDis.comment = "Verified and Loaded on Truck";
                                string jsonDis = JsonConvert.SerializeObject(wmsPalletDataDis);
                                responseData.wmsData = jsonDis;
                                var JsonByte = Encoding.UTF8.GetBytes(jsonDis);
                                var JsonString = Encoding.UTF8.GetString(JsonByte);
                                var _wmsParams = new DynamicParameters();
                                _wmsParams.Add("@ID", activityID);
                                _wmsParams.Add("@Type", "ASSET_PALLET_MAPPING_DISPATCH");
                                _wmsParams.Add("@receivingNo", null);
                                _wmsParams.Add("@dispatchNo", wmsPalletDataDis.dispatchNo);
                                _wmsParams.Add("@warehouseId", wmsPalletDataDis.warehouseId);
                                _wmsParams.Add("@palletName", wmsPalletDataDis.palletName);
                                _wmsParams.Add("@palletSensorId", activityItems.ParentTagID);
                                _wmsParams.Add("@binName", wmsPalletDataDis.binName);
                                _wmsParams.Add("@fromBinName", null);
                                _wmsParams.Add("@fromPalletName", null);
                                _wmsParams.Add("@toPalletName", null);
                                _wmsParams.Add("@toBinName", null);
                                _wmsParams.Add("@comment", wmsPalletDataDis.comment);
                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                                foreach (var wmsData in wmsPalletDataDis.items)
                                {
                                    var _wmsParamsDetails = new DynamicParameters();
                                    _wmsParamsDetails.Add("@ParentID", activityID);
                                    _wmsParamsDetails.Add("@Type", "ASSET_PALLET_MAPPING_DISPATCH");
                                    _wmsParamsDetails.Add("@fromBinName", null);
                                    _wmsParamsDetails.Add("@fromPalletName", null);
                                    _wmsParamsDetails.Add("@stockBinId", null);
                                    _wmsParamsDetails.Add("@serialNumber", null);
                                    _wmsParamsDetails.Add("@skuCode", wmsData.skuCode);
                                    _wmsParamsDetails.Add("@batchNumber", wmsData.batchNumber);
                                    _wmsParamsDetails.Add("@batchDateTime", null);
                                    _wmsParamsDetails.Add("@status", null);
                                    _wmsParamsDetails.Add("@qty", wmsData.qty);
                                    conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataChild, _wmsParamsDetails, commandType: CommandType.StoredProcedure);
                                }

                            }
                        }
                        else
                        {
                            // If any condition is not met, rollback the transaction
                            transaction.Rollback();
                            if (!responseData.isUnique)
                            {
                                responseData.message = "The pallet has been already mapped.";
                            }
                        }
                        responseData.status = true;

                    }
                    catch (Exception ex)
                    {
                        responseData.status = false;
                        responseData.message = ex.Message;

                    }
                }
            }
            return responseData;
        }
        public List<PartialWorkOrderResponse> GetPartialWorkorderList(DeviceData deviceData)
        {
            List<PartialWorkOrderResponse> data = new List<PartialWorkOrderResponse>();
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@clientDeviceID", deviceData.ClientDeviceID);
                _params.Add("@WarehouseID", deviceData.WarehouseID);
                data = db.Query<PartialWorkOrderResponse>(AppSettings.SQLQueryCommand.SP_GetPartialWorkorderList, _params, commandType: CommandType.StoredProcedure).ToList();
            }
            return data;
        }
        public List<PickListItemDetails> GetPartialWorkorderItemDetails(PartialWorkorderListRequest listRequest)
        {
            List<PickListItemDetails> data = new List<PickListItemDetails>();
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@clientDeviceID", listRequest.ClientDeviceID);
                _params.Add("@WorkorderNumber", listRequest.WorkorderNumber);
                _params.Add("@WorkorderType", listRequest.WorkorderType);
                data = db.Query<PickListItemDetails>(AppSettings.SQLQueryCommand.SP_GetPartialWorkorderItemDetails, _params, commandType: CommandType.StoredProcedure).ToList();
            }
            return data;
        }
        //public Response InsertPartialWorkorderDetails(PartialTransactionDetails transactionDetails)
        //{
        //    Response res = new Response();
        //    bool stat1 = false;
        //    bool stat2 = false;
        //    string mess = string.Empty;
        //    Guid workorderDetailsID = Guid.NewGuid();
        //    using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        DateTime TransactionDateTime = DateTime.Now;
        //        conn.Open();
        //        using (SqlTransaction transaction = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                var _params = new DynamicParameters();
        //                WmsPalletBinInfoL0 wmsPalletBinInfoL0 = new WmsPalletBinInfoL0();
        //                _params.Add("@WorkorderDetailsID", workorderDetailsID);
        //                _params.Add("@CustomerID", transactionDetails.CustomerID);
        //                _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
        //                _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
        //                _params.Add("@WorkorderType", transactionDetails.WorkorderType);
        //                _params.Add("@TransactionDateTime", TransactionDateTime);
        //                _params.Add("@PalletTagID", transactionDetails.PalletTagID);
        //                _params.Add("@PalletName", transactionDetails.PalletName);
        //                _params.Add("@LocationTagID", transactionDetails.LocationTagID);
        //                _params.Add("@LocationCategoryID", transactionDetails.LocationCategoryID);
        //                _params.Add("@DCNumber", transactionDetails.DCNumber);
        //                _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //                _params.Add("@Message", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);
        //                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertPartialWorkorder, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
        //                stat1 = _params.Get<bool>("@Status");
        //                mess = _params.Get<string>("@Message");

        //                if (stat1)
        //                {
        //                    foreach (PartialItemDetails i in transactionDetails.ItemDetails)
        //                    {
        //                        var _paramsItemDetails = new DynamicParameters();
        //                        _paramsItemDetails.Add("@WorkorderDetailsID", workorderDetailsID);
        //                        _paramsItemDetails.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
        //                        _paramsItemDetails.Add("@WorkorderType", transactionDetails.WorkorderType);
        //                        _paramsItemDetails.Add("@BinName", i.BinName);
        //                        _paramsItemDetails.Add("@ItemDescription", i.ItemDescription);
        //                        _paramsItemDetails.Add("@ItemName", i.ItemName);
        //                        _paramsItemDetails.Add("@PickedItemID", i.PickedItemID);
        //                        _paramsItemDetails.Add("@BatchID", i.BatchID);
        //                        _paramsItemDetails.Add("@Qty", i.Qty);
        //                        _paramsItemDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertPartialWorkorderItems, _paramsItemDetails, commandType: CommandType.StoredProcedure, transaction: transaction);
        //                        stat2 = _paramsItemDetails.Get<bool>("@Status");
        //                    }
        //                    res.status = stat1;
        //                }
        //                else
        //                {
        //                    res.status = stat1;
        //                    res.message = mess;
        //                }

        //                if (stat1 && stat2)
        //                {
        //                    transaction.Commit();
        //                    wmsPalletBinInfoL0 = GetPalletBinMappedInfoL0(transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, transactionDetails.PalletTagID, transactionDetails.LocationTagID);
        //                    string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoL0);
        //                    var _wmsParams = new DynamicParameters();
        //                    _wmsParams.Add("@ID", workorderDetailsID);
        //                    _wmsParams.Add("@DRN", wmsPalletBinInfoL0.dispatchNo);
        //                    _wmsParams.Add("@JSONData", Encoding.UTF8.GetBytes(jsonData));
        //                    _wmsParams.Add("@ProcessType", "Pallet_Movement_L0");
        //                    conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                    //PutPalletBinMappedInfoL0(wmsPalletBinInfoL0);
        //                    res.status = true;
        //                }
        //                else
        //                {
        //                    res.status = false;
        //                    res.message = mess;
        //                }
        //            }
        //            catch(Exception ex)
        //            {
        //                transaction.Rollback();
        //                res.status = false;
        //                res.message = ex.Message;
        //            }

        //        }
        //    }
        //    return res;
        //}
        public Response InsertPartialWorkorderDetails(PartialTransactionDetails transactionDetails)
        {
            Response res = new Response();
            bool stat1 = false;
            bool stat2 = false;
            string mess = string.Empty;
            Guid workorderDetailsID = Guid.NewGuid();
            using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
            {
                DateTime TransactionDateTime = DateTime.Now;
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var _params = new DynamicParameters();
                        WmsPalletBinInfoL0 wmsPalletBinInfoL0 = new WmsPalletBinInfoL0();
                        _params.Add("@WorkorderDetailsID", workorderDetailsID);
                        _params.Add("@CustomerID", transactionDetails.CustomerID);
                        _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
                        _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
                        _params.Add("@WorkorderType", transactionDetails.WorkorderType);
                        _params.Add("@TransactionDateTime", TransactionDateTime);
                        _params.Add("@PalletTagID", transactionDetails.PalletTagID);
                        _params.Add("@PalletName", transactionDetails.PalletName);
                        _params.Add("@LocationTagID", transactionDetails.LocationTagID);
                        _params.Add("@LocationCategoryID", transactionDetails.LocationCategoryID);
                        _params.Add("@DCNumber", transactionDetails.DCNo);
                        _params.Add("@WarehouseID", transactionDetails.WarehouseID);
                        _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                        _params.Add("@Message", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);
                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertPartialWorkorder, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
                        stat1 = _params.Get<bool>("@Status");
                        mess = _params.Get<string>("@Message");

                        if (stat1)
                        {
                            foreach (PartialItemDetails i in transactionDetails.ItemDetails)
                            {
                                WmsItemsList items = new WmsItemsList();
                                var _paramsItemDetails = new DynamicParameters();
                                _paramsItemDetails.Add("@WorkorderDetailsID", workorderDetailsID);
                                _paramsItemDetails.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
                                _paramsItemDetails.Add("@WorkorderType", transactionDetails.WorkorderType);
                                _paramsItemDetails.Add("@stockBinId", i.stockBinId);
                                _paramsItemDetails.Add("@BinName", i.BinName);
                                _paramsItemDetails.Add("@ItemDescription", i.ItemDescription);
                                _paramsItemDetails.Add("@ItemName", i.ItemName);
                                _paramsItemDetails.Add("@PickedItemID", i.PickedItemID);
                                _paramsItemDetails.Add("@BatchID", i.BatchID);
                                _paramsItemDetails.Add("@Qty", i.Qty);
                                _paramsItemDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertPartialWorkorderItems, _paramsItemDetails, commandType: CommandType.StoredProcedure, transaction: transaction);
                                stat2 = _paramsItemDetails.Get<bool>("@Status");
                            }
                            res.status = stat1;
                        }
                        else
                        {
                            res.status = stat1;
                            res.message = mess;
                        }

                        if (stat1 && stat2)
                        {
                            transaction.Commit();
                            wmsPalletBinInfoL0 = GetPalletBinMappedInfoL0(transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, transactionDetails.PalletTagID, transactionDetails.LocationTagID);
                            string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoL0);
                            var _wmsParams = new DynamicParameters();
                            _wmsParams.Add("@ID", workorderDetailsID);
                            _wmsParams.Add("@Type", "Pallet_Movement_L0");
                            _wmsParams.Add("@receivingNo", null);
                            _wmsParams.Add("@dispatchNo", transactionDetails.DCNo);
                            _wmsParams.Add("@warehouseId", transactionDetails.WarehouseID.ToString());
                            _wmsParams.Add("@palletName", null);
                            _wmsParams.Add("@palletSensorId", transactionDetails.PalletTagID);
                            _wmsParams.Add("@binName", null);
                            _wmsParams.Add("@fromBinName", null);
                            _wmsParams.Add("@fromPalletName", null);
                            _wmsParams.Add("@toPalletName", wmsPalletBinInfoL0.toPalletName);
                            _wmsParams.Add("@toBinName", wmsPalletBinInfoL0.toBinName);
                            _wmsParams.Add("@comment", null);
                            conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                            foreach (var wmsData in wmsPalletBinInfoL0.items)
                            {
                                var _wmsParamsDetails = new DynamicParameters();
                                _wmsParamsDetails.Add("@ParentID", workorderDetailsID);
                                _wmsParamsDetails.Add("@Type", "Pallet_Movement_L0");
                                _wmsParamsDetails.Add("@fromBinName", wmsData.fromBinName);
                                _wmsParamsDetails.Add("@fromPalletName", null);
                                _wmsParamsDetails.Add("@stockBinId", wmsData.stockBinId);
                                _wmsParamsDetails.Add("@serialNumber", null);
                                _wmsParamsDetails.Add("@skuCode", wmsData.skuCode);
                                _wmsParamsDetails.Add("@batchNumber", wmsData.batchNumber);
                                _wmsParamsDetails.Add("@batchDateTime", null);
                                _wmsParamsDetails.Add("@status", null);
                                _wmsParamsDetails.Add("@qty", wmsData.qty);
                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataChild, _wmsParamsDetails, commandType: CommandType.StoredProcedure);
                            }
                            res.status = true;
                        }
                        else
                        {
                            res.status = false;
                            res.message = mess;
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        res.status = false;
                        res.message = ex.Message;
                    }

                }
            }
            return res;
        }
        public List<OrderDetails> GetWorkorderListForPDA(PDAWorkorderList workorderList)
        {
            List<OrderDetails> data = new List<OrderDetails>();
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@ClientDeviceID", workorderList.ClientDeviceID);
                data = db.Query<OrderDetails>(AppSettings.SQLQueryCommand.SP_GetWorkorderListForPDA, _params, commandType: CommandType.StoredProcedure).ToList();
            }
            return data;
        }
        //public List<PartialItemDetails> GetBinInfo(BinDetailsRequest binDetails)
        //{
        //    List<PartialItemDetails> data = new List<PartialItemDetails>();
        //    using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        var _params = new DynamicParameters();
        //        _params.Add("@BinName", binDetails.BinName);
        //        var response = GetItemDetailsByBinName(binDetails.BinName);
        //        data.AddRange(response);
        //    }
        //    return data;
        //}
        //public bool ItemMovementTransaction(ActivityModelInternal activityModel)
        //{
        //    bool Status1 = false;
        //    bool Status2 = false;
        //    bool retVal = false;
        //    WmsPalletBinInfoInternal wmsPalletBinInfo = new WmsPalletBinInfoInternal();
        //    List<WmsPalletBinInfoInternalDetails> wmsPalletBinInfoInternalList = new List<WmsPalletBinInfoInternalDetails>();
        //    WmsPalletBinInfoInternalDetails wmsPalletBinInfoInternal = new WmsPalletBinInfoInternalDetails();
        //    List<WmsItemsInternal> itemsList = new List<WmsItemsInternal>();

        //    //bool retVal = false;
        //    Guid activityID = Guid.NewGuid();

        //    using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        DateTime TransactionDateTime = DateTime.Now;
        //        conn.Open();

        //        using (SqlTransaction transaction = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                var _params = new DynamicParameters();
        //                _params.Add("@ActivityID", activityID);
        //                _params.Add("@ClientDeviceID", activityModel.ClientDeviceID);
        //                _params.Add("@ActivityType", activityModel.ActivityType);
        //                _params.Add("@StartDate", activityModel.StartDate);
        //                _params.Add("@EndDate", activityModel.EndDate);
        //                _params.Add("@CustomerID", activityModel.CustomerID);
        //                _params.Add("@Count", activityModel.Count);
        //                _params.Add("@SourcePalletTagID", activityModel.SourcePalletTagID);
        //                _params.Add("@DestinationPalletTagID", activityModel.DestinationPalletTagID);
        //                _params.Add("@SourceBinTagID", activityModel.SourceBinTagID);
        //                _params.Add("@DestinationBinTagID", activityModel.DestinationBinTagID);
        //                _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);

        //                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertItemMovementActivity, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
        //                Status1 = _params.Get<bool>("@Status");
        //                if (Status1)
        //                {
        //                    foreach (var activityDetailsModelInternal in activityModel.data)
        //                    {
        //                        WmsItemsInternal items = new WmsItemsInternal();
        //                        WmsQRInfoInternal qrInfo = new WmsQRInfoInternal();
        //                        var _paramsActivityDetails = new DynamicParameters();
        //                        _paramsActivityDetails.Add("@ItemDescription", activityDetailsModelInternal.ItemDescription);

        //                        string[] parts;
        //                        if (activityDetailsModelInternal.ItemDescription.Contains(','))
        //                        {
        //                            parts = activityDetailsModelInternal.ItemDescription.Split(',');
        //                            if (parts.Length > 1)
        //                            {
        //                                string itemName = parts[1].TrimStart('0');
        //                                string itemSerialNo = parts[0].TrimStart('0');
        //                                string batchID = parts[3].TrimStart('0');
        //                                string batchDateTime = parts[2];
        //                                _paramsActivityDetails.Add("@ItemName", itemName);
        //                                items.skuCode = itemName;
        //                                qrInfo.skuCode = itemName;

        //                                _paramsActivityDetails.Add("@ItemSerialNo", itemSerialNo);
        //                                items.serialNumber = itemSerialNo;
        //                                qrInfo.serialNumber = itemSerialNo;


        //                                _paramsActivityDetails.Add("@BatchID", batchID);
        //                                items.batchNumber = batchID;
        //                                qrInfo.batchNumber = batchID;

        //                                DateTime date;
        //                                if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                _paramsActivityDetails.Add("@BatchDateTime", items.batchDateTime);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            parts = activityDetailsModelInternal.ItemDescription.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //                            if (parts.Length > 1)
        //                            {
        //                                string itemName = parts[1].TrimStart('0');
        //                                string itemSerialNo = parts[0].TrimStart('0');
        //                                string batchID = parts[3].TrimStart('0');
        //                                string batchDateTime = parts[2];
        //                                _paramsActivityDetails.Add("@ItemName", itemName);
        //                                items.skuCode = itemName;
        //                                qrInfo.skuCode = itemName;

        //                                _paramsActivityDetails.Add("@ItemSerialNo", itemSerialNo);
        //                                items.serialNumber = itemSerialNo;
        //                                qrInfo.serialNumber = itemSerialNo;

        //                                _paramsActivityDetails.Add("@BatchID", batchID);
        //                                items.batchNumber = batchID;
        //                                qrInfo.batchNumber = batchID;

        //                                DateTime date;
        //                                if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                // If parsing in "yyyy-MM-dd HH:mm:ss" format fails, try "yyyy-MM-dd" format
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                // If parsing in "yyyy-MM-dd" format fails, try "MMM-yyyy" format
        //                                else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                _paramsActivityDetails.Add("@BatchDateTime", items.batchDateTime);
        //                            }
        //                        }

        //                        items.qrInfo = qrInfo;
        //                        items.status = "GOOD";
        //                        items.qty = 1.0;

        //                        _paramsActivityDetails.Add("@ActivityID", activityID);
        //                        _paramsActivityDetails.Add("@TransactionDateTime", TransactionDateTime);
        //                        _paramsActivityDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);

        //                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertItemMovementActivityDetails, _paramsActivityDetails, commandType: CommandType.StoredProcedure, transaction: transaction);

        //                        Status2 = _paramsActivityDetails.Get<bool>("@Status");

        //                        //WMS Data Posting

        //                        if (Status2)
        //                        {
        //                            if (items.skuCode != null)
        //                            {
        //                                itemsList.Add(items);
        //                            }
        //                        }
        //                    }
        //                }


        //                if (Status1 && Status2)
        //                {
        //                    transaction.Commit();
        //                    retVal = true;


        //                    wmsPalletBinInfoInternal = GetPalletInfoForItemMovement(activityModel.SourcePalletTagID, activityModel.DestinationPalletTagID, activityModel.SourceBinTagID, activityModel.DestinationBinTagID);
        //                    wmsPalletBinInfoInternal.items = itemsList;
        //                    wmsPalletBinInfoInternal.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
        //                    wmsPalletBinInfoInternalList.Add(wmsPalletBinInfoInternal);
        //                    wmsPalletBinInfo.data = wmsPalletBinInfoInternalList;
        //                    string jsonInternal = JsonConvert.SerializeObject(wmsPalletBinInfo);
        //                    var _wmsParams = new DynamicParameters();
        //                    _wmsParams.Add("@ID", activityID);
        //                    _wmsParams.Add("@DRN", "Item_Movement");
        //                    _wmsParams.Add("@JSONData", Encoding.UTF8.GetBytes(jsonInternal));
        //                    _wmsParams.Add("@ProcessType", "Item_Movement");
        //                    conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);

        //                }
        //                else
        //                {
        //                    // If any condition is not met, rollback the transaction
        //                    transaction.Rollback();
        //                    retVal = false;

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                retVal = false;

        //            }
        //        }
        //    }
        //    return retVal;
        //}
        public bool ItemMovementTransaction(ActivityModelInternal activityModel)
        {
            bool Status1 = false;
            bool Status2 = false;
            bool retVal = false;
            WmsPalletBinInfoInternal wmsPalletBinInfo = new WmsPalletBinInfoInternal();
            List<WmsPalletBinInfoInternalDetails> wmsPalletBinInfoInternalList = new List<WmsPalletBinInfoInternalDetails>();
            WmsPalletBinInfoInternalDetails wmsPalletBinInfoInternal = new WmsPalletBinInfoInternalDetails();
            List<WmsItemsInternal> itemsList = new List<WmsItemsInternal>();

            //bool retVal = false;
            Guid activityID = Guid.NewGuid();

            using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
            {
                DateTime TransactionDateTime = DateTime.Now;
                conn.Open();

                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var _params = new DynamicParameters();
                        _params.Add("@ActivityID", activityID);
                        _params.Add("@ClientDeviceID", activityModel.ClientDeviceID);
                        _params.Add("@ActivityType", activityModel.ActivityType);
                        _params.Add("@StartDate", activityModel.StartDate);
                        _params.Add("@EndDate", activityModel.EndDate);
                        _params.Add("@CustomerID", activityModel.CustomerID);
                        _params.Add("@Count", activityModel.Count);
                        _params.Add("@SourcePalletTagID", activityModel.SourcePalletTagID);
                        _params.Add("@DestinationPalletTagID", activityModel.DestinationPalletTagID);
                        _params.Add("@SourceBinTagID", activityModel.SourceBinTagID);
                        _params.Add("@DestinationBinTagID", activityModel.DestinationBinTagID);
                        _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertItemMovementActivity, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
                        Status1 = _params.Get<bool>("@Status");
                        if (Status1)
                        {
                            foreach (var activityDetailsModelInternal in activityModel.data)
                            {
                                WmsItemsInternal items = new WmsItemsInternal();
                                WmsQRInfoInternal qrInfo = new WmsQRInfoInternal();
                                var _paramsActivityDetails = new DynamicParameters();
                                _paramsActivityDetails.Add("@ItemDescription", activityDetailsModelInternal.ItemDescription);

                                string[] parts;
                                if (activityDetailsModelInternal.ItemDescription.Contains(','))
                                {
                                    parts = activityDetailsModelInternal.ItemDescription.Split(',');
                                    if (parts.Length > 1)
                                    {
                                        string itemName = parts[1].TrimStart('0');
                                        string itemSerialNo = parts[0].TrimStart('0');
                                        string batchID = parts[3].TrimStart('0');
                                        string batchDateTime = parts[2];
                                        _paramsActivityDetails.Add("@ItemName", itemName);
                                        items.skuCode = itemName;
                                        qrInfo.skuCode = itemName;

                                        _paramsActivityDetails.Add("@ItemSerialNo", itemSerialNo);
                                        items.serialNumber = itemSerialNo;
                                        qrInfo.serialNumber = itemSerialNo;


                                        _paramsActivityDetails.Add("@BatchID", batchID);
                                        items.batchNumber = batchID;
                                        qrInfo.batchNumber = batchID;

                                        DateTime date;
                                        if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        _paramsActivityDetails.Add("@BatchDateTime", items.batchDateTime);
                                    }
                                }
                                else
                                {
                                    parts = activityDetailsModelInternal.ItemDescription.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (parts.Length > 1)
                                    {
                                        string itemName = parts[1].TrimStart('0');
                                        string itemSerialNo = parts[0].TrimStart('0');
                                        string batchID = parts[3].TrimStart('0');
                                        string batchDateTime = parts[2];
                                        _paramsActivityDetails.Add("@ItemName", itemName);
                                        items.skuCode = itemName;
                                        qrInfo.skuCode = itemName;

                                        _paramsActivityDetails.Add("@ItemSerialNo", itemSerialNo);
                                        items.serialNumber = itemSerialNo;
                                        qrInfo.serialNumber = itemSerialNo;

                                        _paramsActivityDetails.Add("@BatchID", batchID);
                                        items.batchNumber = batchID;
                                        qrInfo.batchNumber = batchID;

                                        DateTime date;
                                        if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        // If parsing in "yyyy-MM-dd HH:mm:ss" format fails, try "yyyy-MM-dd" format
                                        else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        // If parsing in "yyyy-MM-dd" format fails, try "MMM-yyyy" format
                                        else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        _paramsActivityDetails.Add("@BatchDateTime", items.batchDateTime);
                                    }
                                }

                                items.qrInfo = qrInfo;
                                items.status = "GOOD";
                                items.qty = 1.0;

                                _paramsActivityDetails.Add("@ActivityID", activityID);
                                _paramsActivityDetails.Add("@TransactionDateTime", TransactionDateTime);
                                _paramsActivityDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertItemMovementActivityDetails, _paramsActivityDetails, commandType: CommandType.StoredProcedure, transaction: transaction);

                                Status2 = _paramsActivityDetails.Get<bool>("@Status");

                                //WMS Data Posting

                                if (Status2)
                                {
                                    if (items.skuCode != null)
                                    {
                                        itemsList.Add(items);
                                    }
                                }
                            }
                        }


                        if (Status1 && Status2)
                        {
                            transaction.Commit();
                            retVal = true;


                            wmsPalletBinInfoInternal = GetPalletInfoForItemMovement(activityModel.SourcePalletTagID, activityModel.DestinationPalletTagID, activityModel.SourceBinTagID, activityModel.DestinationBinTagID);
                            wmsPalletBinInfoInternal.items = itemsList;
                            wmsPalletBinInfoInternal.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
                            wmsPalletBinInfoInternalList.Add(wmsPalletBinInfoInternal);
                            wmsPalletBinInfo.data = wmsPalletBinInfoInternalList;
                            string jsonInternal = JsonConvert.SerializeObject(wmsPalletBinInfo);
                           
                            var _wmsParams = new DynamicParameters();
                            _wmsParams.Add("@ID", activityID);
                            _wmsParams.Add("@Type", "Item_Movement");
                            _wmsParams.Add("@receivingNo", null);
                            _wmsParams.Add("@dispatchNo", null);
                            _wmsParams.Add("@warehouseId", wmsPalletBinInfoInternal.warehouseId);
                            _wmsParams.Add("@palletName", null);
                            _wmsParams.Add("@palletSensorId", null);
                            _wmsParams.Add("@binName", null);
                            _wmsParams.Add("@fromBinName", wmsPalletBinInfoInternal.fromBinName);
                            _wmsParams.Add("@fromPalletName", wmsPalletBinInfoInternal.fromPalletName);
                            _wmsParams.Add("@toPalletName", wmsPalletBinInfoInternal.toPalletName);
                            _wmsParams.Add("@toBinName", wmsPalletBinInfoInternal.toBinName);
                            _wmsParams.Add("@comment", null);
                            conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                            foreach (var wmsData in wmsPalletBinInfoInternal.items)
                            {
                                var _wmsParamsDetails = new DynamicParameters();
                                _wmsParamsDetails.Add("@ParentID", activityID);
                                _wmsParamsDetails.Add("@Type", "Item_Movement");
                                _wmsParamsDetails.Add("@fromBinName", null);
                                _wmsParamsDetails.Add("@fromPalletName", null);
                                _wmsParamsDetails.Add("@stockBinId", null);
                                _wmsParamsDetails.Add("@serialNumber", wmsData.serialNumber);
                                _wmsParamsDetails.Add("@skuCode", wmsData.skuCode);
                                _wmsParamsDetails.Add("@batchNumber", wmsData.batchNumber);
                                _wmsParamsDetails.Add("@batchDateTime", wmsData.batchDateTime);
                                _wmsParamsDetails.Add("@status", null);
                                _wmsParamsDetails.Add("@qty", wmsData.qty);
                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataChild, _wmsParamsDetails, commandType: CommandType.StoredProcedure);
                            }

                        }
                        else
                        {
                            // If any condition is not met, rollback the transaction
                            transaction.Rollback();
                            retVal = false;

                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;

                    }
                }
            }
            return retVal;
        }
        public bool InsertItemQRData(ItemsQRActivityModel itemsQRActivity)
        {
            bool retVal = false;
            bool Status1 = false;
            bool Status2 = false;
            string dateTime = null;

            //bool retVal = false;
            Guid activityID = Guid.NewGuid();

            using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
            {
                DateTime TransactionDateTime = DateTime.Now;
                conn.Open();

                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var _params = new DynamicParameters();
                        _params.Add("@ID", activityID);
                        _params.Add("@CustomerID", itemsQRActivity.CustomerID);
                        _params.Add("@ClientDeviceID", itemsQRActivity.ClientDeviceID);
                        _params.Add("@UserID", itemsQRActivity.UserID);
                        _params.Add("@LocationID", itemsQRActivity.LocationID);
                        _params.Add("@PlantID", itemsQRActivity.PlantID);
                        _params.Add("@Count", itemsQRActivity.Count);
                        _params.Add("@StartDateTime", itemsQRActivity.StartDateTime);
                        _params.Add("@EndDateTime", itemsQRActivity.EndDateTime);
                        _params.Add("@TransactionDateTime", itemsQRActivity.TransactionDateTime);
                        _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertItemQrActivity, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
                        Status1 = _params.Get<bool>("@Status");
                        if (Status1)
                        {
                            foreach (var itemsQRActivityDetailsModel in itemsQRActivity.items)
                            {
                              
                                var _paramsActivityDetails = new DynamicParameters();
                                _paramsActivityDetails.Add("@ItemDescription", itemsQRActivityDetailsModel.ItemDescription);

                                string[] parts;
                                if (itemsQRActivityDetailsModel.ItemDescription.Contains(','))
                                {
                                    parts = itemsQRActivityDetailsModel.ItemDescription.Split(',');
                                    if (parts.Length > 1)
                                    {
                                        string itemName = parts[1].TrimStart('0');
                                        string itemSerialNo = parts[0].TrimStart('0');
                                        string batchID = parts[3].TrimStart('0');
                                        string batchDateTime = parts[2];
                                        _paramsActivityDetails.Add("@ItemName", itemName);

                                        _paramsActivityDetails.Add("@ItemSerialNo", itemSerialNo);

                                        _paramsActivityDetails.Add("@BatchID", batchID);
                                    
                                        DateTime date;
                                        
                                        if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            dateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            dateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            dateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            dateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            dateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            dateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        _paramsActivityDetails.Add("@BatchDateTime", dateTime);
                                    }
                                }
                                else
                                {
                                    parts = itemsQRActivityDetailsModel.ItemDescription.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (parts.Length > 1)
                                    {
                                        string itemName = parts[1].TrimStart('0');
                                        string itemSerialNo = parts[0].TrimStart('0');
                                        string batchID = parts[3].TrimStart('0');
                                        string batchDateTime = parts[2];
                                        _paramsActivityDetails.Add("@ItemName", itemName);
                                       
                                        _paramsActivityDetails.Add("@ItemSerialNo", itemSerialNo);
                                        
                                        _paramsActivityDetails.Add("@BatchID", batchID);
                                       
                                        DateTime date;
                                        if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            dateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        // If parsing in "yyyy-MM-dd HH:mm:ss" format fails, try "yyyy-MM-dd" format
                                        else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            dateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        // If parsing in "yyyy-MM-dd" format fails, try "MMM-yyyy" format
                                        else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            dateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            dateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            dateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            dateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        _paramsActivityDetails.Add("@BatchDateTime", dateTime);
                                    }
                                }


                                _paramsActivityDetails.Add("@ParentID", activityID);
                                _paramsActivityDetails.Add("@TransactionDateTime", TransactionDateTime);
                                _paramsActivityDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertItemQrActivityDetails, _paramsActivityDetails, commandType: CommandType.StoredProcedure, transaction: transaction);

                                Status2 = _paramsActivityDetails.Get<bool>("@Status");
                            }
                        }


                        if (Status1 && Status2)
                        {
                            transaction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            // If any condition is not met, rollback the transaction
                            transaction.Rollback();
                            retVal = false;

                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;

                    }
                }
            }
            return retVal;
        }
        public bool InsertDigitizerItemQRData(DigitizerModel digitizerModel)
        {
            bool retVal = false;
            bool Status1 = false;
            bool Status2 = false;
            string dateTime = null;

            //bool retVal = false;
            Guid activityID = Guid.NewGuid();

            using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
            {
                DateTime TransactionDateTime = DateTime.Now;
                conn.Open();

                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var _params = new DynamicParameters();
                        _params.Add("@ID", activityID);
                        _params.Add("@CustomerID", digitizerModel.CustomerID);
                        _params.Add("@ClientDeviceID", digitizerModel.ClientDeviceID);
                        _params.Add("@UserID", digitizerModel.UserID);
                        _params.Add("@LocationID", digitizerModel.LocationID);
                        _params.Add("@PlantID", digitizerModel.PlantID);
                        _params.Add("@Count", digitizerModel.Count);
                        _params.Add("@StartDateTime", digitizerModel.StartDateTime);
                        _params.Add("@EndDateTime", digitizerModel.EndDateTime);
                        _params.Add("@TransactionDateTime", digitizerModel.TransactionDateTime);
                        _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertDigitizerData, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
                        Status1 = _params.Get<bool>("@Status");
                        if (Status1)
                        {
                            foreach (var digitizerModelItems in digitizerModel.items)
                            {

                                var _paramsActivityDetails = new DynamicParameters();
                                _paramsActivityDetails.Add("@ItemDescription", digitizerModelItems.ItemDescription);
                                _paramsActivityDetails.Add("@ItemSerialNo", digitizerModelItems.ItemSerialNo);
                                _paramsActivityDetails.Add("@ItemName", digitizerModelItems.ItemName);
                                _paramsActivityDetails.Add("@ItemCode", digitizerModelItems.ItemCode);
                                _paramsActivityDetails.Add("@BatchID", digitizerModelItems.BatchID);
                                _paramsActivityDetails.Add("@Weight", digitizerModelItems.Weight);
                                string batchDateTime = digitizerModelItems.BatchDateTime;

                                DateTime date;
                                if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                {
                                    dateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                }
                                // If parsing in "yyyy-MM-dd HH:mm:ss" format fails, try "yyyy-MM-dd" format
                                else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out date))
                                {
                                    dateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                }
                                // If parsing in "yyyy-MM-dd" format fails, try "MMM-yyyy" format
                                else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out date))
                                {
                                    dateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                }
                                else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out date))
                                {
                                    dateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                }
                                else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                {
                                    dateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                }
                                else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                {
                                    dateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                }
                                _paramsActivityDetails.Add("@BatchDateTime", dateTime);
                                _paramsActivityDetails.Add("@ParentID", activityID);
                                _paramsActivityDetails.Add("@TransactionDateTime", digitizerModelItems.TransactionDateTime);
                                _paramsActivityDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertDigitizerDataDetails, _paramsActivityDetails, commandType: CommandType.StoredProcedure, transaction: transaction);

                                Status2 = _paramsActivityDetails.Get<bool>("@Status");
                            }
                        }


                        if (Status1 && Status2)
                        {
                            transaction.Commit();
                            retVal = true;
                        }
                        else
                        {
                            // If any condition is not met, rollback the transaction
                            transaction.Rollback();
                            retVal = false;

                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;

                    }
                }
            }
            return retVal;
        }
        public List<BarcodeResponse> GetAllBarcodesForTransaction(Barcode barcode)
        {
            List<BarcodeResponse> data = new List<BarcodeResponse>();
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@ItemDescription", barcode.ItemDescription);
                _params.Add("@ClientDeviceID", barcode.ClientDeviceID);
                data = db.Query<BarcodeResponse>(AppSettings.SQLQueryCommand.SP_GetAllBarcodesForTransaction, _params, commandType: CommandType.StoredProcedure).ToList();
            }
            return data;
        }
        public bool InsertPalletBinTransaction(PalletBinTransModel transModel)
        {
       
            bool retVal = false;
            int stat = 0;
            Guid transID = Guid.NewGuid();
            DateTime serverDateTime = DateTime.Now;

            using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    try
                    {

                        string query = "INSERT INTO PalletBinTransactionTable (TransID, DeviceID, PalletName, PalletTagID, BinName, BinTagID, ServerDateTime) VALUES ('@TransID', '@DeviceID', '@PalletName', '@PalletTagID', '@BinName', '@BinTagID', '@ServerDateTime')";
                        query = query.Replace("@TransID", transID.ToString());
                        query = query.Replace("@DeviceID", transModel.DeviceID);
                        query = query.Replace("@PalletTagID", transModel.PalletTagID);
                        query = query.Replace("@PalletName", transModel.PalletName);
                        query = query.Replace("@BinTagID", transModel.BinTagID);
                        query = query.Replace("@BinName", transModel.BinName);
                        query = query.Replace("@ServerDateTime", serverDateTime.ToString());
                        cmd.CommandText = query;
                        cmd.CommandType = CommandType.Text;
                        stat=  cmd.ExecuteNonQuery();
                        if (stat == 1)
                        {
                            retVal = true;
                        }
                        else
                        {
                            retVal = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        retVal = false;

                    }
                }
            }
            return retVal;
        }
        //public ActivityResponseData InsertTransactionForQRManual(ActivityModelItems activityItems)
        //{
        //    //bool retVal = false;
        //    ActivityResponseData responseData = new ActivityResponseData();
        //    Guid activityID = Guid.NewGuid();
        //    Guid activityDetailsID = Guid.NewGuid();
        //    WMSPalletData wmsPalletData = new WMSPalletData();
        //    WMSPalletDataDispatch wmsPalletDataDis = new WMSPalletDataDispatch();
        //    List<WmsItems> itemsList = new List<WmsItems>();

        //    using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        DateTime TransactionDateTime = DateTime.Now;
        //        conn.Open();
        //        WorkorderCreationItemDetails workorderCreationItemDetails = new WorkorderCreationItemDetails();
        //        using (SqlTransaction transaction = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                var _params = new DynamicParameters();
        //                _params.Add("@ActivityID", activityID);
        //                _params.Add("@ClientDeviceID", activityItems.ClientDeviceID);
        //                _params.Add("@ActivityType", activityItems.ActivityType);
        //                _params.Add("@StartDate", activityItems.StartDate);
        //                _params.Add("@EndDate", activityItems.EndDate);
        //                //_params.Add("@TransactionDateTime", TransactionDateTime);
        //                _params.Add("@CustomerID", activityItems.CustomerID);
        //                _params.Add("@TouchPointID", activityItems.TouchpointID);
        //                _params.Add("@Count", activityItems.Count);
        //                _params.Add("@ParentTagID", activityItems.ParentTagID);
        //                _params.Add("@ParentAssetType", activityItems.ParentAssetType);
        //                _params.Add("@TruckNumber", activityItems.TruckNumber);
        //                _params.Add("@ProcessType", activityItems.ProcessType);
        //                _params.Add("@DRN", activityItems.DRN);
        //                _params.Add("@isUnique", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertActivity, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
        //                workorderCreationItemDetails.TruckNumber = activityItems.TruckNumber;
        //                workorderCreationItemDetails.PalletTagID = activityItems.ParentTagID;
        //                responseData.isUnique = _params.Get<bool>("@isUnique");
        //                if (responseData.isUnique)
        //                {
        //                    foreach (var activityItemDetails in activityItems.data)
        //                    {
        //                        WmsItems items = new WmsItems();
        //                        WmsQRInfo qrInfo = new WmsQRInfo();
        //                        var _paramsActivityDetails = new DynamicParameters();
        //                        _paramsActivityDetails.Add("@ItemDescription", activityItemDetails.ItemDescription);

        //                        string[] parts;
        //                        if (activityItemDetails.ItemDescription.Contains(','))
        //                        {
        //                            parts = activityItemDetails.ItemDescription.Split(',');
        //                            if (parts.Length > 1)
        //                            {
        //                                string itemName = parts[1].TrimStart('0');
        //                                string itemSerialNo = parts[0].TrimStart('0');
        //                                string batchID = parts[3].TrimStart('0');
        //                                string batchDateTime = parts[2];
        //                                _paramsActivityDetails.Add("@ItemName", itemName);
        //                                items.skuCode = itemName;
        //                                qrInfo.skuCode = itemName;

        //                                //_paramsActivityDetails.Add("@ItemSerialNo", itemSerialNo);
        //                                //items.serialNumber = itemSerialNo;
        //                                //qrInfo.serialNumber = itemSerialNo;


        //                                _paramsActivityDetails.Add("@BatchID", batchID);
        //                                items.batchNumber = batchID;
        //                                qrInfo.batchNumber = batchID;

        //                                DateTime date;
        //                                if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                //_paramsActivityDetails.Add("@BatchDateTime", items.batchDateTime);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            parts = activityItemDetails.ItemDescription.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        //                            if (parts.Length > 1)
        //                            {
        //                                string itemName = parts[1].TrimStart('0');
        //                                string itemSerialNo = parts[0].TrimStart('0');
        //                                string batchID = parts[3].TrimStart('0');
        //                                string batchDateTime = parts[2];
        //                                _paramsActivityDetails.Add("@ItemName", itemName);
        //                                items.skuCode = itemName;
        //                                qrInfo.skuCode = itemName;

        //                                //_paramsActivityDetails.Add("@ItemSerialNo", itemSerialNo);
        //                                //items.serialNumber = itemSerialNo;
        //                                //qrInfo.serialNumber = itemSerialNo;

        //                                _paramsActivityDetails.Add("@BatchID", batchID);
        //                                items.batchNumber = batchID;
        //                                qrInfo.batchNumber = batchID;

        //                                DateTime date;
        //                                if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                // If parsing in "yyyy-MM-dd HH:mm:ss" format fails, try "yyyy-MM-dd" format
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                // If parsing in "yyyy-MM-dd" format fails, try "MMM-yyyy" format
        //                                else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture,
        //                                    DateTimeStyles.None, out date))
        //                                {
        //                                    items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

        //                                }
        //                                //_paramsActivityDetails.Add("@BatchDateTime", items.batchDateTime);
        //                            }
        //                        }
        //                        items.qrInfo = qrInfo;
        //                        items.qty = 1.0;
        //                        _paramsActivityDetails.Add("@Qty", activityItemDetails.Qty);
        //                        _paramsActivityDetails.Add("@ActivityID", activityID);
        //                        _paramsActivityDetails.Add("@ProcessType", activityItems.ProcessType);
        //                        _paramsActivityDetails.Add("@DRN", activityItems.DRN);
        //                        _paramsActivityDetails.Add("@TransactionDateTime", TransactionDateTime);
        //                        _paramsActivityDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertActivityDetailsForItems, _paramsActivityDetails, commandType: CommandType.StoredProcedure, transaction: transaction);
        //                        responseData.isItemUnique = _paramsActivityDetails.Get<bool>("@Status");

        //                        //WMS Data Posting
        //                        if (responseData.isItemUnique)
        //                        {
        //                            if (items.skuCode != null)
        //                            {
        //                                for (int i = 0; i < activityItemDetails.Qty; i++)
        //                                {
        //                                    itemsList.Add(items);
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            responseData.message = "";
        //                        }
        //                    }
        //                }

        //                //WMS Data Posting
        //                if (responseData.isUnique && responseData.isItemUnique)
        //                {
        //                    transaction.Commit();
        //                    if (activityItems.ProcessType == "IN")
        //                    {
        //                        if (workorderCreationItemDetails != null)
        //                        {
        //                            InsertWorkorderItemDetailsU0(workorderCreationItemDetails);
        //                        }
        //                        wmsPalletData = GetPalletInfo(activityItems.TruckNumber, activityItems.ParentTagID);
        //                        wmsPalletData.receivingNo = activityItems.DRN;
        //                        wmsPalletData.items = itemsList;
        //                        wmsPalletData.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
        //                        string jsonRec = JsonConvert.SerializeObject(wmsPalletData);
        //                        responseData.wmsData = jsonRec;
        //                        var _wmsParams = new DynamicParameters();
        //                        _wmsParams.Add("@ID", activityID);
        //                        _wmsParams.Add("@DRN", activityItems.DRN);
        //                        _wmsParams.Add("@JSONData", Encoding.UTF8.GetBytes(jsonRec));
        //                        _wmsParams.Add("@ProcessType", "Asset_Pallet_Mapping_Receiving");
        //                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                        //PutPalletInfo(wmsPalletData);

        //                    }
        //                    else if (activityItems.ProcessType == "OUT")
        //                    {
        //                        wmsPalletDataDis = GetPalletInfoDispatch(activityItems.TruckNumber, activityItems.ParentTagID);
        //                        wmsPalletDataDis.dispatchNo = activityItems.DRN;
        //                        wmsPalletDataDis.items = itemsList;
        //                        wmsPalletDataDis.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
        //                        wmsPalletDataDis.comment = "Verified and Loaded on Truck";
        //                        string jsonDis = JsonConvert.SerializeObject(wmsPalletDataDis);
        //                        responseData.wmsData = jsonDis;
        //                        var _wmsParams = new DynamicParameters();
        //                        _wmsParams.Add("@ID", activityID);
        //                        _wmsParams.Add("@DRN", activityItems.DRN);
        //                        _wmsParams.Add("@JSONData", Encoding.UTF8.GetBytes(jsonDis));
        //                        _wmsParams.Add("@ProcessType", "Asset_Pallet_Mapping_Dispatch");
        //                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                        //PutPalletInfoDispatch(wmsPalletDataDis);

        //                    }
        //                }
        //                else
        //                {
        //                    // If any condition is not met, rollback the transaction
        //                    transaction.Rollback();
        //                    if (!responseData.isUnique)
        //                    {
        //                        responseData.message = "The pallet has been already mapped.";
        //                    }
        //                }
        //                responseData.status = true;

        //            }
        //            catch (Exception ex)
        //            {
        //                responseData.status = false;
        //                responseData.message = ex.Message;

        //            }
        //        }
        //    }
        //    return responseData;
        //}
        public ActivityResponseData InsertTransactionForQRManual(ActivityModelItems activityItems)
        {
            //bool retVal = false;
            ActivityResponseData responseData = new ActivityResponseData();
            Guid activityID = Guid.NewGuid();
            Guid activityDetailsID = Guid.NewGuid();
            WMSPalletData wmsPalletData = new WMSPalletData();
            WMSPalletDataDispatch wmsPalletDataDis = new WMSPalletDataDispatch();
            List<WmsItems> itemsList = new List<WmsItems>();

            using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
            {
                DateTime TransactionDateTime = DateTime.Now;
                conn.Open();
                WorkorderCreationItemDetails workorderCreationItemDetails = new WorkorderCreationItemDetails();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var _params = new DynamicParameters();
                        _params.Add("@ActivityID", activityID);
                        _params.Add("@ClientDeviceID", activityItems.ClientDeviceID);
                        _params.Add("@ActivityType", activityItems.ActivityType);
                        _params.Add("@StartDate", activityItems.StartDate);
                        _params.Add("@EndDate", activityItems.EndDate);
                        //_params.Add("@TransactionDateTime", TransactionDateTime);
                        _params.Add("@CustomerID", activityItems.CustomerID);
                        _params.Add("@TouchPointID", activityItems.TouchpointID);
                        _params.Add("@Count", activityItems.Count);
                        _params.Add("@ParentTagID", activityItems.ParentTagID);
                        _params.Add("@ParentAssetType", activityItems.ParentAssetType);
                        _params.Add("@TruckNumber", activityItems.TruckNumber);
                        _params.Add("@ProcessType", activityItems.ProcessType);
                        _params.Add("@DRN", activityItems.DRN);
                        _params.Add("@WarehouseID", activityItems.WarehouseID);
                        _params.Add("@isUnique", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertActivity, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
                        workorderCreationItemDetails.TruckNumber = activityItems.TruckNumber;
                        workorderCreationItemDetails.PalletTagID = activityItems.ParentTagID;
                        responseData.isUnique = _params.Get<bool>("@isUnique");
                        if (responseData.isUnique)
                        {
                            foreach (var activityItemDetails in activityItems.data)
                            {
                                WmsItems items = new WmsItems();
                                WmsQRInfo qrInfo = new WmsQRInfo();
                                var _paramsActivityDetails = new DynamicParameters();
                                _paramsActivityDetails.Add("@ItemDescription", activityItemDetails.ItemDescription);

                                string[] parts;
                                if (activityItemDetails.ItemDescription.Contains(','))
                                {
                                    parts = activityItemDetails.ItemDescription.Split(',');
                                    if (parts.Length > 1)
                                    {
                                        string itemName = parts[1].TrimStart('0');
                                        string itemSerialNo = parts[0].TrimStart('0');
                                        string batchID = parts[3].TrimStart('0');
                                        string batchDateTime = parts[2];
                                        _paramsActivityDetails.Add("@ItemName", itemName);
                                        items.skuCode = itemName;
                                        qrInfo.skuCode = itemName;

                                        //_paramsActivityDetails.Add("@ItemSerialNo", itemSerialNo);
                                        //items.serialNumber = itemSerialNo;
                                        //qrInfo.serialNumber = itemSerialNo;


                                        _paramsActivityDetails.Add("@BatchID", batchID);
                                        items.batchNumber = batchID;
                                        qrInfo.batchNumber = batchID;

                                        DateTime date;
                                        if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        //_paramsActivityDetails.Add("@BatchDateTime", items.batchDateTime);
                                    }
                                }
                                else
                                {
                                    parts = activityItemDetails.ItemDescription.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (parts.Length > 1)
                                    {
                                        string itemName = parts[1].TrimStart('0');
                                        string itemSerialNo = parts[0].TrimStart('0');
                                        string batchID = parts[3].TrimStart('0');
                                        string batchDateTime = parts[2];
                                        _paramsActivityDetails.Add("@ItemName", itemName);
                                        items.skuCode = itemName;
                                        qrInfo.skuCode = itemName;

                                        //_paramsActivityDetails.Add("@ItemSerialNo", itemSerialNo);
                                        //items.serialNumber = itemSerialNo;
                                        //qrInfo.serialNumber = itemSerialNo;

                                        _paramsActivityDetails.Add("@BatchID", batchID);
                                        items.batchNumber = batchID;
                                        qrInfo.batchNumber = batchID;

                                        DateTime date;
                                        if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        // If parsing in "yyyy-MM-dd HH:mm:ss" format fails, try "yyyy-MM-dd" format
                                        else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        // If parsing in "yyyy-MM-dd" format fails, try "MMM-yyyy" format
                                        else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            items.batchDateTime = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        //_paramsActivityDetails.Add("@BatchDateTime", items.batchDateTime);
                                    }
                                }
                                items.qrInfo = qrInfo;
                                items.qty = 1.0;
                                _paramsActivityDetails.Add("@Qty", activityItemDetails.Qty);
                                _paramsActivityDetails.Add("@ActivityID", activityID);
                                _paramsActivityDetails.Add("@ProcessType", activityItems.ProcessType);
                                _paramsActivityDetails.Add("@DRN", activityItems.DRN);
                                _paramsActivityDetails.Add("@WarehouseID", activityItems.WarehouseID);
                                _paramsActivityDetails.Add("@TransactionDateTime", TransactionDateTime);
                                _paramsActivityDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertActivityDetailsForItems, _paramsActivityDetails, commandType: CommandType.StoredProcedure, transaction: transaction);
                                responseData.isItemUnique = _paramsActivityDetails.Get<bool>("@Status");

                                //WMS Data Posting
                                if (responseData.isItemUnique)
                                {
                                    if (items.skuCode != null)
                                    {
                                        for (int i = 0; i < activityItemDetails.Qty; i++)
                                        {
                                            itemsList.Add(items);
                                        }
                                    }
                                }
                                else
                                {
                                    responseData.message = "";
                                }
                            }
                        }

                        //WMS Data Posting
                        if (responseData.isUnique && responseData.isItemUnique)
                        {
                            transaction.Commit();
                            if (activityItems.ProcessType == "IN")
                            {
                                if (workorderCreationItemDetails != null)
                                {
                                    InsertWorkorderItemDetailsU0(workorderCreationItemDetails);
                                }
                                wmsPalletData = GetPalletInfo(activityItems.TruckNumber, activityItems.ParentTagID);
                                wmsPalletData.receivingNo = activityItems.DRN;
                                wmsPalletData.items = itemsList;
                                wmsPalletData.warehouseId = activityItems.WarehouseID.ToString();
                                string jsonRec = JsonConvert.SerializeObject(wmsPalletData);
                                responseData.wmsData = jsonRec;
                           
                                var _wmsParams = new DynamicParameters();
                                _wmsParams.Add("@ID", activityID);
                                _wmsParams.Add("@Type", "ASSET_PALLET_MAPPING_RECEIVING");
                                _wmsParams.Add("@receivingNo", wmsPalletData.receivingNo);
                                _wmsParams.Add("@dispatchNo", null);
                                _wmsParams.Add("@warehouseId", wmsPalletData.warehouseId);
                                _wmsParams.Add("@palletName", wmsPalletData.palletName);
                                _wmsParams.Add("@palletSensorId", activityItems.ParentTagID);
                                _wmsParams.Add("@binName", wmsPalletData.binName);
                                _wmsParams.Add("@fromBinName", null);
                                _wmsParams.Add("@fromPalletName", null);
                                _wmsParams.Add("@toPalletName", null);
                                _wmsParams.Add("@toBinName", null);
                                _wmsParams.Add("@comment", null);
                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                                foreach (var wmsData in wmsPalletData.items)
                                {
                                    var _wmsParamsDetails = new DynamicParameters();
                                    _wmsParamsDetails.Add("@ParentID", activityID);
                                    _wmsParamsDetails.Add("@Type", "ASSET_PALLET_MAPPING_RECEIVING");
                                    _wmsParamsDetails.Add("@fromBinName", null);
                                    _wmsParamsDetails.Add("@fromPalletName", null);
                                    _wmsParamsDetails.Add("@stockBinId", null);
                                    _wmsParamsDetails.Add("@serialNumber", null);
                                    _wmsParamsDetails.Add("@skuCode", wmsData.skuCode);
                                    _wmsParamsDetails.Add("@batchNumber", wmsData.batchNumber);
                                    _wmsParamsDetails.Add("@batchDateTime", wmsData.batchDateTime);
                                    _wmsParamsDetails.Add("@status", null);
                                    _wmsParamsDetails.Add("@qty", wmsData.qty);
                                    conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataChild, _wmsParamsDetails, commandType: CommandType.StoredProcedure);
                                }

                            }
                            else if (activityItems.ProcessType == "OUT")
                            {
                                wmsPalletDataDis = GetPalletInfoDispatch(activityItems.TruckNumber, activityItems.ParentTagID);
                                wmsPalletDataDis.dispatchNo = activityItems.DRN;
                                wmsPalletDataDis.items = itemsList;
                                wmsPalletDataDis.warehouseId = activityItems.WarehouseID.ToString();
                                wmsPalletDataDis.comment = "Verified and Loaded on Truck";
                                string jsonDis = JsonConvert.SerializeObject(wmsPalletDataDis);
                                responseData.wmsData = jsonDis;
                                                               
                                var _wmsParams = new DynamicParameters();
                                _wmsParams.Add("@ID", activityID);
                                _wmsParams.Add("@Type", "ASSET_PALLET_MAPPING_DISPATCH");
                                _wmsParams.Add("@receivingNo", null);
                                _wmsParams.Add("@dispatchNo", wmsPalletDataDis.dispatchNo);
                                _wmsParams.Add("@warehouseId", wmsPalletData.warehouseId);
                                _wmsParams.Add("@palletName", wmsPalletData.palletName);
                                _wmsParams.Add("@palletSensorId", activityItems.ParentTagID);
                                _wmsParams.Add("@binName", wmsPalletData.binName);
                                _wmsParams.Add("@fromBinName", null);
                                _wmsParams.Add("@fromPalletName", null);
                                _wmsParams.Add("@toPalletName", null);
                                _wmsParams.Add("@toBinName", null);
                                _wmsParams.Add("@comment", wmsPalletDataDis.comment);
                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                                foreach (var wmsData in wmsPalletData.items)
                                {
                                    var _wmsParamsDetails = new DynamicParameters();
                                    _wmsParamsDetails.Add("@ParentID", activityID);
                                    _wmsParamsDetails.Add("@Type", "ASSET_PALLET_MAPPING_DISPATCH");
                                    _wmsParamsDetails.Add("@fromBinName", null);
                                    _wmsParamsDetails.Add("@fromPalletName", null);
                                    _wmsParamsDetails.Add("@stockBinId", null);
                                    _wmsParamsDetails.Add("@serialNumber", null);
                                    _wmsParamsDetails.Add("@skuCode", wmsData.skuCode);
                                    _wmsParamsDetails.Add("@batchNumber", wmsData.batchNumber);
                                    _wmsParamsDetails.Add("@batchDateTime", wmsData.batchDateTime);
                                    _wmsParamsDetails.Add("@status", null);
                                    _wmsParamsDetails.Add("@qty", wmsData.qty);
                                    conn.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataChild, _wmsParamsDetails, commandType: CommandType.StoredProcedure);
                                }

                            }
                        }
                        else
                        {
                            // If any condition is not met, rollback the transaction
                            transaction.Rollback();
                            if (!responseData.isUnique)
                            {
                                responseData.message = "The pallet has been already mapped.";
                            }
                        }
                        responseData.status = true;

                    }
                    catch (Exception ex)
                    {
                        responseData.status = false;
                        responseData.message = ex.Message;

                    }
                }
            }
            return responseData;
        }
        public TruckResponseDetails GetDCDetailsForPDA(DCDetails dcTagID)
        {
            TruckResponseDetails data = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@DCTagID", dcTagID.DCTagID);
                data = db.Query<TruckResponseDetails>(AppSettings.SQLQueryCommand.SP_GetDCDetailsForPDA, _params, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            return data;
        }
        public List<SKUResponse> GetSKUDetailsForPDA(SKUDetails sKUDetails)
        {
            List<SKUResponse> data = new List<SKUResponse>();
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@ClientDeviceID", sKUDetails.ClientDeviceID);
                _params.Add("@DCNo", sKUDetails.DCNo);
                _params.Add("@Type", sKUDetails.Type);
                data = db.Query<SKUResponse>(AppSettings.SQLQueryCommand.SP_GetSKUDetailsForPDA, _params, commandType: CommandType.StoredProcedure).ToList();
            }
            return data;
        }
        public List<SKUData> GetAllSKUsPDA()
        {
            List<SKUData> data = new List<SKUData>();
            using (SqlConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                using (SqlCommand cmd = db.CreateCommand())
                {
                    var query = "SELECT skuName AS ItemName, skuCode AS ItemCode FROM SKU WHERE skuName IS NOT NULL";
                    data = db.Query<SKUData>(query).ToList();
                }
            }
            return data;
        }
        public bool GetSOLineItemsLoading(DCDetails dcTagID)
        {
            bool retVal = false;
            string token = GetAuthorizationToken().access_token;
            string dcNo = dcTagID.DCTagID;
            int warehouseID = dcTagID.WarehouseID;
            string url = ConfigurationManager.AppSettings["SOSyncUrl"] + dcNo;
            string urlReturnValue = GetWMSSyncData(url, token, warehouseID);
            DispatchData W = JsonConvert.DeserializeObject<DispatchData>(urlReturnValue);
            if (W != null && W.items.Count != 0)
            {
                retVal = InsetSODetails(W);
            }
            return retVal;
        }
        public List<WarehouseDetails> GetWarehouseDetails()
        {
            List<WarehouseDetails> data = new List<WarehouseDetails>();
            using (SqlConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                using (SqlCommand cmd = db.CreateCommand())
                {
                    var query = "SELECT Name AS WarehouseName, ID AS WarehouseID FROM WarehouseMaster WHERE IsActive = 1";
                    data = db.Query<WarehouseDetails>(query).ToList();
                }
            }
            return data;
        }

        #region PLANT
        public IEnumerable<object> GetDCNumbersForPlant(DeviceData deviceID)
        {
            IEnumerable<object> data = null;
            using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    var query = "SELECT DeliveryNo FROM STO WHERE IsProcessed = 0 ORDER BY ModifiedDateTime DESC";
                    data = conn.Query<string>(query).ToList();
                }
            }
            return data;
        }
        public PlantActivityResponse PlantTransactionForScannedItems(ScannedItemModel activityModel)
        {
            PlantActivityResponse responsedata = new PlantActivityResponse();
            Guid activityID = Guid.NewGuid();
            string batchDateTime1 = string.Empty;

            using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
            {
                DateTime TransactionDateTime = DateTime.Now;
                conn.Open();

                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var _params = new DynamicParameters();
                        _params.Add("@ActivityID", activityID);
                        _params.Add("@ClientDeviceID", activityModel.ClientDeviceID);
                        _params.Add("@ActivityType", activityModel.ActivityType);
                        _params.Add("@StartDate", activityModel.StartDate);
                        _params.Add("@EndDate", activityModel.EndDate);
                        _params.Add("@CustomerID", activityModel.CustomerID);
                        _params.Add("@TouchPointID", activityModel.TouchpointID);
                        _params.Add("@Count", activityModel.Count);
                        _params.Add("@ParentTagID", activityModel.ParentTagID);
                        _params.Add("@DRN", activityModel.DRN);
                        _params.Add("@isUnique", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertActivityForPlant, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
                        responsedata.isUnique = _params.Get<bool>("@isUnique");
                        if (responsedata.isUnique)
                        {
                            foreach (var activityDetailsModel in activityModel.data)
                            {
                               
                                var _paramsActivityDetails = new DynamicParameters();
                                _paramsActivityDetails.Add("@ItemDescription", activityDetailsModel.ItemDescription);

                                string[] parts;
                                if (activityDetailsModel.ItemDescription.Contains(','))
                                {
                                    parts = activityDetailsModel.ItemDescription.Split(',');
                                    if (parts.Length > 1)
                                    {
                                        string itemName = parts[1].TrimStart('0');
                                        string itemSerialNo = parts[0].TrimStart('0');
                                        string batchID = parts[3].TrimStart('0');
                                        string batchDateTime = parts[2];
                                        
                                        _paramsActivityDetails.Add("@ItemName", itemName);
                                        _paramsActivityDetails.Add("@ItemSerialNo", itemSerialNo);
                                        _paramsActivityDetails.Add("@BatchID", batchID);
                                        DateTime date;
                                        if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        _paramsActivityDetails.Add("@BatchDateTime", batchDateTime1);
                                    }
                                }
                                else
                                {
                                    parts = activityDetailsModel.ItemDescription.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (parts.Length > 1)
                                    {
                                        string itemName = parts[1].TrimStart('0');
                                        string itemSerialNo = parts[0].TrimStart('0');
                                        string batchID = parts[3].TrimStart('0');
                                        string batchDateTime = parts[2];
                                        _paramsActivityDetails.Add("@ItemName", itemName);
                                        _paramsActivityDetails.Add("@ItemSerialNo", itemSerialNo);
                                        _paramsActivityDetails.Add("@BatchID", batchID);
                                       
                                        DateTime date;
                                        if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        // If parsing in "yyyy-MM-dd HH:mm:ss" format fails, try "yyyy-MM-dd" format
                                        else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        // If parsing in "yyyy-MM-dd" format fails, try "MMM-yyyy" format
                                        else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        _paramsActivityDetails.Add("@BatchDateTime", batchDateTime1);
                                    }
                                }

                              
                                _paramsActivityDetails.Add("@ActivityID", activityID);
                                _paramsActivityDetails.Add("@DRN", activityModel.DRN);
                                _paramsActivityDetails.Add("@TransactionDateTime", TransactionDateTime);
                                _paramsActivityDetails.Add("@isItemUnique", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertScannedActivityDetailsForPlant, _paramsActivityDetails, commandType: CommandType.StoredProcedure, transaction: transaction);

                                responsedata.isItemUnique = _paramsActivityDetails.Get<bool>("@isItemUnique");

                              
                                if (!responsedata.isItemUnique)
                                {
                                    var duplicateItemDescription = activityDetailsModel.ItemDescription;
                                    if (duplicateItemDescription != null)
                                    {
                                        responsedata.message = $"Item '{duplicateItemDescription}' is already mapped.";
                                    }
                                    break;
                                }
                            }
                        }

                        if (responsedata.isUnique && responsedata.isItemUnique)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            // If any condition is not met, rollback the transaction
                            transaction.Rollback();
                            if (!responsedata.isUnique)
                            {
                                responsedata.message = "The pallet has been already mapped.";
                            }

                        }
                        responsedata.status = true;

                    }
                    catch (Exception ex)
                    {
                        responsedata.status = false;
                        responsedata.message = ex.Message;

                    }
                }
            }
            return responsedata;
        }
        public PlantActivityResponse PlantTransactionForWithoutScannedItems(ManualItemModel activityItems)
        {
            //bool retVal = false;
            PlantActivityResponse responseData = new PlantActivityResponse();
            Guid activityID = Guid.NewGuid();
            

            using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
            {
                DateTime TransactionDateTime = DateTime.Now;
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var _params = new DynamicParameters();
                        _params.Add("@ActivityID", activityID);
                        _params.Add("@ClientDeviceID", activityItems.ClientDeviceID);
                        _params.Add("@ActivityType", activityItems.ActivityType);
                        _params.Add("@StartDate", activityItems.StartDate);
                        _params.Add("@EndDate", activityItems.EndDate);
                        _params.Add("@CustomerID", activityItems.CustomerID);
                        _params.Add("@TouchPointID", activityItems.TouchpointID);
                        _params.Add("@Count", activityItems.Count);
                        _params.Add("@ParentTagID", activityItems.ParentTagID);
                        _params.Add("@DRN", activityItems.DRN);
                        _params.Add("@isUnique", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertActivityForPlant, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
                        responseData.isUnique = _params.Get<bool>("@isUnique");
                        if (responseData.isUnique)
                        {
                            foreach (var activityItemDetails in activityItems.data)
                            {
                                var _paramsActivityDetails = new DynamicParameters();

                                var ItemDesc = activityItemDetails.ItemDescription.Split(',');
                                _paramsActivityDetails.Add("@ItemDescription", ItemDesc[0]);
                                _paramsActivityDetails.Add("@ItemName", ItemDesc[2]);
                                _paramsActivityDetails.Add("@BatchID", ItemDesc[1]);
                                _paramsActivityDetails.Add("@Qty", activityItemDetails.Qty);
                                _paramsActivityDetails.Add("@ActivityID", activityID);
                                _paramsActivityDetails.Add("@TransactionDateTime", TransactionDateTime);
                                _paramsActivityDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertManualActivityDetailsForPlant, _paramsActivityDetails, commandType: CommandType.StoredProcedure, transaction: transaction);
                                responseData.isItemUnique = _paramsActivityDetails.Get<bool>("@Status");

                            }
                        }

                        //WMS Data Posting
                        if (responseData.isUnique && responseData.isItemUnique)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            // If any condition is not met, rollback the transaction
                            transaction.Rollback();
                            if (!responseData.isUnique)
                            {
                                responseData.message = "The pallet has been already mapped.";
                            }
                        }
                        responseData.status = true;

                    }
                    catch (Exception ex)
                    {
                        responseData.status = false;
                        responseData.message = ex.Message;

                    }
                }
            }
            return responseData;
        }
        public PlantActivityResponse PlantTransactionForQRManualItems(ManualItemModel activityItems)
        {
            //bool retVal = false;
            PlantActivityResponse responseData = new PlantActivityResponse();
            Guid activityID = Guid.NewGuid();
            string batchDateTime1 = string.Empty;

            using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
            {
                DateTime TransactionDateTime = DateTime.Now;
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        var _params = new DynamicParameters();
                        _params.Add("@ActivityID", activityID);
                        _params.Add("@ClientDeviceID", activityItems.ClientDeviceID);
                        _params.Add("@ActivityType", activityItems.ActivityType);
                        _params.Add("@StartDate", activityItems.StartDate);
                        _params.Add("@EndDate", activityItems.EndDate);
                        _params.Add("@CustomerID", activityItems.CustomerID);
                        _params.Add("@TouchPointID", activityItems.TouchpointID);
                        _params.Add("@Count", activityItems.Count);
                        _params.Add("@ParentTagID", activityItems.ParentTagID);
                        _params.Add("@DRN", activityItems.DRN);
                        _params.Add("@isUnique", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                        conn.Execute(AppSettings.SQLQueryCommand.SP_InsertActivityForPlant, _params, commandType: CommandType.StoredProcedure, transaction: transaction);
                        responseData.isUnique = _params.Get<bool>("@isUnique");
                        if (responseData.isUnique)
                        {
                            foreach (var activityItemDetails in activityItems.data)
                            {
                                var _paramsActivityDetails = new DynamicParameters();
                                _paramsActivityDetails.Add("@ItemDescription", activityItemDetails.ItemDescription);

                                string[] parts;
                                if (activityItemDetails.ItemDescription.Contains(','))
                                {
                                    parts = activityItemDetails.ItemDescription.Split(',');
                                    if (parts.Length > 1)
                                    {
                                        string itemName = parts[1].TrimStart('0');
                                        string itemSerialNo = parts[0].TrimStart('0');
                                        string batchID = parts[3].TrimStart('0');
                                        string batchDateTime = parts[2];
                                        _paramsActivityDetails.Add("@ItemName", itemName);
                                        _paramsActivityDetails.Add("@BatchID", batchID);

                                        DateTime date;
                                        if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        //_paramsActivityDetails.Add("@BatchDateTime", batchDateTime1);
                                    }
                                }
                                else
                                {
                                    parts = activityItemDetails.ItemDescription.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (parts.Length > 1)
                                    {
                                        string itemName = parts[1].TrimStart('0');
                                        string itemSerialNo = parts[0].TrimStart('0');
                                        string batchID = parts[3].TrimStart('0');
                                        string batchDateTime = parts[2];
                                        _paramsActivityDetails.Add("@ItemName", itemName);
                                        _paramsActivityDetails.Add("@BatchID", batchID);
                                        DateTime date;
                                        if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "MMM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "yyyy-MM", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = new DateTime(date.Year, date.Month, 1).ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        else if (DateTime.TryParseExact(batchDateTime, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture,
                                            DateTimeStyles.None, out date))
                                        {
                                            batchDateTime1 = date.ToString("yyyy-MM-ddTHH:mm:ssZ");

                                        }
                                        //_paramsActivityDetails.Add("@BatchDateTime", items.batchDateTime);
                                    }
                                }
                                _paramsActivityDetails.Add("@Qty", activityItemDetails.Qty);
                                _paramsActivityDetails.Add("@ActivityID", activityID);
                                _paramsActivityDetails.Add("@TransactionDateTime", TransactionDateTime);
                                _paramsActivityDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                                conn.Execute(AppSettings.SQLQueryCommand.SP_InsertManualActivityDetailsForPlant, _paramsActivityDetails, commandType: CommandType.StoredProcedure, transaction: transaction);
                                responseData.isItemUnique = _paramsActivityDetails.Get<bool>("@Status");

                            }
                        }

                        //WMS Data Posting
                        if (responseData.isUnique && responseData.isItemUnique)
                        {
                            transaction.Commit();
                        }
                        else
                        {
                            // If any condition is not met, rollback the transaction
                            transaction.Rollback();
                            if (!responseData.isUnique)
                            {
                                responseData.message = "The pallet has been already mapped.";
                            }
                        }
                        responseData.status = true;

                    }
                    catch (Exception ex)
                    {
                        responseData.status = false;
                        responseData.message = ex.Message;

                    }
                }
            }
            return responseData;
        }
        #endregion

        #endregion
        #region Report
        public List<QRTranscationDetails> GetItemsQRActivityDetails(int LocationId)
        {
            List<QRTranscationDetails> TranscationData = new List<QRTranscationDetails>();
            try
            {
                using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
                {
                    db.Open();

                    var _params = new DynamicParameters();
                    _params.Add("@LocationId", LocationId);
                    var transcationNumbers = db.Query<ItemsQRActivity>(AppSettings.SQLQueryCommand.SP_GET_TranscationNoQR, _params, commandType: CommandType.StoredProcedure).ToList();


                    foreach (var transNo in transcationNumbers)
                    {
                        QRTranscationDetails qR = new QRTranscationDetails();

                        var palletDetails = GetQRDetails(transNo.Id.ToString());
                        qR.TranscationId = transNo.Id.ToString();
                        qR.TranscationNo = transNo.StartDateTime.ToString("ddMMyyyHHmmss");
                        transNo.StartDateTimeSTR = transNo.StartDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                        transNo.EndDateTimeSTR = transNo.EndDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                        transNo.ServerDateTimeSTR = transNo.ServerDateTime.ToString("dd-MM-yyyy HH:mm:ss");
                        qR.activity = transNo;
                        qR.data = palletDetails;

                        TranscationData.Add(qR);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return TranscationData;
        }

        public List<LocationDetails> GetQRLocationName()
        {

            List<LocationDetails> data = new List<LocationDetails>()
            {
                new LocationDetails() { LocationName = "1"},
                new LocationDetails() { LocationName = "2" },
                new LocationDetails() { LocationName = "3"},
                new LocationDetails() { LocationName = "4"},
                new LocationDetails() { LocationName = "5" }
            };
            return data;
            //return null;
        }


        private List<ItemsQRActivityDetails> GetQRDetails(string transNo)
        {
            List<ItemsQRActivityDetails> items = new List<ItemsQRActivityDetails>();

            try
            {
                using (SqlConnection conn = new SqlConnection(AppSettings.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        string sqlStr = "SELECT * FROM ItemsQrActivityDetails WHERE ParentActivityId='@TranscationId' ORDER BY ServerDateTime DESC";
                        sqlStr = sqlStr.Replace("@TranscationId", transNo);
                        cmd.CommandText = sqlStr;
                        cmd.CommandType = System.Data.CommandType.Text;
                        using (PSLDataReader dr = new PSLDataReader(cmd.ExecuteReader()))
                        {
                            // transcations = new List<TranscationDetails>();
                            while (dr.Read())
                            {

                                ItemsQRActivityDetails itemsQR = new ItemsQRActivityDetails();
                                itemsQR.Id = dr.GetGuid("Id").ToString();
                                itemsQR.ParentActivityId = dr.GetGuid("ParentActivityId").ToString();
                                itemsQR.ItemDescription = dr.GetString("ItemDescription");
                                itemsQR.ItemSerialNo = dr.GetInt32("ItemSerialNo").ToString();
                                itemsQR.ItemCode = dr.GetString("ItemCode");
                                itemsQR.BatchId = dr.GetString("BatchId");
                                itemsQR.BatchDateTime = dr.GetString("BatchDateTime");
                                itemsQR.TransactionDateTime = dr.GetDateTime("TransactionDateTime");
                                itemsQR.Serverdatetime = dr.GetDateTime("Serverdatetime");
                                itemsQR.ServerDateTimeSTR = itemsQR.Serverdatetime.ToString("dd-MM-yyyy HH:mm:ss");
                                items.Add(itemsQR);
                            }
                        }
                    }
                }


                //return transcations;
            }
            catch (Exception ex)
            {
                throw new Exception("Error While GetQRDetails : ", ex);
            }

            return items;

        }
        #endregion
        #region TV
        public List<LocationDetails> GetLocationName()
        {
            List<LocationDetails> data = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                data = db.Query<LocationDetails>(AppSettings.SQLQueryCommand.SP_GetLocationName, _params, commandType: CommandType.StoredProcedure).ToList();

            }
            return data;
            //return null;
        }
        public List<TruckResponseDetailsByLocation> GetPalletItemDetailsByLocationName(LocationDetails locationDetails)
        {
            List<TruckResponseDetailsByLocation> data = new List<TruckResponseDetailsByLocation>();

            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();

                var _params = new DynamicParameters();
                _params.Add("@LocationName", locationDetails.LocationName);
                var truckDetails = db.Query<TruckResponseDetailsByLocation>(AppSettings.SQLQueryCommand.SP_GetTruckDetailsByLocationName, _params, commandType: CommandType.StoredProcedure).ToList();


                foreach (var trucks in truckDetails)
                {
                    List<PalletItemDetails> palletItemDetails = new List<PalletItemDetails>();
                    List<ItemDetails> MegaItemsList = new List<ItemDetails>();

                    var palletDetails = db.Query<PalletItemDetails>(AppSettings.SQLQueryCommand.SP_GetPalletDetailsWithTruckID, new { TruckID = trucks.TruckNumber }, commandType: CommandType.StoredProcedure).ToList();

                    foreach (var pallet in palletDetails)
                    {
                        var itemDetails = db.Query<ItemDetails>(AppSettings.SQLQueryCommand.SP_GetPalletWithItemDetails, new { TruckNumber = trucks.TruckNumber, PalletName = pallet.PalletName }, commandType: CommandType.StoredProcedure).ToList();

                        List<ItemDetails> itemsdetails1 = new List<ItemDetails>();

                        var distinctWithCounts = itemDetails.Select(itemD => itemD.ItemCode).GroupBy(x => x).Select(g => new { Name = g.Key, Count = g.Count() });
                        foreach (var item in distinctWithCounts)
                        {
                            ItemDetails item1 = new ItemDetails();
                            item1.ItemCode = item.Name;
                            item1.Qty = item.Count;

                            for (int i = 0; i < itemDetails.Count; i++)
                            {
                                if (item.Name == itemDetails[i].ItemCode && itemDetails[i].Qty != 0)
                                {
                                    item1.Qty = item1.Qty + itemDetails[i].Qty - 1;
                                }

                                if (i == itemDetails.Count - 1)
                                {
                                    itemsdetails1.Add(item1);
                                    MegaItemsList.Add(item1);
                                }
                            }


                            //Console.WriteLine($"{item.Name}: {item.Count}");
                        }



                        var palletItem = new PalletItemDetails
                        {
                            PalletName = pallet.PalletName,
                            ItemDetails = itemsdetails1.ToList()
                        };

                        palletItemDetails.Add(palletItem);
                    }
                    trucks.ItemDetailSummary = ItemsSummary(MegaItemsList);
                    trucks.PalletItemDetails = palletItemDetails;
                    data.Add(trucks);
                }
            }

            return data;
        }
        //Generating ItemsSummary
        public List<ItemDetails> ItemsSummary(List<ItemDetails> totalItems)
        {
            List<ItemDetails> ItemsSummary = new List<ItemDetails>();

            var SumdistinctWithCounts = totalItems.Select(itemD => itemD.ItemCode).GroupBy(x => x).Select(g => new { Name = g.Key, Count = g.Count() });
            foreach (var itemSum in SumdistinctWithCounts)
            {
                ItemDetails item1 = new ItemDetails();
                item1.ItemCode = itemSum.Name;
                int Qty = 0;

                for (int i = 0; i < totalItems.Count; i++)
                {
                    if (itemSum.Name == totalItems[i].ItemCode)
                    {
                        item1.Qty = Qty + totalItems[i].Qty;
                        Qty = item1.Qty;
                    }

                    if (i == totalItems.Count - 1)
                    {
                        ItemsSummary.Add(item1);

                    }
                }


                //Console.WriteLine($"{item.Name}: {item.Count}");
            }
            return ItemsSummary;
        }

        #endregion

        #region Workorder Creation
        //U0 Creation
        public bool InsertWorkorderDetailsU0(WorkorderCreationDetails workorderCreation)
        {
            bool retval = false;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                Guid obj = Guid.NewGuid();

                if (workorderCreation.TruckNumber != null)
                {
                    var _params = new DynamicParameters();
                    _params.Add("@WorkorderID", obj);
                    _params.Add("@TruckID", workorderCreation.TruckNumber);
                    _params.Add("@WarehouseID", workorderCreation.WarehouseID);
                    _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    db.Execute(AppSettings.SQLQueryCommand.SP_CreateWorkOrderU0, _params, commandType: CommandType.StoredProcedure);
                    retval = _params.Get<bool>("@Status");
                }
            }
            return retval;
        }
        private bool InsertWorkorderItemDetailsU0(WorkorderCreationItemDetails workorderItems)
        {
            bool retval = false;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();

                if (workorderItems.TruckNumber != null)
                {
                    var _params = new DynamicParameters();
                    _params.Add("@TruckID", workorderItems.TruckNumber);
                    _params.Add("@PalletTagID", workorderItems.PalletTagID);
                    _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    db.Execute(AppSettings.SQLQueryCommand.SP_CreateWorkOrderDetailsU0, _params, commandType: CommandType.StoredProcedure);
                    retval = _params.Get<bool>("@Status");
                }
            }
            return retval;
        }

        //U1 Creation
        public bool receivingWorkOrder(WorkorderCreationDetailsU1 workorderCreation)
        {
            bool retval = false;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                Guid obj = Guid.NewGuid();
                String workorderType = "U1";

                if (workorderCreation != null)
                {
                    var _params = new DynamicParameters();
                    _params.Add("@WorkorderID", obj);
                    _params.Add("@WorkorderType", workorderType);
                    _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    db.Execute(AppSettings.SQLQueryCommand.SP_CreateWorkOrderU1, _params, commandType: CommandType.StoredProcedure);

                    foreach (WorkorderCreationItemDetailsU1 i in workorderCreation.data)
                    {
                        var _paramsWorkOrderDetails = new DynamicParameters();
                        _paramsWorkOrderDetails.Add("@WorkorderID", obj);
                        _paramsWorkOrderDetails.Add("@PalletName", i.palletName);
                        _paramsWorkOrderDetails.Add("@BinName", i.toBinName);
                        _paramsWorkOrderDetails.Add("@ReceivingNo", i.receivingNo);
                        _paramsWorkOrderDetails.Add("@WarehouseID", i.warehouseId);
                        _paramsWorkOrderDetails.Add("@IsU1", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                        db.Execute(AppSettings.SQLQueryCommand.SP_CreateWorkOrderDetailsU1, _paramsWorkOrderDetails, commandType: CommandType.StoredProcedure);


                        bool isU1Required = _paramsWorkOrderDetails.Get<bool>("@IsU1");
                        if (!isU1Required)
                        {
                            using (SqlConnection db1 = new SqlConnection(AppSettings.ConnectionString))
                            {
                                string updateQuery = "UPDATE WorkOrder SET WorkOrderStatus = 'C', IsActive = 0 WHERE ProcessInfo1 = @receivingNo AND WorkOrderType = 'U1' AND WorkOrderStatus = 'A' AND IsActive = 1";

                                db1.Open();

                                using (SqlCommand updateCommand = new SqlCommand(updateQuery, db1))
                                {
                                    updateCommand.Parameters.AddWithValue("@receivingNo", i.receivingNo);
                                    int rowsAffected = updateCommand.ExecuteNonQuery(); 
                                }

                            }

                            Guid TransID = Guid.NewGuid();
                            var _wmsParams = new DynamicParameters();
                            _wmsParams.Add("@ID", TransID);
                            _wmsParams.Add("@Type", "Pallet_Movement_U0U1");
                            _wmsParams.Add("@receivingNo", i.receivingNo);
                            _wmsParams.Add("@dispatchNo", null);
                            _wmsParams.Add("@warehouseId", i.warehouseId);
                            _wmsParams.Add("@palletName", null);
                            _wmsParams.Add("@palletSensorId", null);
                            _wmsParams.Add("@binName", null);
                            _wmsParams.Add("@fromBinName", i.fromBinName);
                            _wmsParams.Add("@fromPalletName", null);
                            _wmsParams.Add("@toPalletName", null);
                            _wmsParams.Add("@toBinName", i.toBinName);
                            _wmsParams.Add("@comment", null);
                            db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                        }

                    }

                    retval = _params.Get<bool>("@Status");
                }
            }
            return retval;
        }
        //L0 Creation
        public bool dispatchWorkOrder(WorkorderCreationDetailsL0 workorderCreationL0)
        {
            bool retval = false;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                Guid obj = Guid.NewGuid();
                String workorderType = "L0";

                if (workorderCreationL0 != null)
                {
                    var _params = new DynamicParameters();
                    _params.Add("@WorkorderID", obj);
                    _params.Add("@WorkorderType", workorderType);
                    _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    db.Execute(AppSettings.SQLQueryCommand.SP_CreateWorkOrderL0, _params, commandType: CommandType.StoredProcedure);

                    foreach (WorkorderCreationItemDetailsL0 i in workorderCreationL0.data)
                    {
                        var _paramsWorkOrderDetails = new DynamicParameters();
                        _paramsWorkOrderDetails.Add("@WorkorderID", obj);
                        _paramsWorkOrderDetails.Add("@PalletName", i.palletName);
                        _paramsWorkOrderDetails.Add("@ItemDescription", i.skuName);
                        _paramsWorkOrderDetails.Add("@ItemName", i.skuCode);
                        _paramsWorkOrderDetails.Add("@BatchID", i.batchNumber);
                        _paramsWorkOrderDetails.Add("@BinName", i.fromBinName);
                        _paramsWorkOrderDetails.Add("@DispatchNo", i.dispatchNo);
                        _paramsWorkOrderDetails.Add("@Qty", i.qty);
                        _paramsWorkOrderDetails.Add("@IsFull", i.isFull);
                        _paramsWorkOrderDetails.Add("@WarehouseID", i.warehouseId);
                        _paramsWorkOrderDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                        db.Execute(AppSettings.SQLQueryCommand.SP_CreateWorkOrderDetailsL0, _paramsWorkOrderDetails, commandType: CommandType.StoredProcedure);


                        retval = _paramsWorkOrderDetails.Get<bool>("@Status");
                    }


                    retval = _params.Get<bool>("@Status");
                }
            }
            return retval;
        }
        //L1 Creation
        //public bool InsertWorkorderDetailsL1(WorkorderCreationDetails workorderCreation)
        //{
        //    bool retval = false;
        //    List<PalletDetails> details = new List<PalletDetails>();
        //    using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        Guid obj = Guid.NewGuid();

        //        if (workorderCreation.TruckNumber != null)
        //        {
        //            var _params = new DynamicParameters();
        //            _params.Add("@WorkorderID", obj);
        //            _params.Add("@TruckID", workorderCreation.TruckNumber);
        //            details = db.Query<PalletDetails>(AppSettings.SQLQueryCommand.SP_CreateWorkOrderL1, _params, commandType: CommandType.StoredProcedure).ToList();

        //            foreach (PalletDetails i in details)
        //            {
        //                var _paramsPalletDetails = new DynamicParameters();
        //                _paramsPalletDetails.Add("@WorkorderID", obj);
        //                _paramsPalletDetails.Add("@PalletName", i.PalletName);
        //                _paramsPalletDetails.Add("@PalletTagID", i.PalletTagID);
        //                _paramsPalletDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //                db.Execute(AppSettings.SQLQueryCommand.SP_CreateWorkOrderDetailsL1, _paramsPalletDetails, commandType: CommandType.StoredProcedure);

        //                retval = _paramsPalletDetails.Get<bool>("@Status");
        //            }
                        
        //        }
        //    }
        //    return retval;
        //}
        public bool InsertWorkorderDetailsL1(WorkorderCreationDetails workorderCreation)
        {
            bool retval = false;
            List<PalletDetails> details = new List<PalletDetails>();
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                Guid obj = Guid.NewGuid();

                if (workorderCreation.TruckNumber != null)
                {
                    var _params = new DynamicParameters();
                    _params.Add("@WorkorderID", obj);
                    _params.Add("@TruckID", workorderCreation.TruckNumber);
                    db.Execute(AppSettings.SQLQueryCommand.SP_CreateWorkOrderL1, _params, commandType: CommandType.StoredProcedure);
                    string query = "SELECT DRN FROM ActivityHeader WHERE TruckID = @TruckID AND IsProcessed = 0";
                    string dispatchNo = db.Query<string>(query, new { TruckID = workorderCreation.TruckNumber }).FirstOrDefault();
                    string[] DRNValues;
                  
                    if (dispatchNo.Contains(','))
                     {
                        DRNValues = dispatchNo.Split(',');
                        if (DRNValues.Length > 1)
                        {
                            foreach(string drn in DRNValues)
                            {
                                string query1 = "SELECT WD.PalletName AS PalletName, WD.PalletTagID AS PalletTagID FROM WorkOrder W, WorkOrderDetails WD WHERE WD.WorkOrderID = W.WorkOrderID AND W.WorkOrderType = 'L0' AND WD.Status = 'Completed' AND W.ProcessInfo1 = @DRN";
                                 details = db.Query<PalletDetails>(query1, new { DRN = drn }).ToList();
                                
                                foreach (PalletDetails i in details)
                                {
                                    var _paramsPalletDetails = new DynamicParameters();
                                    _paramsPalletDetails.Add("@WorkorderID", obj);
                                    _paramsPalletDetails.Add("@PalletName", i.PalletName);
                                    _paramsPalletDetails.Add("@PalletTagID", i.PalletTagID);
                                    _paramsPalletDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                                    db.Execute(AppSettings.SQLQueryCommand.SP_CreateWorkOrderDetailsL1, _paramsPalletDetails, commandType: CommandType.StoredProcedure);

                                    retval = _paramsPalletDetails.Get<bool>("@Status");
                                    
                                }
                               
                                db.Execute("UPDATE WorkOrder SET IsActive = 0, WorkOrderStatus = 'C' WHERE WorkOrderType = 'L0' AND ProcessInfo1 = @DRN AND IsActive = 1 AND WorkOrderStatus = 'A'", new { DRN = drn });
                            }
                           
                        }
                    }
                    else
                    {
                        string query2 = "SELECT WD.PalletName AS PalletName, WD.PalletTagID AS PalletTagID FROM WorkOrder W, WorkOrderDetails WD WHERE WD.WorkOrderID = W.WorkOrderID AND W.WorkOrderType = 'L0' AND WD.Status = 'Completed' AND W.ProcessInfo1 = @DRN";
                        details = db.Query<PalletDetails>(query2, new { DRN = dispatchNo }).ToList();

                        foreach (PalletDetails i in details)
                        {
                            var _paramsPalletDetails = new DynamicParameters();
                            _paramsPalletDetails.Add("@WorkorderID", obj);
                            _paramsPalletDetails.Add("@PalletName", i.PalletName);
                            _paramsPalletDetails.Add("@PalletTagID", i.PalletTagID);
                            _paramsPalletDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                            db.Execute(AppSettings.SQLQueryCommand.SP_CreateWorkOrderDetailsL1, _paramsPalletDetails, commandType: CommandType.StoredProcedure);

                            retval = _paramsPalletDetails.Get<bool>("@Status");

                        }
                        db.Execute("UPDATE WorkOrder SET IsActive = 0, WorkOrderStatus = 'C' WHERE WorkOrderType = 'L0' AND ProcessInfo1 = @DRN AND IsActive = 1 AND WorkOrderStatus = 'A'", new { DRN = dispatchNo });
                    }

                }
            }
            return retval;
        }
        //ClosedU0
        public bool CloseWorkorderU0(CloseWorkorderRequest request)
        {
            bool isClosed = false;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@TruckNumber", request.TruckNumber);
                _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                db.Execute(AppSettings.SQLQueryCommand.SP_CloseWorkorderU0, _params, commandType: CommandType.StoredProcedure);
                isClosed = _params.Get<bool>("@Status");
            }
            return isClosed;
        }
        //InternalWorkorderCreation
        public bool internalWorkOrder(WorkorderCreationDetailsI0 workorderCreation)
        {
            bool retval = false;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                Guid obj = Guid.NewGuid();
                String workorderType = "I0";

                if (workorderCreation != null)
                {
                    var _params = new DynamicParameters();
                    _params.Add("@WorkorderID", obj);
                    _params.Add("@WorkorderType", workorderType);
                    _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    db.Execute(AppSettings.SQLQueryCommand.SP_CreateWorkOrderI0, _params, commandType: CommandType.StoredProcedure);

                    foreach (WorkorderCreationItemDetailsI0 i in workorderCreation.data)
                    {
                        var _paramsWorkOrderDetails = new DynamicParameters();
                        _paramsWorkOrderDetails.Add("@WorkorderID", obj);
                        _paramsWorkOrderDetails.Add("@PalletName", i.palletName);
                        _paramsWorkOrderDetails.Add("@toBinName", i.toBinName);
                        _paramsWorkOrderDetails.Add("@fromBinName", i.fromBinName);
                        _paramsWorkOrderDetails.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                        db.Execute(AppSettings.SQLQueryCommand.SP_CreateWorkOrderDetailsI0, _paramsWorkOrderDetails, commandType: CommandType.StoredProcedure);


                        retval = _paramsWorkOrderDetails.Get<bool>("@Status");
                    }


                    retval = _params.Get<bool>("@Status");
                }
            }
            return retval;
        }
        #endregion
        #region BotService
        public List<PendingWorkOrderDetails> GetPendingWorkorderDetails()
        {
            List<PendingWorkOrderDetails> data = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                data = db.Query<PendingWorkOrderDetails>(AppSettings.SQLQueryCommand.SP_GetPendingWorkorderDetails, _params, commandType: CommandType.StoredProcedure).ToList();

            }
            return data;
        }
        //public bool InsertTransactionDetailsForBotService(TransactionDetailsV1 transactionDetails)
        //{
        //    bool retval = false;


        //    using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        var _params = new DynamicParameters();

        //        if (transactionDetails.TransID != null && transactionDetails.SubTagDetails != null && transactionDetails.SubTagDetails.Count > 0)
        //        {
        //            _params.Add("@TransID", transactionDetails.TransID);
        //            _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
        //            _params.Add("@PalletTagID", transactionDetails.PalletTagID);
        //            _params.Add("@CategoryID", transactionDetails.CategoryID);
        //            _params.Add("@AntennaID", transactionDetails.AntennaID);
        //            _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
        //            _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
        //            _params.Add("@WorkorderType", transactionDetails.WorkorderType);
        //            _params.Add("@Count", transactionDetails.Count);
        //            _params.Add("@RSSI", transactionDetails.RSSI);
        //            _params.Add("@TransDatetime", transactionDetails.TransDatetime);
        //            _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //            db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTransLogForService, _params, commandType: CommandType.StoredProcedure);
        //            retval = _params.Get<bool>("@Status");

        //            if (transactionDetails.SubTagDetails != null && transactionDetails.SubTagDetails.Count != 0)
        //            {
        //                foreach (SubTagDataV1 i in transactionDetails.SubTagDetails)
        //                {
        //                    WmsPalletBinInfo wmsPalletBin = new WmsPalletBinInfo();
        //                    WmsPalletBinInfoL0 wmsPalletBinInfoL0 = new WmsPalletBinInfoL0();
        //                    WmsPalletBinInfoL1 wmsPalletBinInfoL1 = new WmsPalletBinInfoL1();
        //                    List<WmsPalletBinInfoInternalDetails> wmsPalletBinInfoI0List = new List<WmsPalletBinInfoInternalDetails>();
        //                    WmsPalletBinInfoInternalDetails wmsPalletBinInfoI0 = new WmsPalletBinInfoInternalDetails();
        //                    WmsPalletBinInfoInternal wmsPalletBinInfoInternal = new WmsPalletBinInfoInternal();
        //                    _params.Add("@TransID", transactionDetails.TransID);
        //                    _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
        //                    _params.Add("@PalletTagID", transactionDetails.PalletTagID);
        //                    _params.Add("@CategoryID", transactionDetails.CategoryID);
        //                    _params.Add("@AntennaID", transactionDetails.AntennaID);
        //                    _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
        //                    _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
        //                    _params.Add("@WorkorderType", transactionDetails.WorkorderType);
        //                    _params.Add("@Count", transactionDetails.Count);
        //                    _params.Add("@RSSI", transactionDetails.RSSI);
        //                    _params.Add("@TransDatetime", transactionDetails.TransDatetime);

        //                    _params.Add("@SubTransID", transactionDetails.TransID);
        //                    _params.Add("@SuggestedDestinationTagID", i.SuggestedDestinationTagID);
        //                    _params.Add("@ActualDestinationTagID", i.ActualDestinationTagID);
        //                    _params.Add("@SubRSSI", i.RSSI);
        //                    _params.Add("@SubCount", i.Count);
        //                    _params.Add("@SubCategoryID", i.CategoryID);
        //                    _params.Add("@TagType", i.TagType);
        //                    _params.Add("@SubTransDatetime", i.TransDatetime);
        //                    _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTranslogitemsForService, _params, commandType: CommandType.StoredProcedure);
        //                    retval = _params.Get<bool>("@Status");

        //                    if (retval)
        //                    {
        //                        if (transactionDetails.WorkorderType == "U0" || transactionDetails.WorkorderType == "U1")
        //                        {
        //                            wmsPalletBin = GetPalletBinMappedInfo(transactionDetails.PalletTagID, transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, i.ActualDestinationTagID);
        //                            wmsPalletBin.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
        //                            wmsPalletBin.palletSensorId = transactionDetails.PalletTagID;
        //                            string jsonData = JsonConvert.SerializeObject(wmsPalletBin);
        //                            var _wmsParams = new DynamicParameters();
        //                            _wmsParams.Add("@ID", transactionDetails.TransID);
        //                            _wmsParams.Add("@DRN", wmsPalletBin.receivingNo);
        //                            _wmsParams.Add("@JSONData", Encoding.UTF8.GetBytes(jsonData));
        //                            _wmsParams.Add("@ProcessType", "Pallet_Movement_U0U1");
        //                            db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                            //PutPalletBinMappedInfo(wmsPalletBin);
        //                        }
        //                        else if (transactionDetails.WorkorderType == "L0")
        //                        {
        //                            wmsPalletBinInfoL0 = GetPalletBinMappedInfoL0(transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, transactionDetails.PalletTagID, i.ActualDestinationTagID);
        //                            string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoL0);
        //                            var _wmsParams = new DynamicParameters();
        //                            _wmsParams.Add("@ID", transactionDetails.TransID);
        //                            _wmsParams.Add("@DRN", wmsPalletBinInfoL0.dispatchNo);
        //                            _wmsParams.Add("@JSONData", Encoding.UTF8.GetBytes(jsonData));
        //                            _wmsParams.Add("@ProcessType", "Pallet_Movement_L0");
        //                            db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                            //PutPalletBinMappedInfoL0(wmsPalletBinInfoL0);
        //                        }
        //                        else if (transactionDetails.WorkorderType == "L1")
        //                        {
        //                            wmsPalletBinInfoL1 = GetPalletBinMappedInfoL1(transactionDetails.PalletTagID, transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, i.ActualDestinationTagID);
        //                            wmsPalletBinInfoL1.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
        //                            wmsPalletBinInfoL1.palletSensorId = transactionDetails.PalletTagID;
        //                            string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoL1);
        //                            var query = "SELECT ProcessInfo1 FROM WorkOrder WHERE WONumber = @WorkOrderNumber";
        //                            var dispatchNo = db.Query<string>(query, new { WorkorderNumber = transactionDetails.WorkorderNumber });
        //                            var _wmsParams = new DynamicParameters();
        //                            _wmsParams.Add("@ID", transactionDetails.TransID);
        //                            _wmsParams.Add("@DRN", dispatchNo);
        //                            _wmsParams.Add("@JSONData", Encoding.UTF8.GetBytes(jsonData));
        //                            _wmsParams.Add("@ProcessType", "Pallet_Movement_L1");
        //                            db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                            //PutPalletBinMappedInfoL1(wmsPalletBinInfoL1);
        //                        } 
        //                        else if (transactionDetails.WorkorderType == "I0")
        //                        {
        //                            wmsPalletBinInfoI0 = GetPalletBinMappedInfoI0(transactionDetails.PalletTagID, transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, i.ActualDestinationTagID);
        //                            wmsPalletBinInfoI0.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
        //                            wmsPalletBinInfoI0List.Add(wmsPalletBinInfoI0);
        //                            wmsPalletBinInfoInternal.data = wmsPalletBinInfoI0List;
        //                            string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoInternal);
        //                            var _wmsParams = new DynamicParameters();
        //                            _wmsParams.Add("@ID", transactionDetails.TransID);
        //                            _wmsParams.Add("@DRN", "Internal Movement");
        //                            _wmsParams.Add("@JSONData", Encoding.UTF8.GetBytes(jsonData));
        //                            _wmsParams.Add("@ProcessType", "Pallet_Movement_I0");
        //                            db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSData, _wmsParams, commandType: CommandType.StoredProcedure);
        //                        }
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                //Should never be here..
        //            }

        //        }
        //        else if (transactionDetails.TransID != null && transactionDetails.SubTagDetails != null && transactionDetails.SubTagDetails.Count == 0)
        //        {
        //            //Posting Parent if Subitems count is zero
        //            _params.Add("@TransID", transactionDetails.TransID);
        //            _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
        //            _params.Add("@PalletTagID", transactionDetails.PalletTagID);
        //            _params.Add("@CategoryID", transactionDetails.CategoryID);
        //            _params.Add("@AntennaID", transactionDetails.AntennaID);
        //            _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
        //            _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
        //            _params.Add("@WorkorderType", transactionDetails.WorkorderType);
        //            _params.Add("@Count", transactionDetails.Count);
        //            _params.Add("@RSSI", transactionDetails.RSSI);
        //            _params.Add("@TransDatetime", transactionDetails.TransDatetime);
        //            _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //            db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTransLogForService, _params, commandType: CommandType.StoredProcedure);
        //            retval = _params.Get<bool>("@Status");

        //            //retval = true;
        //        }
        //        else if (transactionDetails.TransID != null && transactionDetails.SubTagDetails == null)
        //        {
        //            //Posting Parent if Subitems come as null...
        //            _params.Add("@TransID", transactionDetails.TransID);
        //            _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
        //            _params.Add("@PalletTagID", transactionDetails.PalletTagID);
        //            _params.Add("@CategoryID", transactionDetails.CategoryID);
        //            _params.Add("@AntennaID", transactionDetails.AntennaID);
        //            _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
        //            _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
        //            _params.Add("@WorkorderType", transactionDetails.WorkorderType);
        //            _params.Add("@Count", transactionDetails.Count);
        //            _params.Add("@RSSI", transactionDetails.RSSI);
        //            _params.Add("@TransDatetime", transactionDetails.TransDatetime);
        //            _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
        //            db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTransLogForService, _params, commandType: CommandType.StoredProcedure);
        //            retval = _params.Get<bool>("@Status");

        //            //retval = true;
        //        }
        //    }
        //    return retval;
        //}
        public bool InsertTransactionDetailsForBotService(TransactionDetailsV1 transactionDetails)
        {
            bool retval = false;


            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();

                if (transactionDetails.TransID != null && transactionDetails.SubTagDetails != null && transactionDetails.SubTagDetails.Count > 0)
                {
                    _params.Add("@TransID", transactionDetails.TransID);
                    _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
                    _params.Add("@PalletTagID", transactionDetails.PalletTagID);
                    _params.Add("@CategoryID", transactionDetails.CategoryID);
                    _params.Add("@AntennaID", transactionDetails.AntennaID);
                    _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
                    _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
                    _params.Add("@WorkorderType", transactionDetails.WorkorderType);
                    _params.Add("@Count", transactionDetails.Count);
                    _params.Add("@RSSI", transactionDetails.RSSI);
                    _params.Add("@TransDatetime", transactionDetails.TransDatetime);
                    _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTransLogForService, _params, commandType: CommandType.StoredProcedure);
                    retval = _params.Get<bool>("@Status");

                    if (transactionDetails.SubTagDetails != null && transactionDetails.SubTagDetails.Count != 0)
                    {
                        foreach (SubTagDataV1 i in transactionDetails.SubTagDetails)
                        {
                            WmsPalletBinInfo wmsPalletBin = new WmsPalletBinInfo();
                            WmsPalletBinInfoL0 wmsPalletBinInfoL0 = new WmsPalletBinInfoL0();
                            WmsPalletBinInfoL1 wmsPalletBinInfoL1 = new WmsPalletBinInfoL1();
                            List<WmsPalletBinInfoInternalDetails> wmsPalletBinInfoI0List = new List<WmsPalletBinInfoInternalDetails>();
                            WmsPalletBinInfoInternalDetails wmsPalletBinInfoI0 = new WmsPalletBinInfoInternalDetails();
                            WmsPalletBinInfoInternal wmsPalletBinInfoInternal = new WmsPalletBinInfoInternal();
                            _params.Add("@TransID", transactionDetails.TransID);
                            _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
                            _params.Add("@PalletTagID", transactionDetails.PalletTagID);
                            _params.Add("@CategoryID", transactionDetails.CategoryID);
                            _params.Add("@AntennaID", transactionDetails.AntennaID);
                            _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
                            _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
                            _params.Add("@WorkorderType", transactionDetails.WorkorderType);
                            _params.Add("@Count", transactionDetails.Count);
                            _params.Add("@RSSI", transactionDetails.RSSI);
                            _params.Add("@TransDatetime", transactionDetails.TransDatetime);

                            _params.Add("@SubTransID", transactionDetails.TransID);
                            _params.Add("@SuggestedDestinationTagID", i.SuggestedDestinationTagID);
                            _params.Add("@ActualDestinationTagID", i.ActualDestinationTagID);
                            _params.Add("@SubRSSI", i.RSSI);
                            _params.Add("@SubCount", i.Count);
                            _params.Add("@SubCategoryID", i.CategoryID);
                            _params.Add("@TagType", i.TagType);
                            _params.Add("@SubTransDatetime", i.TransDatetime);
                            _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                            db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTranslogitemsForService, _params, commandType: CommandType.StoredProcedure);
                            retval = _params.Get<bool>("@Status");

                            if (retval)
                            {
                                if (transactionDetails.WorkorderType == "U0" || transactionDetails.WorkorderType == "U1")
                                {
                                    wmsPalletBin = GetPalletBinMappedInfo(transactionDetails.PalletTagID, transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, i.ActualDestinationTagID);
                                    wmsPalletBin.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
                                    wmsPalletBin.palletSensorId = transactionDetails.PalletTagID;
                                    string jsonData = JsonConvert.SerializeObject(wmsPalletBin);
                                    
                                    var _wmsParams = new DynamicParameters();
                                    _wmsParams.Add("@ID", transactionDetails.TransID);
                                    _wmsParams.Add("@Type", "Pallet_Movement_U0U1");
                                    _wmsParams.Add("@receivingNo", wmsPalletBin.receivingNo);
                                    _wmsParams.Add("@dispatchNo", null);
                                    _wmsParams.Add("@warehouseId", wmsPalletBin.warehouseId);
                                    _wmsParams.Add("@palletName", wmsPalletBin.palletName);
                                    _wmsParams.Add("@palletSensorId", wmsPalletBin.palletSensorId);
                                    _wmsParams.Add("@binName", null);
                                    _wmsParams.Add("@fromBinName", wmsPalletBin.fromBinName);
                                    _wmsParams.Add("@fromPalletName", null);
                                    _wmsParams.Add("@toPalletName", null);
                                    _wmsParams.Add("@toBinName", wmsPalletBin.toBinName);
                                    _wmsParams.Add("@comment", null);
                                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                                }
                                else if (transactionDetails.WorkorderType == "L0")
                                {
                                    wmsPalletBinInfoL0 = GetPalletBinMappedInfoL0(transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, transactionDetails.PalletTagID, i.ActualDestinationTagID);
                                    string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoL0);
                                    
                                    var _wmsParams = new DynamicParameters();
                                    _wmsParams.Add("@ID", transactionDetails.TransID);
                                    _wmsParams.Add("@Type", "Pallet_Movement_L0");
                                    _wmsParams.Add("@receivingNo", null);
                                    _wmsParams.Add("@dispatchNo", wmsPalletBinInfoL0.dispatchNo);
                                    _wmsParams.Add("@warehouseId", null);
                                    _wmsParams.Add("@palletName", null);
                                    _wmsParams.Add("@palletSensorId", transactionDetails.PalletTagID);
                                    _wmsParams.Add("@binName", null);
                                    _wmsParams.Add("@fromBinName", null);
                                    _wmsParams.Add("@fromPalletName", null);
                                    _wmsParams.Add("@toPalletName", wmsPalletBinInfoL0.toPalletName);
                                    _wmsParams.Add("@toBinName", wmsPalletBinInfoL0.toBinName);
                                    _wmsParams.Add("@comment", null);
                                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                                    foreach (var wmsData in wmsPalletBinInfoL0.items)
                                    {
                                        var _wmsParamsDetails = new DynamicParameters();
                                        _wmsParamsDetails.Add("@ParentID", transactionDetails.TransID);
                                        _wmsParamsDetails.Add("@Type", "Pallet_Movement_L0");
                                        _wmsParamsDetails.Add("@fromBinName", wmsData.fromBinName);
                                        _wmsParamsDetails.Add("@fromPalletName", wmsData.fromPalletName);
                                        _wmsParamsDetails.Add("@stockBinId", wmsData.stockBinId);
                                        _wmsParamsDetails.Add("@serialNumber", null);
                                        _wmsParamsDetails.Add("@skuCode", wmsData.skuCode);
                                        _wmsParamsDetails.Add("@batchNumber", wmsData.batchNumber);
                                        _wmsParamsDetails.Add("@batchDateTime", null);
                                        _wmsParamsDetails.Add("@status", null);
                                        _wmsParamsDetails.Add("@qty", wmsData.qty);
                                        db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataChild, _wmsParamsDetails, commandType: CommandType.StoredProcedure);
                                    }

                                }
                                else if (transactionDetails.WorkorderType == "L1")
                                {
                                    wmsPalletBinInfoL1 = GetPalletBinMappedInfoL1(transactionDetails.PalletTagID, transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, i.ActualDestinationTagID);
                                    wmsPalletBinInfoL1.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
                                    wmsPalletBinInfoL1.palletSensorId = transactionDetails.PalletTagID;
                                    string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoL1);
                                    var query = "SELECT ProcessInfo1 FROM WorkOrder WHERE WONumber = @WorkOrderNumber";
                                    var dispatchNo = db.Query<string>(query, new { WorkorderNumber = transactionDetails.WorkorderNumber });
                                   
                                    var _wmsParams = new DynamicParameters();
                                    _wmsParams.Add("@ID", transactionDetails.TransID);
                                    _wmsParams.Add("@Type", "Pallet_Movement_L1");
                                    _wmsParams.Add("@receivingNo", null);
                                    _wmsParams.Add("@dispatchNo", dispatchNo);
                                    _wmsParams.Add("@warehouseId", wmsPalletBinInfoL1.warehouseId);
                                    _wmsParams.Add("@palletName", wmsPalletBinInfoL1.palletName);
                                    _wmsParams.Add("@palletSensorId", wmsPalletBinInfoL1.palletSensorId);
                                    _wmsParams.Add("@binName", null);
                                    _wmsParams.Add("@fromBinName", wmsPalletBinInfoL1.fromBinName);
                                    _wmsParams.Add("@fromPalletName", null);
                                    _wmsParams.Add("@toPalletName", null);
                                    _wmsParams.Add("@toBinName", wmsPalletBinInfoL1.toBinName);
                                    _wmsParams.Add("@comment", null);
                                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                                }
                                else if (transactionDetails.WorkorderType == "I0")
                                {
                                    wmsPalletBinInfoI0 = GetPalletBinMappedInfoI0(transactionDetails.PalletTagID, transactionDetails.WorkorderNumber, transactionDetails.WorkorderType, i.ActualDestinationTagID);
                                    wmsPalletBinInfoI0.warehouseId = ConfigurationManager.AppSettings["warehouseId"];
                                    wmsPalletBinInfoI0List.Add(wmsPalletBinInfoI0);
                                    wmsPalletBinInfoInternal.data = wmsPalletBinInfoI0List;
                                    string jsonData = JsonConvert.SerializeObject(wmsPalletBinInfoInternal);
                                    
                                    var _wmsParams = new DynamicParameters();
                                    _wmsParams.Add("@ID", transactionDetails.TransID);
                                    _wmsParams.Add("@Type", "Pallet_Movement_I0");
                                    _wmsParams.Add("@receivingNo", null);
                                    _wmsParams.Add("@dispatchNo", null);
                                    _wmsParams.Add("@warehouseId", wmsPalletBinInfoI0.warehouseId);
                                    _wmsParams.Add("@palletName", null);
                                    _wmsParams.Add("@palletSensorId", null);
                                    _wmsParams.Add("@binName", null);
                                    _wmsParams.Add("@fromBinName", wmsPalletBinInfoI0.fromBinName);
                                    _wmsParams.Add("@fromPalletName", wmsPalletBinInfoI0.fromPalletName);
                                    _wmsParams.Add("@toPalletName", wmsPalletBinInfoI0.toPalletName);
                                    _wmsParams.Add("@toBinName", wmsPalletBinInfoI0.toBinName);
                                    _wmsParams.Add("@comment", null);
                                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertWMSDataParent, _wmsParams, commandType: CommandType.StoredProcedure);
                                }
                            }
                        }
                    }
                    else
                    {
                        //Should never be here..
                    }

                }
                else if (transactionDetails.TransID != null && transactionDetails.SubTagDetails != null && transactionDetails.SubTagDetails.Count == 0)
                {
                    //Posting Parent if Subitems count is zero
                    _params.Add("@TransID", transactionDetails.TransID);
                    _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
                    _params.Add("@PalletTagID", transactionDetails.PalletTagID);
                    _params.Add("@CategoryID", transactionDetails.CategoryID);
                    _params.Add("@AntennaID", transactionDetails.AntennaID);
                    _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
                    _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
                    _params.Add("@WorkorderType", transactionDetails.WorkorderType);
                    _params.Add("@Count", transactionDetails.Count);
                    _params.Add("@RSSI", transactionDetails.RSSI);
                    _params.Add("@TransDatetime", transactionDetails.TransDatetime);
                    _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTransLogForService, _params, commandType: CommandType.StoredProcedure);
                    retval = _params.Get<bool>("@Status");

                    //retval = true;
                }
                else if (transactionDetails.TransID != null && transactionDetails.SubTagDetails == null)
                {
                    //Posting Parent if Subitems come as null...
                    _params.Add("@TransID", transactionDetails.TransID);
                    _params.Add("@ClientDeviceID", transactionDetails.ClientDeviceID);
                    _params.Add("@PalletTagID", transactionDetails.PalletTagID);
                    _params.Add("@CategoryID", transactionDetails.CategoryID);
                    _params.Add("@AntennaID", transactionDetails.AntennaID);
                    _params.Add("@ListItemStatus", transactionDetails.ListItemStatus);
                    _params.Add("@WorkorderNumber", transactionDetails.WorkorderNumber);
                    _params.Add("@WorkorderType", transactionDetails.WorkorderType);
                    _params.Add("@Count", transactionDetails.Count);
                    _params.Add("@RSSI", transactionDetails.RSSI);
                    _params.Add("@TransDatetime", transactionDetails.TransDatetime);
                    _params.Add("@Status", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    db.Execute(AppSettings.SQLQueryCommand.SP_InsertReaderTransLogForService, _params, commandType: CommandType.StoredProcedure);
                    retval = _params.Get<bool>("@Status");

                    //retval = true;
                }
            }
            return retval;
        }
        #endregion


        /*Warehouse code*/


        public List<CustomerMaster> GetAllCustomers()
        {
            List<CustomerMaster> castDetails = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                castDetails = db.Query<CustomerMaster>(AppSettings.SQLQueryCommand.SP_Get_CustomerDetails, commandTimeout: 60, commandType: CommandType.StoredProcedure).ToList();
            }
            return castDetails;
        }

        public List<Vendor> GetAllVendors(Guid tenantID)
        {
            List<Vendor> locations = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@TenantID", tenantID);
                locations = db
                    .Query<Vendor>(AppSettings.SQLQueryCommand.SP_Get_AllVendors, _params, commandType: CommandType.StoredProcedure).ToList();
            }
            return locations;
        }


        public List<LostAssets> GetAllRCOAssets(Guid tenantID)
        {
            List<LostAssets> data = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@TenantID", tenantID);
                data = db
                    .Query<LostAssets>(AppSettings.SQLQueryCommand.SP_GetAllRCOAssets, _params, commandType: CommandType.StoredProcedure).ToList();
            }
            return data;
        }

        public List<AssetTypeMaster> GetAllAssetType(Guid tenantID)
        {
            List<AssetTypeMaster> locations = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@TenantID", tenantID);
                locations = db
                    .Query<AssetTypeMaster>(AppSettings.SQLQueryCommand.SP_Get_AllAssetTypes, _params, commandType: CommandType.StoredProcedure).ToList();
            }
            return locations;
        }



        public List<AssetMaster> GetAllAssets(Guid tenantID)
        {
            List<AssetMaster> locations = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@TenantID", tenantID);
                locations = db
                    .Query<AssetMaster>(AppSettings.SQLQueryCommand.SP_Get_AllAssets, _params, commandType: CommandType.StoredProcedure).ToList();
            }
            return locations;
        }

        public List<TouchPointModel> GetAllTouchPoints(Guid tenantID)
        {
            List<TouchPointModel> touchPoints = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@TenantID", tenantID);
                touchPoints = db
                    .Query<TouchPointModel>(AppSettings.SQLQueryCommand.SP_Get_Touchpoints, _params, commandType: CommandType.StoredProcedure).ToList();
            }
            return touchPoints;
        }


        

        public User GetUserData(User usr)
        {
            User detail = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                // string password = Psl.Chase.Utils.CryptorEngine.Encrypt(usr.Password.ToString().Trim(), true);
                var _params = new DynamicParameters();
                _params.Add("@userName", usr.UserName);
                _params.Add("@password", usr.Password);
                _params.Add("@ComapnyId", usr.CustomerID);
                detail = db.Query<User>(AppSettings.SQLQueryCommand.Login, _params, commandType: CommandType.StoredProcedure).FirstOrDefault();
                //password = string.Empty;
            }
            return detail;
        }

        public bool createActivity(Activity activity)
        {
            try
            {
                activity.TransactionDateTime = DateTime.Now;
                Guid obj = Guid.NewGuid();
                activity.ActivityID = obj;
            
                db.Activities.Add(activity);
                db.SaveChanges();
                foreach (var activityDetails in activity.ActivityDetails)
                {
                    var asset = db.AssetMasters.First(r => r.AssetID == activityDetails.AssetID);
                    if (activity.ActivityType == "IN")
                    {
                        asset.AState = "IN";
                        asset.AStateDateTime = DateTime.Now;
                    }
                    else if (activity.ActivityType == "OUT")
                    {
                        asset.AState = "OUT";
                        asset.AStateDateTime = DateTime.Now;
                    }
                    else if (activity.ActivityType == "INV")
                    {
                        asset.LastInventoryDateTime = DateTime.Now;
                    }
                    db.Entry(asset).State = EntityState.Modified;
                    db.Entry(activityDetails).State = EntityState.Modified;
                }
                db.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                if (ActivityExists(activity.ActivityID))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }


        public bool registerAsset(RegisterModel registerModel)
        {
            try
            {
                var asset = db.AssetMasters.Where(r => r.AssetID == registerModel.AssetID && r.CustomerID == registerModel.CustomerID && r.ASerialNo.ToString() == "").FirstOrDefault();
                if (asset != null)
                {
                    asset.ASerialNo = registerModel.ASerialNo;
                    asset.SID = registerModel.SID;
                    asset.UID = registerModel.UID.Value;
                    asset.ATagID = registerModel.ATagID;
                    db.Entry(asset).State = EntityState.Modified;
                    var serialNo = db.SerialNumbers.First(r => r.SerialNumber1 == registerModel.ASerialNo && r.CustomerID == registerModel.CustomerID);
                    serialNo.AssetTypeID = asset.ATypeID;
                    db.Entry(serialNo).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        public bool RegisterAssetMobile(RegisterModelMobile registerModel)
        {
            try
            {
                var asset = db.AssetMasters.Where(r => r.AssetID == registerModel.AssetID && r.CustomerID == registerModel.CustomerID && r.ASerialNo.ToString() == "").FirstOrDefault();
                if (asset != null)
                {
                    var serialNo = db.SerialNumbers.First(r => r.SerialNumber1 == registerModel.ASerialNo && r.CustomerID == registerModel.CustomerID);
                    asset.ASerialNo = registerModel.ASerialNo;
                    asset.SID = serialNo.SID;
                    asset.UID = registerModel.UID.Value;
                    asset.ATagID = registerModel.ATagID;
                    asset.IsRegistered = true;
                    db.Entry(asset).State = EntityState.Modified;
                    serialNo.AssetTypeID = asset.ATypeID;
                    serialNo.TagID = registerModel.ATagID;
                    db.Entry(serialNo).State = EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (DbUpdateException)
            {
                throw;
            }
        }

        private bool ActivityExists(Guid id)
        {
            return db.Activities.Count(e => e.ActivityID == id) > 0;
        }

//        public LoginResponse MobileLogin(LoginModel login)
//        {

//            var loginResponse = new LoginResponse();
//            using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(AppSettings.ConnectionString))
//            {


//                using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand())
//                {
//                    cmd.CommandText = AppSettings.SQLQueryCommand.MobileLogin;
//                    cmd.Parameters.AddWithValue("@userName", login.UserName);
//                    cmd.Parameters.AddWithValue("@password", login.Password);
//                    cmd.Parameters.AddWithValue("@ClientDeviceId", login.ClientDeviceID);
//                    cmd.Connection = conn;
//                    cmd.CommandType = CommandType.StoredProcedure;

//                    conn.Open();

//                    System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(cmd);

//                    DataSet ds = new DataSet();
//                    adapter.Fill(ds);
//                    DataTable tableA = ds.Tables[0];
//                    DataTable tableB = ds.Tables[1];
//                    DataTable tablec = ds.Tables[2];
//                    loginResponse = ds.Tables[0].AsEnumerable()
//   .Select(dataRow => new LoginResponse
//   {
//       UserName = dataRow.Field<string>("UserName"),
//       Password = dataRow.Field<string>("Password"),
//       CompanyCode = dataRow.Field<int>("CompanyCode"),
//       CustomerID = dataRow.Field<Guid>("CustomerID"),
//       UserID = dataRow.Field<Guid>("UserID"),
//       //ISLogRequired = dataRow.Field<bool>("ISLogRequired")
//   }).First();

//                    loginResponse.Vendor = ds.Tables[1].AsEnumerable()
//.Select(dataRow => new VendorLogin
//{
//    VendorID = dataRow.Field<int>("VendorID"),
//    Name = dataRow.Field<string>("Name")

//}).ToList();

//                    loginResponse.AssetType = ds.Tables[2].AsEnumerable()
//.Select(dataRow => new AssetTypeLogin
//{
//    ATypeID = dataRow.Field<int>("ATypeID"),
//    AssetName = dataRow.Field<string>("AssetName")
//}).ToList();

//                    loginResponse.Room = ds.Tables[3].AsEnumerable()
//.Select(dataRow => new RoomMasterLogin
//{
//   RoomID = dataRow.Field<Guid>("RoomID"),
//   RoomName = dataRow.Field<string>("RoomNo")

//}).ToList();

//                    conn.Close();
//                }
//            }
//            return loginResponse;


//        }


        //public List<AssetMasterResponse> GetAllAssetsforMobile(Guid tenantID)
        //{
        //    List<AssetMasterResponse> locations = null;
        //    using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        var _params = new DynamicParameters();
        //        _params.Add("@TenantID", tenantID);
        //        locations = db
        //            .Query<AssetMasterResponse>(AppSettings.SQLQueryCommand.SP_Get_AllAssets_Mobile, _params, commandType: CommandType.StoredProcedure).ToList();
        //    }
        //    return locations;
        //}

        //public List<AssetMasterResponse> GetAllAssetsforMobile(UserData user) //Master Response_GRB
        //{
        //    List<AssetMasterResponse> locations = new List<AssetMasterResponse>();
        //    //string connectionString = "Server=your_server_name;Database=your_database_name;User Id=your_username;Password=your_password;";
        //    string query = "SELECT AM.ATagID, AM.AName, ATM.AssetName FROM AssetMaster AM,AssetTypeMaster ATM WHERE AM.ATypeID=ATM.ATypeID and AM.CustomerID = @CustomerID "; // Replace YourTableName with the actual table name

        //    using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            command.Parameters.AddWithValue("@CustomerID", user.CustomerID);
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    // Access data using reader.GetString(), reader.GetInt32(), etc.
        //                    AssetMasterResponse asset = new AssetMasterResponse();
        //                    asset.tagid = reader.GetString(0);
        //                    asset.name = reader.GetString(1);
        //                    asset.type = reader.GetString(2);
        //                    //asset.status = "true";
        //                    //asset.message = "Success";

        //                    //Console.WriteLine($"ID: {id}, Name: {name}, Email: {email}");
        //                    locations.Add(asset);
        //                }
        //            }
        //        }
        //    }
        //        return locations;
        //}

        //public bool CreateActivityMobile(ActivityModel activityModel)
        //{
        //    Guid obj = Guid.NewGuid();
        //    var activity = new Activity()
        //    {
        //        ActivityID = obj,
        //        UID = activityModel.UID,
        //        CustomerID = activityModel.CustomerID,
        //        ActivityType = activityModel.ActivityType,
        //        RoomID=activityModel.RoomID,
        //        VendorID = activityModel.VendorID,
        //        Count = activityModel.Count,
        //        StartDate = activityModel.StartDate,
        //        EndDate = activityModel.EndDate.Value,
        //    };
        //    try
        //    {

        //        activity.TransactionDateTime = DateTime.Now;
        //        foreach (var tag in activityModel.Data)
        //        {
        //            var asset = db.AssetMasters.FirstOrDefault(r => r.ATagID == tag);
        //            if (asset != null)
        //            {
        //                var activityDetail = new ActivityDetail()
        //                {
        //                    AssetID = asset.AssetID,
        //                    ActivityID = activity.ActivityID
        //                };
        //                activity.ActivityDetails.Add(activityDetail);
        //            }
        //            else
        //            {
        //                Guid eRRActivityDetailID = Guid.NewGuid();
        //                var errorActivityDetail = new ErrorActivityDetail()
        //                {
        //                    ERRActivityDetailID = eRRActivityDetailID,
        //                    ActivityID = activity.ActivityID,
        //                    TagID = tag
        //                };
        //                activity.ErrorActivityDetails.Add(errorActivityDetail);
        //            }
        //        }
        //        foreach (var activityDetails in activity.ActivityDetails)
        //        {
        //            Guid obj2 = Guid.NewGuid();
        //            activityDetails.ActivityDetailsID = obj2;
        //            var asset = db.AssetMasters.First(r => r.AssetID == activityDetails.AssetID);
        //            if (activityModel.ActivityType == "IN")
        //            {
        //                asset.AState = "IN";
        //                asset.AStateDateTime = DateTime.Now;
        //            }
        //            else if (activityModel.ActivityType == "OUT")
        //            {
        //                asset.AState = "OUT";
        //                asset.AStateDateTime = DateTime.Now;
        //                asset.AssetLife = asset.AssetLife + 1;
        //            }
        //            else if (activityModel.ActivityType == "INV")
        //            {
        //                DateTime indianTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
        //                asset.LastInventoryDateTime = indianTime.ToUniversalTime();
        //            }
        //            db.Entry(asset).State = EntityState.Modified;
        //        }
        //        db.Activities.Add(activity);
        //        db.SaveChanges();
        //        return true;
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (ActivityExists(activity.ActivityID))
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //}

        public List<RoomMasterModel> GetAllRooms(Guid tenantID)
        {
            List<RoomMasterModel> rooms = null;
            using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
            {
                db.Open();
                var _params = new DynamicParameters();
                _params.Add("@TenantID", tenantID);
                rooms = db
                    .Query<RoomMasterModel>(AppSettings.SQLQueryCommand.SP_Get_AllRooms, _params, commandType: CommandType.StoredProcedure).ToList();
            }
            return rooms;
        }



        //public List<TaskMaster> GetTaskMaster(DateTime? ModifiedDateTime)
        //{
        //    List<TaskMaster> tasks = null;
        //    using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        var _params = new DynamicParameters();
        //        _params.Add("@ModifiedDateTime", ModifiedDateTime);
        //        tasks = db.Query<TaskMaster>(AppSettings.SQLQueryCommand.SP_Get_GETTasks, _params, commandTimeout: 60, commandType: CommandType.StoredProcedure).ToList();
        //    }
        //    return tasks;
        //}

        //public List<TransactionType> GetTansactionTypes(DateTime? ModifiedDateTime)
        //{
        //    List<TransactionType> transactionTypes = null;
        //    using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        var _params = new DynamicParameters();
        //        _params.Add("@ModifiedDateTime", ModifiedDateTime);
        //        transactionTypes = db.Query<TransactionType>(AppSettings.SQLQueryCommand.SP_GET_TansactionTypes, _params, commandTimeout: 60, commandType: CommandType.StoredProcedure).ToList();
        //    }
        //    return transactionTypes;
        //}

        //public List<PDAUser> GetUsers(DateTime? ModifiedDateTime)
        //{
        //    List<PDAUser> users = null;
        //    using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        var _params = new DynamicParameters();
        //        _params.Add("@ModifiedDateTime", ModifiedDateTime);
        //        users = db.Query<PDAUser>(AppSettings.SQLQueryCommand.SP_GET_PDAUsers, _params, commandTimeout: 60, commandType: CommandType.StoredProcedure).ToList();
        //    }
        //    return users;
        //}

        //public List<PDASourcePlant> GetSourcePlants(DateTime? ModifiedDateTime)
        //{
        //    List<PDASourcePlant> plants = null;
        //    using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        var _params = new DynamicParameters();
        //        _params.Add("@ModifiedDateTime", ModifiedDateTime);
        //        plants = db.Query<PDASourcePlant>(AppSettings.SQLQueryCommand.SP_GET_PDASourcePlants, _params, commandTimeout: 60, commandType: CommandType.StoredProcedure).ToList();
        //    }
        //    return plants;
        //}

        //public List<TouchPoint> GetTouchPoints(DateTime? ModifiedDateTime)
        //{
        //    List<TouchPoint> touchPoints = null;
        //    using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        var _params = new DynamicParameters();
        //        _params.Add("@ModifiedDateTime", ModifiedDateTime);
        //        touchPoints = db.Query<TouchPoint>(AppSettings.SQLQueryCommand.SP_GET_TouchPoints, _params, commandTimeout: 60, commandType: CommandType.StoredProcedure).ToList();
        //    }
        //    return touchPoints;
        //}

        //public List<Location> GetLocations(DateTime? ModifiedDateTime)
        //{
        //    List<Location> locations = null;
        //    using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        var _params = new DynamicParameters();
        //        _params.Add("@ModifiedDateTime", ModifiedDateTime);
        //        locations = db.Query<Location>(AppSettings.SQLQueryCommand.SP_GET_Locations, _params, commandTimeout: 60, commandType: CommandType.StoredProcedure).ToList();
        //    }
        //    return locations;
        //}

        //public TripDetailsInfo GetLastTripByTruckId(string truckId)
        //{
        //    TripDetailsInfo tripDetail = null;
        //    using (IDbConnection db = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        db.Open();
        //        var _params = new DynamicParameters();
        //        _params.Add("@TruckId", truckId);
        //        tripDetail = db
        //            .Query<TripDetailsInfo>(AppSettings.SQLQueryCommand.SP_GET_LastTripDetailsbyTruckID, _params, commandType: CommandType.StoredProcedure)
        //            .FirstOrDefault();
        //    }
        //    return tripDetail;
        //}

        //public int InsertTripDetail(PDATripDetail tripdetail)
        //{
        //    int retVal = 0;
        //    using (IDbConnection conn = new SqlConnection(AppSettings.ConnectionString))
        //    {
        //        conn.Open();
        //        var _params = new DynamicParameters();
        //        _params.Add("@TripID", tripdetail.TripId);
        //        _params.Add("@TouchPointType", tripdetail.TouchPointType);
        //        _params.Add("@TruckID", tripdetail.TruckID);
        //        _params.Add("@TaskTypeID", tripdetail.TaskTypeID);
        //        _params.Add("@STONumber", tripdetail.STONumber);
        //        _params.Add("@RouteID", tripdetail.RouteID);
        //        _params.Add("@LotNumber", tripdetail.LotNumber);
        //        _params.Add("@VesselCode", tripdetail.VesselCode);
        //        _params.Add("@BundurName", tripdetail.BundurName);
        //        _params.Add("@PermitNo", tripdetail.PermitNo);
        //        _params.Add("@PrintSrNo", tripdetail.printSrNo);
        //        _params.Add("@UserName", tripdetail.UserID);
        //        _params.Add("@ValidatePrevTrip", tripdetail.ValidatePrevTrip);
        //        _params.Add("@ReturnVal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        //        conn.Execute(AppSettings.SQLQueryCommand.SP_Insert_PDA_TripDetails, _params, commandTimeout: 60, commandType: CommandType.StoredProcedure);
        //        retVal = _params.Get<int>("@ReturnVal");
        //    }
        //    return retVal;
        //}
        #region WMS API Method
        private WMSPalletData GetPalletInfo(string TruckNo, string ParentTagID)
        {
            WMSPalletData palletData = new WMSPalletData();

            // Create SqlConnection and SqlCommand objects
            using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(AppSettings.SQLQueryCommand.SP_GetPalletInfo, connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@TruckNumber", SqlDbType.VarChar).Value = TruckNo;
                command.Parameters.Add("@ParentTagID", SqlDbType.VarChar).Value = ParentTagID;

                try
                {
                    // Open the connection
                    connection.Open();

                    // Execute the command
                    SqlDataReader reader = command.ExecuteReader();

                    // Read first result set
                    ////while (reader.Read())
                    ////{
                    ////    palletData.receivingNo = reader.GetString(0);
                    ////}

                    ////// Move to the next result set
                    ////reader.NextResult();

                    // Read second result set
                    while (reader.Read())
                    {
                        palletData.palletName = reader.GetString(0);
                    }

                    // Move to the next result set
                    reader.NextResult();

                    // Read third result set
                    while (reader.Read())
                    {
                        palletData.binName = reader.GetString(0);
                    }

                    // Close the reader
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }


            return palletData;
        }
        private WMSPalletDataDispatch GetPalletInfoDispatch(string DRN, string ParentTagID)
        {
            WMSPalletDataDispatch palletData = new WMSPalletDataDispatch();

            // Create SqlConnection and SqlCommand objects
            using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(AppSettings.SQLQueryCommand.SP_GetPalletInfoDis, connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@DRN", SqlDbType.VarChar).Value = DRN;
                command.Parameters.Add("@ParentTagID", SqlDbType.VarChar).Value = ParentTagID;

                try
                {
                    // Open the connection
                    connection.Open();

                    // Execute the command
                    SqlDataReader reader = command.ExecuteReader();

                    // Read first result set
                    //while (reader.Read())
                    //{
                    //    palletData.dispatchNo = reader.GetString(0);
                    //}

                    //// Move to the next result set
                    //reader.NextResult();

                    // Read second result set
                    while (reader.Read())
                    {
                        palletData.palletName = reader.GetString(0);
                    }

                    // Move to the next result set
                    reader.NextResult();

                    // Read third result set
                    while (reader.Read())
                    {
                        palletData.binName = reader.GetString(0);
                    }

                    // Close the reader
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }


            return palletData;
        }
        private WmsPalletBinInfo GetPalletBinMappedInfo(string PalletTagID, string WorkorderNumber, string WorkorderType, string SubTagID)
        {
            WmsPalletBinInfo palletBinData = new WmsPalletBinInfo();

            // Create SqlConnection and SqlCommand objects
            using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(AppSettings.SQLQueryCommand.SP_GetPalletBinMappedInfo, connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@SubTagID", SqlDbType.VarChar).Value = SubTagID;
                command.Parameters.Add("@PalletTagID", SqlDbType.VarChar).Value = PalletTagID;
                command.Parameters.Add("@WorkorderNumber", SqlDbType.VarChar).Value = WorkorderNumber;
                command.Parameters.Add("@WorkorderType", SqlDbType.VarChar).Value = WorkorderType;

                try
                {

                    connection.Open();


                    SqlDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {
                        palletBinData.toBinName = reader.GetString(0);
                    }


                    reader.NextResult();


                    while (reader.Read())
                    {
                        palletBinData.palletName = reader.GetString(0);
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        palletBinData.fromBinName = reader.GetString(0);
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        palletBinData.receivingNo = reader.GetString(0);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }


            return palletBinData;
        }
        private WmsPalletBinInfoL0 GetPalletBinMappedInfoL0( string WorkorderNumber, string WorkorderType, string PalletTagID, string SubTagID)
        {
            WmsPalletBinInfoL0 wmsPalletBinInfoL0 = new WmsPalletBinInfoL0();
            List<WmsItemsList> itemsList = new List<WmsItemsList>();

            // Create SqlConnection and SqlCommand objects
            using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(AppSettings.SQLQueryCommand.SP_GetPalletBinMappedInfoL0, connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@SubTagID", SqlDbType.VarChar).Value = SubTagID;
                command.Parameters.Add("@PalletTagID", SqlDbType.VarChar).Value = PalletTagID;
                command.Parameters.Add("@WorkorderNumber", SqlDbType.VarChar).Value = WorkorderNumber;
                command.Parameters.Add("@WorkorderType", SqlDbType.VarChar).Value = WorkorderType;

                try
                {

                    connection.Open();


                    SqlDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {
                        wmsPalletBinInfoL0.dispatchNo = reader.GetString(0);
                    }


                    reader.NextResult();


                    while (reader.Read())
                    {
                        wmsPalletBinInfoL0.toBinName = reader.GetString(0);
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        wmsPalletBinInfoL0.toPalletName = reader.GetString(0);
                    }

                    reader.Close();
                    
                        SqlCommand command1 = new SqlCommand(AppSettings.SQLQueryCommand.SP_GetPalletBinMappedItemsL0, connection);
                        command1.CommandType = CommandType.StoredProcedure;

                        command1.Parameters.Add("@SubTagID", SqlDbType.VarChar).Value = SubTagID;
                        command1.Parameters.Add("@PalletTagID", SqlDbType.VarChar).Value = PalletTagID;
                        command1.Parameters.Add("@WorkorderNumber", SqlDbType.VarChar).Value = WorkorderNumber;
                        command1.Parameters.Add("@WorkorderType", SqlDbType.VarChar).Value = WorkorderType;

                    using (SqlDataReader reader1 = command1.ExecuteReader())
                    {
                        while (reader1.Read())
                        {
                            WmsItemsList item = new WmsItemsList
                            {
                                fromBinName = reader1["fromBinName"].ToString(),
                                skuCode = reader1["skuCode"].ToString(),
                                batchNumber = reader1["batchNumber"].ToString() == "null" ? null : reader1["batchNumber"].ToString(),
                                stockBinId = reader1["stockBinId"] as int?,
                                qty = Convert.ToInt32(reader1["qty"])

                            };
                            itemsList.Add(item);
                        }
                        reader1.Close();
                    }


                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            wmsPalletBinInfoL0.items = itemsList;
            return wmsPalletBinInfoL0;
        }
        private WmsPalletBinInfoL1 GetPalletBinMappedInfoL1(string PalletTagID, string WorkorderNumber, string WorkorderType, string SubTagID)
        {
            WmsPalletBinInfoL1 palletBinData = new WmsPalletBinInfoL1();

            // Create SqlConnection and SqlCommand objects
            using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(AppSettings.SQLQueryCommand.SP_GetPalletBinMappedInfo, connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@SubTagID", SqlDbType.VarChar).Value = SubTagID;
                command.Parameters.Add("@PalletTagID", SqlDbType.VarChar).Value = PalletTagID;
                command.Parameters.Add("@WorkorderNumber", SqlDbType.VarChar).Value = WorkorderNumber;
                command.Parameters.Add("@WorkorderType", SqlDbType.VarChar).Value = WorkorderType;

                try
                {

                    connection.Open();


                    SqlDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {
                        palletBinData.toBinName = reader.GetString(0);
                    }


                    reader.NextResult();


                    while (reader.Read())
                    {
                        palletBinData.palletName = reader.GetString(0);
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        palletBinData.fromBinName = reader.GetString(0);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }


            return palletBinData;
        }
        private WmsPalletBinInfoInternalDetails GetPalletBinMappedInfoI0(string PalletTagID, string WorkorderNumber, string WorkorderType, string SubTagID)
        {
            WmsPalletBinInfoInternalDetails palletBinData = new WmsPalletBinInfoInternalDetails();

            // Create SqlConnection and SqlCommand objects
            using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(AppSettings.SQLQueryCommand.SP_GetPalletBinMappedInfoI0, connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@SubTagID", SqlDbType.VarChar).Value = SubTagID;
                command.Parameters.Add("@PalletTagID", SqlDbType.VarChar).Value = PalletTagID;
                command.Parameters.Add("@WorkorderNumber", SqlDbType.VarChar).Value = WorkorderNumber;
                command.Parameters.Add("@WorkorderType", SqlDbType.VarChar).Value = WorkorderType;

                try
                {

                    connection.Open();


                    SqlDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {
                        palletBinData.toBinName = reader.GetString(0);
                    }


                    reader.NextResult();


                    while (reader.Read())
                    {
                        palletBinData.fromPalletName = reader.GetString(0);
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        palletBinData.toPalletName = reader.GetString(0);
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        palletBinData.fromBinName = reader.GetString(0);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }


            return palletBinData;
        }
        private WmsPalletBinInfoInternalDetails GetPalletInfoForItemMovement(string SourcePalletTagID, string DestinationPalletTagID, string SourceBinTagID, string DestinationBinTagID)
        {
            WmsPalletBinInfoInternalDetails internalMoveData = new WmsPalletBinInfoInternalDetails();

            // Create SqlConnection and SqlCommand objects
            using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(AppSettings.SQLQueryCommand.SP_GetPalletBinMappedInfoForItemMovement, connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.Add("@SourcePalletTagID", SqlDbType.VarChar).Value = SourcePalletTagID;
                command.Parameters.Add("@DestinationPalletTagID", SqlDbType.VarChar).Value = DestinationPalletTagID;
                command.Parameters.Add("@SourceBinTagID", SqlDbType.VarChar).Value = SourceBinTagID;
                command.Parameters.Add("@DestinationBinTagID", SqlDbType.VarChar).Value = DestinationBinTagID;

                try
                {

                    connection.Open();


                    SqlDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {
                        internalMoveData.fromPalletName = reader.GetString(0);
                    }


                    reader.NextResult();


                    while (reader.Read())
                    {
                        internalMoveData.toPalletName = reader.GetString(0);
                    }

                    reader.NextResult();

                    while (reader.Read())
                    {
                        internalMoveData.fromBinName = reader.GetString(0);
                    }
                    reader.NextResult();
                    while (reader.Read())
                    {
                        internalMoveData.toBinName = reader.GetString(0);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }


            return internalMoveData;
        }
        //public bool PutPalletInfo(WMSPalletData wMSPallet)
        //{
        //    bool retval = false;
        //    string token = GetAuthorizationToken().access_token;
        //    string ReponseConvert = string.Empty;
        //    string jsonData = JsonConvert.SerializeObject(wMSPallet);

        //    var client = new HttpClient();
        //    string unloadMethodUrl = ConfigurationManager.AppSettings["UnloadMethodUrl"];
        //    var request = new HttpRequestMessage(HttpMethod.Put, unloadMethodUrl);
        //    //var request = new HttpRequestMessage(HttpMethod.Put, "http://192.168.100.18:8082/wms/api/v1/stocks/process/unload");
        //    request.Headers.Add("Accept", "application/json");
        //    request.Headers.Add("Authorization", "Bearer " + token);
        //    request.Headers.Add("X-TenantId", "1");
        //    var content = new StringContent(jsonData, null, "application/json");
        //    request.Content = content;
        //    var response = client.SendAsync(request);
        //    Task<string> responseBody = response.Result.Content.ReadAsStringAsync();
        //    ReponseConvert = responseBody.Result.ToString();


        //    return retval;
        //}
        //private bool PutPalletInfoDispatch(WMSPalletDataDispatch wMSPallet)
        //{
        //    bool retval = false;
        //    string token = GetAuthorizationToken().access_token;
        //    string ReponseConvert = string.Empty;
        //    string jsonData = JsonConvert.SerializeObject(wMSPallet);

        //    var client = new HttpClient();
        //    string loadMethodUrl = ConfigurationManager.AppSettings["LoadMethodUrl"];
        //    var request = new HttpRequestMessage(HttpMethod.Put, loadMethodUrl);
        //    //var request = new HttpRequestMessage(HttpMethod.Put, "http://192.168.100.18:8082/wms/api/v1/dispatch/loadComplete");
        //    request.Headers.Add("Accept", "application/json");
        //    request.Headers.Add("Authorization", "Bearer " + token);
        //    request.Headers.Add("X-TenantId", "1");
        //    var content = new StringContent(jsonData, null, "application/json");
        //    request.Content = content;
        //    var response = client.SendAsync(request);
        //    Task<string> responseBody = response.Result.Content.ReadAsStringAsync();
        //    ReponseConvert = responseBody.Result.ToString();


        //    return retval;
        //}
        //private bool PutPalletBinMappedInfo(WmsPalletBinInfo wmsPalletBin)
        //{
        //    bool retval = false;
        //    string token = GetAuthorizationToken().access_token;
        //    string ReponseConvert = string.Empty;
        //    string jsonData = JsonConvert.SerializeObject(wmsPalletBin);

        //    var client = new HttpClient();
        //    string receivingMethodUrl = ConfigurationManager.AppSettings["ReceivingMethodUrl"];
        //    var request = new HttpRequestMessage(HttpMethod.Put, receivingMethodUrl);
        //    //var request = new HttpRequestMessage(HttpMethod.Put, "http://192.168.100.18:8082/wms/api/v1/stocks/process/put");
        //    request.Headers.Add("Accept", "application/json");
        //    request.Headers.Add("Authorization", "Bearer " + token);
        //    request.Headers.Add("X-TenantId", "1");
        //    var content = new StringContent(jsonData, null, "application/json");
        //    request.Content = content;
        //    var response = client.SendAsync(request);
        //    Task<string> responseBody = response.Result.Content.ReadAsStringAsync();
        //    ReponseConvert = responseBody.Result.ToString();


        //    return retval;
        //}
        //private bool PutPalletBinMappedInfoL0(WmsPalletBinInfoL0 wmsPalletBin)
        //{
        //    bool retval = false;
        //    string token = GetAuthorizationToken().access_token;
        //    string ReponseConvert = string.Empty;
        //    string jsonData = JsonConvert.SerializeObject(wmsPalletBin);

        //    var client = new HttpClient();
        //    string dispatchMethodUrlL0 = ConfigurationManager.AppSettings["DispatchMethodUrlL0"];
        //    var request = new HttpRequestMessage(HttpMethod.Put, dispatchMethodUrlL0);
        //    //var request = new HttpRequestMessage(HttpMethod.Put, "http://192.168.100.18:8082/wms/api/v1/stocks/process/pick");
        //    request.Headers.Add("Accept", "application/json");
        //    request.Headers.Add("Authorization", "Bearer " + token);
        //    request.Headers.Add("X-TenantId", "1");
        //    var content = new StringContent(jsonData, null, "application/json");
        //    request.Content = content;
        //    var response = client.SendAsync(request);
        //    Task<string> responseBody = response.Result.Content.ReadAsStringAsync();
        //    ReponseConvert = responseBody.Result.ToString();


        //    return retval;
        //}
        //private bool PutPalletBinMappedInfoL1(WmsPalletBinInfoL1 wmsPalletBin)
        //{
        //    bool retval = false;
        //    string token = GetAuthorizationToken().access_token;
        //    string ReponseConvert = string.Empty;
        //    string jsonData = JsonConvert.SerializeObject(wmsPalletBin);

        //    var client = new HttpClient();
        //    string dispatchMethodUrlL1 = ConfigurationManager.AppSettings["DispatchMethodUrlL1"];
        //    var request = new HttpRequestMessage(HttpMethod.Put, dispatchMethodUrlL1);
        //    //var request = new HttpRequestMessage(HttpMethod.Put, "http://192.168.100.18:8082/wms/api/v1/stocks/process/moveLoadingZone");
        //    request.Headers.Add("Accept", "application/json");
        //    request.Headers.Add("Authorization", "Bearer " + token);
        //    request.Headers.Add("X-TenantId", "1");
        //    var content = new StringContent(jsonData, null, "application/json");
        //    request.Content = content;
        //    var response = client.SendAsync(request);
        //    Task<string> responseBody = response.Result.Content.ReadAsStringAsync();
        //    ReponseConvert = responseBody.Result.ToString();


        //    return retval;
        //}
        public List<PartialItemDetails> GetItemDetailsByBinName(BinDetailsRequest binName)
        {
            List<PartialItemDetails> details = new List<PartialItemDetails>();
            string token = GetAuthorizationToken().access_token;
            string ReponseConvert = string.Empty;

            var client = new HttpClient();
            string getItemDetailsUrl = ConfigurationManager.AppSettings["GetBinDetailsUrl"];
            var request = new HttpRequestMessage(HttpMethod.Get, getItemDetailsUrl + binName.BinName);
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", "Bearer " + token);
            request.Headers.Add("X-TenantId", "1");
            var response = client.SendAsync(request).Result;
            String responseBody = response.Content.ReadAsStringAsync().Result;
            

            JArray jsonArray = JArray.Parse(responseBody);
            foreach (JObject item in jsonArray)
            {
                string skuName = item["sku"]["name"].ToString();
                string batchNumber = item["batchNumber"].ToString();
                int qty = (int)item["qty"];
                details.Add(new PartialItemDetails { ItemDescription = skuName, BatchID = batchNumber, Qty = qty, BinName = binName.BinName });
            }
            return details;
        }


        private AuthorizationWMS GetAuthorizationToken()
        {
            AuthorizationWMS authorization = new AuthorizationWMS();
            string authorizationUrl = ConfigurationManager.AppSettings["AuthorizationUrl"];
            string username = ConfigurationManager.AppSettings["Username"];
            string password = ConfigurationManager.AppSettings["Password"];

            string jsonData = JsonConvert.SerializeObject(new
            {
                username = username,
                password = password
            });

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(authorizationUrl);

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://192.168.100.18:8082/wms/api/auth/authorize");
            //string jsonData = "{\r\n    \"username\": \"ashutosh.sahib@psl.co.in\",\r\n    \"password\": \"pass123\"\r\n}";

            request.Method = "POST";
            request.ContentType = "application/json";
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);
            request.ContentLength = byteArray.Length;


            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }


            HttpWebResponse response = (HttpWebResponse)request.GetResponse();


            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream);
                string apiResponse = reader.ReadToEnd();
                authorization = JsonConvert.DeserializeObject<AuthorizationWMS>(apiResponse);
            }


            response.Close();

            return authorization;
        }
        public string GetWMSSyncData(string URL, string Token, int warehouseid)
        {
            string ReponseConvert = string.Empty;
            try
            {

                var client = new HttpClient();
                client.DefaultRequestHeaders.ConnectionClose = true;
                var request = new HttpRequestMessage(HttpMethod.Get, URL);
                request.Headers.Add("Accept", "*/*");
                request.Headers.Add("Authorization", "Bearer " + Token);
                request.Headers.Add("X-TenantId", warehouseid.ToString());
                try
                {
                    var response = client.SendAsync(request);
                    //response.EnsureSuccessStatusCode();
                    //ReponseConvert =response.Content.ReadAsStringAsync();
                    Task<string> responseBody = response.Result.Content.ReadAsStringAsync();
                    ReponseConvert = responseBody.Result.ToString();
                }
                catch (HttpRequestException ex)
                {
                    // handle exception
                   
                    throw;
                }
            }
            catch (Exception ex)
            {
                
            }
            return ReponseConvert;

        }

        private string ExistsSOCheckQuery = "SELECT CASE WHEN EXISTS (SELECT * FROM SalesOrder WHERE DeliveryNo='@CheckDataVar') THEN 1 ELSE 0 END AS IsDataExists";
        private string ExistsSOItemsCheckQuery = "SELECT CASE WHEN EXISTS (SELECT * FROM SalesOrderLineItems SOL, SalesOrder SO WHERE SO.SOID = SOL.SOID AND SO.DeliveryNo='@dispatchNo' AND SOL.SalesDocumentItem = '@orderItemNo' AND ItemCode = '@skuCode') THEN 1 ELSE 0 END AS IsDataExists";
        public bool CheckifEntryExists(string Value, string Query)
        {
            bool retval = false;
            Query = Query.Replace("@CheckDataVar", Value);
            using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(Query, connection);

                connection.Open();
                retval = Convert.ToBoolean(command.ExecuteScalar());
                connection.Close();
            }

            return retval;
        }
        public bool CheckifLineItemsExists(string dispatchNo, string orderItemNo, string skuCode, string Query)
        {
            bool retval = false;
            Query = Query.Replace("@dispatchNo", dispatchNo);
            Query = Query.Replace("@orderItemNo", orderItemNo);
            Query = Query.Replace("@skuCode", skuCode);
            using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(Query, connection);

                connection.Open();
                retval = Convert.ToBoolean(command.ExecuteScalar());
                connection.Close();
            }

            return retval;
        }
        public bool InsetSODetails(DispatchData SO)
        {
            bool retval = false;
            try
            {
                using (SqlConnection db = new SqlConnection(AppSettings.ConnectionString))
                {
                    db.Open();

                    foreach (var b in SO.items)
                    {

                        using (SqlCommand command = db.CreateCommand())
                        {

                            if (CheckifEntryExists(SO.dispatchNo, ExistsSOCheckQuery))
                            {
                                if(CheckifLineItemsExists(SO.dispatchNo, b.orderItemNo, b.skuCode, ExistsSOItemsCheckQuery))
                                {
                                    try
                                    {
                                        UpdateLineItemsforSO(SO.dispatchNo, b.orderItemNo, b.skuCode, b.recordStatus, b.qty);
                                    }
                                    catch (Exception ex)
                                    {
                                        LogManager.Logger.LogError("WMS Items Update Function() ERROR.:" + ex.ToString());
                                    }
                                }
                                else
                                {
                                    string soID = string.Empty;
                                    try
                                    {
                                        string query = "SELECT * FROM SalesOrder WHERE DeliveryNo = '@dcNo'";
                                        using (SqlCommand getCmnd = new SqlCommand(query, db))
                                        {
                                            getCmnd.Parameters.AddWithValue("@dcNo", SO.dispatchNo);
                                            using (SqlDataReader reader = getCmnd.ExecuteReader())
                                            {
                                                while (reader.Read())
                                                {
                                                    if (reader["SOID"] != DBNull.Value)
                                                        soID = Convert.ToString(reader["SOID"]);
                                                }
                                            }
                                        }
                                    } 
                                    catch(Exception ex)
                                    {

                                    }

                                    command.CommandText = INSERT_SOLineItems;
                                    command.CommandType = System.Data.CommandType.Text;
                                    command.Parameters.AddWithValue("@ID", Guid.NewGuid());
                                    command.Parameters.AddWithValue("@SOID_ITEMS", soID);
                                    command.Parameters.AddWithValue("@SalesDocumentItem", b.orderItemNo);
                                    command.Parameters.AddWithValue("@OrderQty", b.qty);
                                    command.Parameters.AddWithValue("@ItemCode", b.skuCode);
                                    command.Parameters.AddWithValue("@ItemDescription", b.skuName);
                                    command.Parameters.AddWithValue("@LoadedQty", 0);
                                    command.Parameters.AddWithValue("@RecordStatus", b.recordStatus);
                                    command.Parameters.AddWithValue("@IsActive", 1);
                                    command.Parameters.AddWithValue("@IsProcessed", 1);
                                    command.Parameters.AddWithValue("@ModifiedDateTime", DateTime.Now);
                                    command.Parameters.AddWithValue("@ServerDateTime", DateTime.Now);
                                    command.ExecuteNonQuery();

                                }
                                
                            }
                            retval = true;
                            // Check Error
                        }
                    }
                }

                return retval;
            }

            catch (Exception ex)
            {
                throw new Exception("InsetSODetails() :--", ex);
                retval = false;
                return retval;
            }
            finally
            {

            }
        }
        public bool UpdateLineItemsforSO(string dispatchNo, string orderItemNo, string skuCode, string recordStatus, double qty)
        {
            bool retValue = false;

            string existingrecordStatus = string.Empty;
            double existingQty = 0.0;
            string Query = "Select SOL.RecordStatus, SOL.OrderQty from SalesOrderLineItems SOL, SalesOrder SO where SO.SOID = SOL.SOID and SO.DeliveryNo = '" + dispatchNo + "' and SOL.SalesDocumentItem = '" + orderItemNo + "' and SOL.ItemCode = '" + skuCode + "'";
            using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(Query, connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["RecordStatus"] != DBNull.Value)
                            existingrecordStatus = Convert.ToString(reader["RecordStatus"]);
                        if (reader["OrderQty"] != DBNull.Value)
                            existingQty = Convert.ToDouble(reader["OrderQty"]);
                    }
                }
            }

            if (existingrecordStatus != recordStatus || existingQty != qty)
            {
                string updateQuery = "Update SOL set SOL.RecordStatus = '" + recordStatus + "', SOL.OrderQty = '" + qty + "' from SalesOrderLineItems SOL, SalesOrder SO where SO.SOID = SOL.SOID and SO.DeliveryNo = '" + dispatchNo + "' and SOL.SalesDocumentItem = '" + orderItemNo + "' and SOL.ItemCode = '" + skuCode + "'";
                using (SqlConnection connection = new SqlConnection(AppSettings.ConnectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(updateQuery, connection);
                    command.ExecuteNonQuery();
                    retValue = true;
                }
            }
            return retValue;
        }


        #endregion
    }
}
