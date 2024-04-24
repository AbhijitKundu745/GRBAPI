using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PSL.Warehouse.CentralService.Global
{
    public static class AppSettings
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string ConnectionString { get; private set; }
        public static bool EmailNotification { get; private set; }
        public static bool SMSNotification { get; private set; }

        public static void loadAppSettings()
        {
            try
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString.ToString().Trim();
                EmailNotification = Convert.ToBoolean(ConfigurationManager.AppSettings["EmailNotification"].ToString().Trim());
                SMSNotification = Convert.ToBoolean(ConfigurationManager.AppSettings["SMSNotification"].ToString().Trim());
                try
                {
                    SQLQueryCommand.loadSQLQueryCommands();
                }
                catch (Exception ex)
                {
                    Log.Error("Error while loading query commands", ex);
                }

                try
                {
                    EmailSettings.LoadEmailSettings();
                }
                catch (Exception ex)
                {
                    Log.Error("Error while loading email settings", ex);
                }

                try
                {
                    SMSSettings.LoadSMSSettings();
                }
                catch (Exception ex)
                {
                    Log.Error("Error while loading sms settings", ex);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in loadAppSettings", ex);
            }
        }

        public static class EmailSettings
        {
            public static string host { get; private set; }
            public static int port { get; private set; } = 0;
            public static bool enableSSL { get; private set; } = false;
            public static bool UseDefaultCredentials { get; private set; } = false;
            public static string username { get; private set; }
            public static string password { get; private set; }
            public static int timeout { get; private set; } = 5000;
            public static string fromMailID { get; private set; }
            public static List<string> toMailIDs { get; private set; }
            public static List<string> ccMailIDs { get; private set; }

            public static void LoadEmailSettings()
            {
                try
                {
                    if (!EmailNotification)
                        return;

                    var emailSettings = ConfigurationManager.GetSection("emailSettings") as NameValueCollection;
                    if (emailSettings != null && emailSettings.HasKeys())
                    {
                        if (emailSettings.AllKeys.Contains("host"))
                        {
                            host = Convert.ToString(emailSettings["host"]);
                        }
                        if (emailSettings.AllKeys.Contains("port"))
                        {
                            port = Convert.ToInt32(emailSettings["port"]);
                        }
                        if (emailSettings.AllKeys.Contains("enableSSL"))
                        {
                            enableSSL = Convert.ToBoolean(emailSettings["enableSSL"]);
                        }
                        if (emailSettings.AllKeys.Contains("UseDefaultCredentials"))
                        {
                            UseDefaultCredentials = Convert.ToBoolean(emailSettings["UseDefaultCredentials"]);
                        }
                        if (emailSettings.AllKeys.Contains("username"))
                        {
                            username = Convert.ToString(emailSettings["username"]);
                        }
                        if (emailSettings.AllKeys.Contains("password"))
                        {
                            password = Convert.ToString(emailSettings["password"]);
                        }
                        if (emailSettings.AllKeys.Contains("timeout"))
                        {
                            timeout = Convert.ToInt32(emailSettings["timeout"]);
                        }
                        if (emailSettings.AllKeys.Contains("fromMailID"))
                        {
                            fromMailID = Convert.ToString(emailSettings["fromMailID"]);
                        }
                        if (emailSettings.AllKeys.Contains("toMailIDs"))
                        {
                            string to = Convert.ToString(emailSettings["toMailIDs"]);
                            if (!string.IsNullOrEmpty(to))
                            {
                                toMailIDs = to.Split(',').ToList();
                            }
                        }
                        if (emailSettings.AllKeys.Contains("ccMailIDs"))
                        {
                            string CC = Convert.ToString(emailSettings["ccMailIDs"]);
                            if (!string.IsNullOrEmpty(CC))
                            {
                                ccMailIDs = CC.Split(',').ToList();
                            }
                        }

                    }

                    if (string.IsNullOrEmpty(AppSettings.EmailSettings.username) ||
                        string.IsNullOrEmpty(AppSettings.EmailSettings.password) || string.IsNullOrEmpty(AppSettings.EmailSettings.host))
                        throw new Exception("could not send mail because of invalid parameters..please check parameters");
                }
                catch (Exception ex)
                {
                    //EmailNotification = false;
                    throw new Exception("Error at loading mailSettings" + ex.Message);
                }
            }

        }

        public static class StoredProcedure
        {

            //Login
            public static string GetUserDetails { get; set; } = "Prc_DesktopLogin";

            //AssetRegUC
            public static string GetAllAsset { get; set; } = "PSL_GetAllAssets";
            public static string DeleteRegisteredAssets { get; set; } = "Prc_DeleteRegisteredAsset";
            public static string UpdateRegisteredAssets { get; set; } = "Prc_UpdateRegisteredAsset";
            public static string GetAssetTypeByCustomer { get; set; } = "Prc_GetAssetTypeByCustomer";
            public static string InsertAssetRegistration { get; set; } = "Prc_InsertAssetRegistration";


            //AssetMapping

            public static string GetAssetsForMapping { get; set; } = "Prc_GetAssetsForMapping";
            public static string GetRegisteredAssets { get; set; } = "Prc_GetRegisteredAssets";
            public static string GetSID { get; set; } = "Prc_GetSID";
            public static string MapAssets { get; set; } = "Prc_MapAssets";

            //Processes
            public static string GetVendors { get; set; } = "Prc_GetVendors";
            public static string GetAssetLife { get; set; } = "Prc_GetAssetLife";
            public static string InsertActivity { get; set; } = "Prc_InsertActivity";
            public static string GetTagDetails { get; set; } = "Prc_GetTagDetails";
            public static string InsertActivityDetails { get; set; } = "Prc_InsertActivityDetails";
            public static string InsertErrorActivity { get; set; } = "Prc_InsertErrorActivity";
            public static string InsertSerialNo { get; internal set; } = "PSL_InsertSerialNo";
            public static string UpdateChipID { get; internal set; } = "PSL_UpdateChipID";
        }
        public static class SMSSettings
        {
            public static string url { get; private set; }
            public static string APIKey { get; private set; }
            public static string SenderId { get; private set; }
            public static void LoadSMSSettings()
            {
                try
                {
                    if (!SMSNotification)
                        return;

                    var smsSettings = ConfigurationManager.GetSection("smsSettings") as NameValueCollection;
                    if (smsSettings != null && smsSettings.HasKeys())
                    {
                        if (smsSettings.AllKeys.Contains("url"))
                        {
                            url = Convert.ToString(smsSettings["url"]);
                        }
                        if (smsSettings.AllKeys.Contains("APIKey"))
                        {
                            APIKey = Convert.ToString(smsSettings["APIKey"]);
                        }
                        if (smsSettings.AllKeys.Contains("SenderId"))
                        {
                            SenderId = Convert.ToString(smsSettings["SenderId"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error at loading smsSettings" + ex.Message);
                }
            }


        }

        public static class SQLQueryCommand
        {


            public static string SP_Get_AssetInfo { get; private set; }
            public static string SP_Get_PalletDetails { get; private set; }
            public static string SP_Get_ContainerDetails { get; private set; }
            public static string SP_InsertReaderTranslogitems { get; private set; }
            public static string SP_InsertReaderTransLog { get; private set; }
            public static string SP_InsertReaderTransLog1 { get; private set; }
            public static string SP_GetReaderWorkorderList { get; private set; }
            public static string SP_UploadCurrentPallet { get; private set; }



            public static string SP_Get_CustomerDetails { get; private set; }
            public static string SP_Get_AllVendors { get; private set; }
            public static string SP_GetAllRCOAssets { get; private set; }
            public static string SP_Get_AllAssetTypes { get; private set; }

            public static string SP_Get_AllAssets { get; private set; }
            public static string SP_Get_Touchpoints { get; private set; }

            public static string SP_Get_AllRooms { get; private set; }
            public static string Login { get; private set; }
           
            public static string MobileLogin { get; private set; }
            public static string SP_Get_AllAssets_Mobile { get; private set; }
            public static string SP_InsertActivity { get; private set; }
            public static string SP_InsertActivityDetails { get; private set; }

            public static string TabLogin { get; private set; }
            public static string SP_GetStatusDetails { get; private set; }
            public static string SP_GetWorkorderLists { get; private set; }
            public static string SP_UpdateWorkorderStatus { get; private set; }
            public static string SP_GetWorkorderListItems { get; private set; }
            public static string SP_WorkorderStateUpdate { get; private set; }
            public static string SP_GetPalletName { get; private set; }
            public static string SP_StartReader { get; private set; }

            public static string SP_WebLogin { get; private set; }
            public static string SP_GetTruckDetails { get; private set; }
            public static string SP_GetPalletWithItemDetails { get; private set; }
            public static string SP_GetPalletDetailsWithTruckID { get; private set; }
            public static string SP_CreateWorkOrder { get; private set; }
            public static string SP_CreateWorkOrderDetails { get; private set; }
            public static string SP_GetTruckDetailsForPDA { get; private set; }
            public static string SP_GetLocationName { get; private set; }
            public static string SP_GetTruckDetailsByLocationName { get; private set; }
            public static string SP_GetSTOLineItemsByTruckID { get; private set; }
            public static string SP_InsertActivityDetailsForItems { get; private set; }
            public static string SP_CreateWorkOrderU1 { get; private set; }
            public static string SP_CreateWorkOrderDetailsU1 { get; private set; }
            public static string SP_GetPalletInfo { get; private set; }
            public static string SP_GetPalletInfoDis { get; private set; }
            public static string SP_GetPalletBinMappedInfo { get; private set; }
            public static string SP_GetPalletBinMappedInfoL0 { get; private set; }
            public static string SP_GetPalletBinMappedItemsL0 { get; private set; }
            public static string SP_UploadCurrentBin { get; private set; }


            public static string SP_CreateWorkOrderU0 { get; private set; }
            public static string SP_CreateWorkOrderDetailsU0 { get; private set; }
            public static string SP_CreateWorkOrderL0 { get; private set; }
            public static string SP_CreateWorkOrderDetailsL0 { get; private set; }
            public static string SP_CreateWorkOrderL1 { get; private set; }
            public static string SP_CreateWorkOrderDetailsL1 { get; private set; }
            public static string SP_GetPartialWorkorderList { get; private set; }
            public static string SP_GetPartialWorkorderItemDetails { get; private set; }
            public static string SP_InsertPartialWorkorder { get; private set; }
            public static string SP_InsertPartialWorkorderItems { get; private set; }
            public static string SP_GetPendingWorkorderDetails { get; private set; }
            public static string SP_InsertReaderTransLogForService { get; private set; }
            public static string SP_InsertReaderTranslogitemsForService { get; private set; }
            public static string SP_GetWorkorderListForPDA { get; private set; }
            public static string SP_CloseWorkorderU0 { get; private set; }

            public static string SP_InsertWMSData { get; private set; }
            public static string SP_CreateWorkOrderI0 { get; private set; }
            public static string SP_CreateWorkOrderDetailsI0 { get; private set; }


            public static string SP_Insert_ApprovalRecord { get; private set; }
            public static string SP_ApproveTransaction { get; private set; }
            public static string SP_Get_ApprovalRecord { get; private set; }
            public static string SP_Insert_Tripdetail { get; private set; }
            public static string SP_Insert_Transactiondetail { get; private set; }
            public static string SP_Get_CastDetails { get; private set; }
            public static string SP_Get_GETTasks { get; private set; }
            public static string SP_GET_TansactionTypes { get; private set; }
            public static string SP_GET_PDAUsers { get; private set; }
            public static string SP_GET_PDASourcePlants { get; private set; }
            public static string SP_GET_TouchPoints { get; private set; }
            public static string SP_GET_Locations { get; private set; }
            public static string SP_GET_PreviousTripDetails { get; private set; }
            public static string SP_GET_LastTripDetailsbyTruckID { get; set; }
            public static string SP_Insert_PDA_TripDetails { get; private set; }
            public static string SP_GET_NotificationConfiguration { get; private set; }


            public static void loadSQLQueryCommands()
            {
                try
                {
                    SP_Get_AssetInfo = ConfigurationManager.AppSettings["SP_Get_AssetInfo"].ToString().Trim();
                    SP_Get_PalletDetails = ConfigurationManager.AppSettings["SP_Get_PalletDetails"].ToString().Trim();
                    SP_Get_ContainerDetails= ConfigurationManager.AppSettings["SP_Get_ContainerDetails"].ToString().Trim();
                    SP_InsertReaderTranslogitems = ConfigurationManager.AppSettings["SP_InsertReaderTranslogitems"].ToString().Trim();
                    SP_InsertReaderTransLog = ConfigurationManager.AppSettings["SP_InsertReaderTransLog"].ToString().Trim();
                    SP_InsertReaderTransLog1 = ConfigurationManager.AppSettings["SP_InsertReaderTransLog1"].ToString().Trim();
                    SP_GetReaderWorkorderList = ConfigurationManager.AppSettings["SP_GetReaderWorkorderList"].ToString().Trim();
                    SP_UploadCurrentPallet = ConfigurationManager.AppSettings["SP_UploadCurrentPallet"].ToString().Trim();
                    SP_GetStatusDetails = ConfigurationManager.AppSettings["SP_GetStatusDetails"].ToString().Trim();
                    SP_GetWorkorderLists = ConfigurationManager.AppSettings["SP_GetWorkorderLists"].ToString().Trim();
                    SP_UpdateWorkorderStatus = ConfigurationManager.AppSettings["SP_UpdateWorkorderStatus"].ToString().Trim();
                    SP_GetWorkorderListItems = ConfigurationManager.AppSettings["SP_GetWorkorderListItems"].ToString().Trim();
                    SP_WorkorderStateUpdate = ConfigurationManager.AppSettings["SP_WorkorderStateUpdate"].ToString().Trim();
                    SP_GetPalletName = ConfigurationManager.AppSettings["SP_GetPalletName"].ToString().Trim();
                    SP_WebLogin = ConfigurationManager.AppSettings["SP_WebLogin"].ToString().Trim();
                    SP_GetTruckDetails = ConfigurationManager.AppSettings["SP_GetTruckDetails"].ToString().Trim();
                    SP_GetPalletWithItemDetails = ConfigurationManager.AppSettings["SP_GetPalletWithItemDetails"].ToString().Trim();
                    SP_GetPalletDetailsWithTruckID = ConfigurationManager.AppSettings["SP_GetPalletDetailsWithTruckID"].ToString().Trim();
                    SP_CreateWorkOrder = ConfigurationManager.AppSettings["SP_CreateWorkOrder"].ToString().Trim();
                    SP_CreateWorkOrderDetails = ConfigurationManager.AppSettings["SP_CreateWorkOrderDetails"].ToString().Trim();
                    SP_GetTruckDetailsForPDA = ConfigurationManager.AppSettings["SP_GetTruckDetailsForPDA"].ToString().Trim();
                    SP_InsertActivity = ConfigurationManager.AppSettings["SP_InsertActivity"].ToString().Trim();
                    SP_InsertActivityDetails = ConfigurationManager.AppSettings["SP_InsertActivityDetails"].ToString().Trim();
                    SP_GetLocationName = ConfigurationManager.AppSettings["SP_GetLocationName"].ToString().Trim();
                    SP_GetTruckDetailsByLocationName = ConfigurationManager.AppSettings["SP_GetTruckDetailsByLocationName"].ToString().Trim();
                    SP_GetSTOLineItemsByTruckID = ConfigurationManager.AppSettings["SP_GetSTOLineItemsByTruckID"].ToString().Trim();
                    SP_InsertActivityDetailsForItems = ConfigurationManager.AppSettings["SP_InsertActivityDetailsForItems"].ToString().Trim();
                    SP_CreateWorkOrderU1 = ConfigurationManager.AppSettings["SP_CreateWorkOrderU1"].ToString().Trim();
                    SP_CreateWorkOrderDetailsU1 = ConfigurationManager.AppSettings["SP_CreateWorkOrderDetailsU1"].ToString().Trim();
                    SP_GetPalletInfo = ConfigurationManager.AppSettings["SP_GRB_GETPalletInfo"].ToString().Trim();
                    SP_GetPalletInfoDis = ConfigurationManager.AppSettings["SP_GetPalletInfoDis"].ToString().Trim();
                    SP_GetPalletBinMappedInfo = ConfigurationManager.AppSettings["SP_GRB_GetPalletBinMappedInfo"].ToString().Trim();
                    SP_GetPalletBinMappedInfoL0 = ConfigurationManager.AppSettings["SP_GetPalletBinMappedInfoL0"].ToString().Trim();
                    SP_GetPalletBinMappedItemsL0 = ConfigurationManager.AppSettings["SP_GetPalletBinMappedItemsL0"].ToString().Trim();
                    SP_StartReader = ConfigurationManager.AppSettings["SP_StartReader"].ToString().Trim();
                    SP_GetPartialWorkorderList = ConfigurationManager.AppSettings["SP_GetPartialWorkorderList"].ToString().Trim();
                    SP_GetPartialWorkorderItemDetails = ConfigurationManager.AppSettings["SP_GetPartialWorkorderItemDetails"].ToString().Trim();
                    SP_InsertPartialWorkorder = ConfigurationManager.AppSettings["SP_InsertPartialWorkorder"].ToString().Trim();
                    SP_InsertPartialWorkorderItems = ConfigurationManager.AppSettings["SP_InsertPartialWorkorderItems"].ToString().Trim();
                    SP_GetPendingWorkorderDetails = ConfigurationManager.AppSettings["SP_GetPendingWorkorderDetails"].ToString().Trim();
                    SP_InsertReaderTransLogForService = ConfigurationManager.AppSettings["SP_InsertReaderTransLogForService"].ToString().Trim();
                    SP_InsertReaderTranslogitemsForService = ConfigurationManager.AppSettings["SP_InsertReaderTranslogitemsForService"].ToString().Trim();
                    SP_GetWorkorderListForPDA = ConfigurationManager.AppSettings["SP_GetWorkorderListForPDA"].ToString().Trim();
                    SP_UploadCurrentBin = ConfigurationManager.AppSettings["SP_UploadCurrentBin"].ToString().Trim();
                    


                    SP_CreateWorkOrderU0 = ConfigurationManager.AppSettings["SP_CreateWorkOrderU0"].ToString().Trim();
                    SP_CreateWorkOrderDetailsU0 = ConfigurationManager.AppSettings["SP_CreateWorkOrderDetailsU0"].ToString().Trim();
                    SP_CreateWorkOrderL0 = ConfigurationManager.AppSettings["SP_CreateWorkOrderL0"].ToString().Trim();
                    SP_CreateWorkOrderDetailsL0 = ConfigurationManager.AppSettings["SP_CreateWorkOrderDetailsL0"].ToString().Trim();
                    SP_CreateWorkOrderL1 = ConfigurationManager.AppSettings["SP_CreateWorkOrderL1"].ToString().Trim();
                    SP_CreateWorkOrderDetailsL1 = ConfigurationManager.AppSettings["SP_CreateWorkOrderDetailsL1"].ToString().Trim();
                    SP_CloseWorkorderU0 = ConfigurationManager.AppSettings["SP_CloseWorkorderU0"].ToString().Trim();

                    SP_InsertWMSData = ConfigurationManager.AppSettings["SP_InsertWMSData"].ToString().Trim();
                    SP_CreateWorkOrderI0 = ConfigurationManager.AppSettings["SP_CreateWorkOrderI0"].ToString().Trim();
                    SP_CreateWorkOrderDetailsI0 = ConfigurationManager.AppSettings["SP_CreateWorkOrderDetailsI0"].ToString().Trim();




                    SP_Get_CustomerDetails = ConfigurationManager.AppSettings["SP_Get_CustomerDetails"].ToString().Trim();
                    SP_Get_AllVendors = ConfigurationManager.AppSettings["SP_Get_AllVendors"].ToString().Trim();
                    SP_GetAllRCOAssets = ConfigurationManager.AppSettings["SP_GetAllRCOAssets"].ToString().Trim();
                    SP_Get_AllAssetTypes = ConfigurationManager.AppSettings["SP_Get_AllAssetTypes"].ToString().Trim();
                    SP_Get_AllAssets = ConfigurationManager.AppSettings["SP_Get_AllAssets"].ToString().Trim();
                    SP_Get_Touchpoints = ConfigurationManager.AppSettings["SP_Get_Touchpoints"].ToString().Trim();
                    Login = ConfigurationManager.AppSettings["Login"].ToString().Trim();
                    TabLogin = ConfigurationManager.AppSettings["TabLogin"].ToString().Trim();
                    MobileLogin = ConfigurationManager.AppSettings["MobileLogin"].ToString().Trim();
                    SP_Get_AllAssets_Mobile = ConfigurationManager.AppSettings["SP_Get_AllAssets_Mobile"].ToString().Trim();
                    SP_Get_AllRooms = ConfigurationManager.AppSettings["SP_Get_AllRooms"].ToString().Trim();

                    SP_Get_ApprovalRecord = ConfigurationManager.AppSettings["SP_Get_ApprovalRecord"].ToString().Trim();
                    SP_Insert_ApprovalRecord = ConfigurationManager.AppSettings["SP_Insert_ApprovalRecord"].ToString().Trim();
                    SP_ApproveTransaction = ConfigurationManager.AppSettings["SP_ApproveTransaction"].ToString().Trim();
                    SP_Insert_Tripdetail = ConfigurationManager.AppSettings["SP_Insert_Tripdetail"].ToString().Trim();
                    SP_Insert_Transactiondetail = ConfigurationManager.AppSettings["SP_Insert_Transactiondetail"].ToString().Trim();
                    SP_Get_CastDetails = ConfigurationManager.AppSettings["SP_Get_CastDetails"].ToString().Trim();
                    SP_Get_GETTasks = ConfigurationManager.AppSettings["SP_Get_GETTasks"].ToString().Trim();
                    SP_GET_TansactionTypes = ConfigurationManager.AppSettings["SP_GET_TansactionTypes"].ToString().Trim();
                    SP_GET_PDAUsers = ConfigurationManager.AppSettings["SP_GET_PDAUsers"].ToString().Trim();
                    SP_GET_PDASourcePlants = ConfigurationManager.AppSettings["SP_GET_PDASourcePlants"].ToString().Trim();
                    SP_GET_TouchPoints = ConfigurationManager.AppSettings["SP_GET_TouchPoints"].ToString().Trim();
                    SP_GET_Locations = ConfigurationManager.AppSettings["SP_GET_locations"].ToString().Trim();
                    SP_GET_PreviousTripDetails = ConfigurationManager.AppSettings["SP_GET_PreviousTripDetails"].ToString().Trim();
                    SP_GET_LastTripDetailsbyTruckID = ConfigurationManager.AppSettings["SP_GET_LastTripDetailsbyTruckID"].ToString().Trim();
                    SP_Insert_PDA_TripDetails = ConfigurationManager.AppSettings["SP_Insert_PDA_TripDetails"].ToString().Trim();
                    SP_GET_NotificationConfiguration = ConfigurationManager.AppSettings["SP_GET_NotificationConfiguration"].ToString().Trim();

                }
                catch (Exception ex)
                {
                    throw new Exception("Error in loadSQLQueryCommands" + ex.Message);
                }
            }

        }

    }

}
