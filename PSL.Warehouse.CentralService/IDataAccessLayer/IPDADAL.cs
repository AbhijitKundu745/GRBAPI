using PSL.Warehouse.CentralService.DBContext;
using PSL.Laundry.CentralService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSL.Warehouse.CentralService.Models;

namespace PSL.Warehouse.CentralService.IDataAccessLayer
{
    public interface IPDADAL
    {
        List<CustomerMaster> GetAllCustomers();

        List<Vendor> GetAllVendors(Guid tenantID);

        List<LostAssets> GetAllRCOAssets(Guid tenantID);

        List<AssetTypeMaster> GetAllAssetType(Guid tenantID);


        List<RoomMasterModel> GetAllRooms(Guid tenantID);

        List<AssetMaster> GetAllAssets(Guid tenantID);
        List<TouchPointModel> GetAllTouchPoints(Guid tenantID);

        User GetUserData(User user);

        LoginResponse MobileLogin(LoginModel login);

        bool createActivity(Activity activity);

        bool registerAsset(RegisterModel registerModel);
        //List<AssetMasterResponse> GetAllAssetsforMobile(Guid tenantID);
        List<AssetMasterResponse> GetAllAssetsforMobile(Guid tenantID);
        bool RegisterAssetMobile(RegisterModelMobile registerModel);
        //bool CreateActivityMobile(ActivityModel activity);
        ActivityResponseData CreateActivityMobile(ActivityModel activity);

        //WareHouse
        List<AssetInfo> GetAssetInfo(UserData user);
        List<PalletsDetails> GetPalletDataByID(PalletData pallet);
        List<ContainersDetails> GetContainerDataByID(ContainersData containers);
        bool InsertTransactionDetails(TransactionDetails transactionDetails);
        bool InsertTagLoggerDetails(TagDetailsRequest transactionDetails);
        ReaderWorkorderListResponse GetReaderWorkorderList(ReaderWorkorderListRequest readerWorkorderList);
        List<AssetMaster> GetWorkOrderDetails(string DeviceID);
        //List<AssetMasterResponse> GetAllAssetsforMobile(UserData user);
        //List<AssetMasterResponse> GetAllAssetsforMobile(object user);
        bool CheckCredentials(LoginRequest request);
        //WorkOrderResponse GetReaderStatus(DeviceData deviceData);
        ReaderStatusinfo GetReaderStatus(DeviceData deviceData);
        //List<Workorderlist> GetWorkorderlists(UserData user);
        List<Workorderlist> GetWorkorderlists(DeviceData deviceData);
        WorkorderStatusResponse UpdateWorkorderStatus(WorkorderStatusRequest workorderStatus);
        //WorkorderListResponse GetWorkorderListItems(WorkorderListRequest workorderItems);
        bool WebLogin(UserModel user);
        List<TruckDetails> GetTruckDetails();
        List<PalletItemDetails> GetPalletWithItemDetails(TruckDetails truckDetails);
        //bool InsertWorkorderDetails(WorkorderCreationDetails workorderCreation);
        TruckResponseDetails GetTruckDetailsForPDA(TruckIDDetails truckID);
        List<LocationDetails> GetLocationName();
        List<TruckResponseDetailsByLocation> GetPalletItemDetailsByLocationName(LocationDetails locationDetails);
        List<ItemDescriptionDetails> GetSTOLineItemsByTruckID(TruckDetails truckNumber);
        ActivityResponseData InsertTransactionForItems(ActivityModelItems activityItems);
        bool receivingWorkOrder(WorkorderCreationDetailsU1 workorderCreation);

        bool InsertWorkorderDetailsU0(WorkorderCreationDetails workorderCreation);
        WorkorderListResponse GetWorkorderListItemsV1(WorkorderListRequest workorderItems);
        bool UploadCurrentPalletV1(CurrentPalletRequest currentPallet);
        bool StartReader(ReaderStartRequset startRequset);
        List<PartialWorkOrderResponse> GetPartialWorkorderList(DeviceData deviceData);
        List<PickListItemDetails> GetPartialWorkorderItemDetails(PartialWorkorderListRequest listRequest);
        bool InsertPartialWorkorderDetails(PartialTransactionDetails transactionDetails);
        List<PartialItemDetails> GetItemDetailsByBinName(BinDetailsRequest binDetails);
        bool dispatchWorkOrder(WorkorderCreationDetailsL0 workorderCreationL0);
        bool InsertWorkorderDetailsL1(WorkorderCreationDetails workorderCreation);
        List<PendingWorkOrderDetails> GetPendingWorkorderDetails();
        bool InsertTransactionDetailsForBotService(TransactionDetailsV1 transactionDetails);
        List<OrderDetails> GetWorkorderListForPDA(PDAWorkorderList workorderList);
        bool CloseWorkorderU0(CloseWorkorderRequest request);
        bool UploadCurrentBin(CurrentBinRequest currentBin);
        bool internalWorkOrder(WorkorderCreationDetailsI0 workorderCreation);


        //List<WorkorderStatusResponse> GetReaderStatus(string ClientDeviceID);
        //bool WorkorderStateUpdate(WorkorderStatusRequest workorderState);
        //List<PalletsDetails> GetPalletName(UserData clientData);







        //List<TaskMaster> GetTaskMaster(DateTime? ModifiedDateTime);

        //List<TransactionType> GetTansactionTypes(DateTime? ModifiedDateTime);

        //List<PDAUser> GetUsers(DateTime? ModifiedDateTime);

        //List<PDASourcePlant> GetSourcePlants(DateTime? ModifiedDateTime);

        //List<TouchPoint> GetTouchPoints(DateTime? ModifiedDateTime);

        //List<Location> GetLocations(DateTime? ModifiedDateTime);

        //int InsertTripDetail(PDATripDetail tripdetail);

        //TripDetailsInfo GetLastTripByTruckId(string truckId);
    }
}
