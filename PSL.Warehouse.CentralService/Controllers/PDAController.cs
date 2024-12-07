using PSL.Warehouse.CentralService.DBContext;
using PSL.Laundry.CentralService.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using PSL.Warehouse.CentralService.Logger;
using PSL.Warehouse.CentralService.IDataAccessLayer;
using PSL.Warehouse.CentralService.Models;
using System.Web.Http.Cors;
using System.Threading.Tasks;

namespace PSL.Warehouse.CentralService.Controllers
{
    [RoutePrefix("PDA")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PDAController : ApiController
    {
        #region properties
        private readonly ILogger<PDAController> Log;
        private readonly IPDADAL pdaDAO;
        #endregion

        public PDAController(ILogger<PDAController> logger, IPDADAL pdaDAO)
        {
            this.Log = logger;
            this.pdaDAO = pdaDAO;
        }


        [HttpGet, Route("GetAssetInfo")]
        public IHttpActionResult GetAssetInfo(UserData user)
        {
            ResponseData responseData = new ResponseData();
            List<AssetInfo> AssetInfos = null;
            try
            {
                AssetInfos = pdaDAO.GetAssetInfo(user);
                responseData.status = true;
                responseData.data = AssetInfos;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }



        [HttpGet, Route("GetPalletCheckByID")]
        public IHttpActionResult GetPalletCheckByID(PalletData pallet)
        {
            ResponseData responseData = new ResponseData();
            List<PalletsDetails> Checkdata = null;
            try
            {
                Checkdata = pdaDAO.GetPalletDataByID(pallet);
                responseData.status = true;
                responseData.data = Checkdata;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        [HttpGet, Route("GetContainerCheckByID")]
        public IHttpActionResult GetContainerCheckByID(ContainersData containers)
        {
            ResponseData responseData = new ResponseData();
            List<ContainersDetails> Checkdata = null;
            try
            {
                Checkdata = pdaDAO.GetContainerDataByID(containers);
                responseData.status = true;
                responseData.data = Checkdata;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        

        [HttpGet, Route("GetWorkOrderDetails")]
        public IHttpActionResult GetAllAssetTypes(string DeviceID)
        {
            ResponseData responseData = new ResponseData();
            List<AssetMaster> assettypes = null;
            try
            {
                assettypes = pdaDAO.GetWorkOrderDetails(DeviceID);
                responseData.status = true;
                responseData.data = assettypes;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }


        [HttpGet, Route("GetAllCustomers")]
        public IHttpActionResult GetAllCustomers()
        {
            ResponseData responseData = new ResponseData();
            List<CustomerMaster> customerDetails = null;
            try
            {
                customerDetails = pdaDAO.GetAllCustomers();
                responseData.status = true;
                responseData.data = customerDetails;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        [HttpGet, Route("GetAllVendors")]
        public IHttpActionResult GetAllVendors(Guid tenantID)
        {
            ResponseData responseData = new ResponseData();
            List<Vendor> locations = null;
            try
            {
                locations = pdaDAO.GetAllVendors(tenantID);
                responseData.status = true;
                responseData.data = locations;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        [HttpGet, Route("GetLostAssets")]
        public IHttpActionResult GetAllRCOAssets(Guid tenantID)
        {
            ResponseData responseData = new ResponseData();
            List<LostAssets> data = null;
            try
            {
                data = pdaDAO.GetAllRCOAssets(tenantID);
                responseData.status = true;
                responseData.data = data;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        [HttpGet, Route("GetAllAssetTypes")]
        public IHttpActionResult GetAllAssetTypes(Guid tenantID)
        {
            ResponseData responseData = new ResponseData();
            List<AssetTypeMaster> assettypes = null;
            try
            {
                assettypes = pdaDAO.GetAllAssetType(tenantID);
                responseData.status = true;
                responseData.data = assettypes;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        [HttpGet, Route("GetAllRooms")]
        public IHttpActionResult GetAllRooms(Guid tenantID)
        {
            ResponseData responseData = new ResponseData();
            List<RoomMasterModel> rooms = null;
            try
            {
                rooms = pdaDAO.GetAllRooms(tenantID);
                responseData.status = true;
                responseData.data = rooms;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }


        [HttpGet, Route("GetAllAssets")]
        public IHttpActionResult GetAllAssets(Guid tenantID)
        {
            ResponseData responseData = new ResponseData();
            List<AssetMaster> assets = null;
            try
            {
                assets = pdaDAO.GetAllAssets(tenantID);
                responseData.status = true;
                responseData.data = assets;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        [HttpGet, Route("GetAllTouchPoints")]
        public IHttpActionResult GetAllTouchPoints(Guid tenantID)
        {
            ResponseData responseData = new ResponseData();
            List<TouchPointModel> assets = null;
            try
            {
                assets = pdaDAO.GetAllTouchPoints(tenantID);
                responseData.status = true;
                responseData.data = assets;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        [HttpPost, Route("Transaction")]
        public IHttpActionResult Transaction(Activity activity)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            bool result;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                result = pdaDAO.createActivity(activity);
                responseData.status = true;
                responseData.data = result;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        [HttpPost, Route("Login")]
        public IHttpActionResult Login(User user)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            User userdetails;
            try
            {
                userdetails = pdaDAO.GetUserData(user);
                responseData.status = true;
                responseData.data = userdetails;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        /*Warehouse Code Starts*/
        #region PDA
        [HttpPost, Route("MobileLogin")]
        public IHttpActionResult MobileLogin(LoginModel login)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            LoginResponse userdetails;
            try
            {
                userdetails = pdaDAO.MobileLogin(login);
                responseData.status = true;
                responseData.data = userdetails;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        [HttpGet, Route("GetAllAssetsMobile")]
        public IHttpActionResult GetAllAssetsMobile(Guid tenantID)
        {
            ResponseData responseData = new ResponseData();
            List<AssetMasterResponse> assets = null;
            try
            {
                assets = pdaDAO.GetAllAssetsforMobile(tenantID);
                responseData.status = true;
                responseData.data = assets;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        [HttpPost, Route("TransactionMobile")]
        public IHttpActionResult TransactionMobile(ActivityModel activity)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            ActivityResponseData listdata = null;
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return BadRequest(ModelState);
                //}

                listdata = pdaDAO.CreateActivityMobile(activity);

                if (listdata.status)
                {
                    if (listdata.isUnique)
                    {
                        if (listdata.isItemUnique)
                        {
                            responseData.status = true;
                            responseData.message = "The pallet has been mapped successfully";
                            Log.Info(listdata.wmsData);
                        }
                        else
                        {
                            responseData.status = false;
                            responseData.message = listdata.message;
                        }
                    }
                    else
                    {
                        responseData.status = false;
                        responseData.message = listdata.message;
                    }
                }
                else
                {
                    responseData.status = false;
                    responseData.message = listdata.message;
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }
        [HttpPost, Route("GetTruckDetailsForPDA")]
        public IHttpActionResult GetTruckDetailsForPDA([FromBody] TruckIDDetails truckID)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            TruckResponseDetails data = null;

            try
            {
                data = pdaDAO.GetTruckDetailsForPDA(truckID);
                responseData.status = true;
                responseData.message = "";
                responseData.data = data;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }

            return Ok(responseData);
        }
        [HttpPost, Route("GetSTOLineItemsByTruckID")]
        public IHttpActionResult GetSTOLineItemsByTruckID([FromBody] TruckDetails truckNumber)
        {
            ResponseData responseData = new ResponseData();
            List<ItemDescriptionDetails> data = null;

            try
            {
                data = pdaDAO.GetSTOLineItemsByTruckID(truckNumber);
                responseData.status = true;
                responseData.message = "";
                responseData.data = data;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }

            return Ok(responseData);
        }
        [HttpPost, Route("InsertTransactionForItems")]
        public IHttpActionResult InsertTransactionForItems(ActivityModelItems activityItems)
        {
            Response response = new Response();
            ActivityResponseData responseData = null;
            //bool retVal;
            try
            {
                responseData = pdaDAO.InsertTransactionForItems(activityItems);
                if (responseData.status)
                {
                    if (responseData.isUnique)
                    {
                        if (responseData.isItemUnique)
                        {
                            response.status = true;
                            response.message = "The pallet has been mapped successfully";
                            Log.Info(responseData.wmsData);
                        }
                        else
                        {
                            response.status = false;
                            response.message = responseData.message;
                        }
                    }
                    else
                    {
                        response.status = false;
                        response.message = responseData.message;
                    }
                }
                else
                {
                    response.status = false;
                    response.message = responseData.message;
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                Log.Error(ex);
            }
            return Ok(response);
        }
        [HttpPost, Route("GetPartialWorkorderList")]
        public IHttpActionResult GetPartialWorkorderList([FromBody] DeviceData deviceData)
        {
            ResponseData response = new ResponseData();
            List<PartialWorkOrderResponse> data = null;
            try
            {
                data = pdaDAO.GetPartialWorkorderList(deviceData);
                response.status = true;
                response.message = "";
                response.data = data;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                response.data = null;
                Log.Error(ex);
            }
            return Ok(response);
        }
        [HttpPost, Route("GetPartialWorkorderItemDetails")]
        public IHttpActionResult GetPartialWorkorderItemDetails([FromBody] PartialWorkorderListRequest listRequest)
        {
            ResponseData response = new ResponseData();
            List<PickListItemDetails> data = null;
            try
            {
                data = pdaDAO.GetPartialWorkorderItemDetails(listRequest);
                response.status = true;
                response.message = "";
                response.data = data;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                response.data = null;
                Log.Error(ex);
            }
            return Ok(response);
        }
        [HttpPost, Route("InsertPartialWorkorderDetails")]
        public IHttpActionResult InsertPartialWorkorderDetails([FromBody] PartialTransactionDetails transactionDetails)
        {
            Response responseData = new Response();
            Response response = null; 
            string messageVal = string.Empty;
            try
            {
                if (transactionDetails != null)
                {
                    response = pdaDAO.InsertPartialWorkorderDetails(transactionDetails);

                    if (response.status)
                    {
                        responseData.status = true;
                        responseData.message = "Data Inserted Successfully";
                    }
                    else
                    {
                        responseData.status = false;
                        responseData.message = response.message;
                      
                    }
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.StackTrace;
                //retval = 0;
                Log.Error("TransactionDetailsController => " + ex.Message + "\n" + ex.StackTrace);
            }
            return Ok(responseData);
        }
        [HttpPost, Route("GetWorkorderListForPDA")]
        public IHttpActionResult GetWorkorderListForPDA([FromBody] PDAWorkorderList workorderList)
        {
            ResponseData responseData = new ResponseData();
            List<OrderDetails> orderDetails = null;
            try
            {
                orderDetails = pdaDAO.GetWorkorderListForPDA(workorderList);
                responseData.status = true;
                responseData.message = "";
                responseData.data = orderDetails;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }
        [HttpPost, Route("GetBinInfo")]
        public IHttpActionResult GetBinInfo([FromBody] BinDetailsRequest binDetails)
        {
            ResponseData response = new ResponseData();
            List<PartialItemDetails> data = null;
            try
            {
                data = pdaDAO.GetItemDetailsByBinName(binDetails);
                response.status = true;
                response.message = "";
                response.data = data;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                response.data = null;
                Log.Error(ex);
            }
            return Ok(response);
        }
        [HttpPost, Route("ItemMovementTransaction")]
        public IHttpActionResult ItemMovementTransaction(ActivityModelInternal activityInternal)
        {
            Response responseData = new Response();
            bool retVal = false;
            try
            {
                retVal = pdaDAO.ItemMovementTransaction(activityInternal);

                if (retVal)
                {
                    responseData.status = true;
                    responseData.message = "Data inserted successfully";
                }
                else
                {
                    responseData.status = false;
                    responseData.message = "Error to insert the data";
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                Log.Error(ex);
            }
            return Ok(responseData);
        }
        [HttpPost, Route("InsertItemQRData")]
        public IHttpActionResult InsertItemQRData(ItemsQRActivityModel itemsQRActivity)
        {
            Response responseData = new Response();
            bool retVal = false;
            try
            {
                retVal = pdaDAO.InsertItemQRData(itemsQRActivity);

                if (retVal)
                {
                    responseData.status = true;
                    responseData.message = "Data inserted successfully";
                }
                else
                {
                    responseData.status = false;
                    responseData.message = "Error to insert the data";
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                Log.Error(ex);
            }
            return Ok(responseData);
        }
        [HttpPost, Route("InsertDigitizerItemQRData")]
        public IHttpActionResult InsertDigitizerItemQRData(DigitizerModel digitizerModel)
        {
            Response responseData = new Response();
            bool retVal = false;
            try
            {
                retVal = pdaDAO.InsertDigitizerItemQRData(digitizerModel);

                if (retVal)
                {
                    responseData.status = true;
                    responseData.message = "Data inserted successfully";
                }
                else
                {
                    responseData.status = false;
                    responseData.message = "Error to insert the data";
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                Log.Error(ex);
            }
            return Ok(responseData);
        }
        [HttpPost, Route("GetAllBarcodesForTransaction")]
        public IHttpActionResult GetAllBarcodesForTransaction([FromBody] Barcode barcode)
        {
            ResponseData responseData = new ResponseData();
            List<BarcodeResponse> barcodeDetails = null;
           
            try
            {
                barcodeDetails = pdaDAO.GetAllBarcodesForTransaction(barcode);

               
                    responseData.status = true;
                    responseData.message = "";
                    responseData.data = barcodeDetails;

            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }
        [HttpPost, Route("InsertPalletBinTransaction")]
        public IHttpActionResult InsertPalletBinTransaction(PalletBinTransModel transModel)
        {
            Response responseData = new Response();
            bool retVal = false;
            try
            {
                retVal = pdaDAO.InsertPalletBinTransaction(transModel);

                if (retVal)
                {
                    responseData.status = true;
                    responseData.message = "Data inserted successfully";
                }
                else
                {
                    responseData.status = false;
                    responseData.message = "Error to insert the data";
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                Log.Error(ex);
            }
            return Ok(responseData);
        }
        [HttpPost, Route("InsertTransactionForQRManual")]
        public IHttpActionResult InsertTransactionForQRManual(ActivityModelItems activityItems)
        {
            Response response = new Response();
            ActivityResponseData responseData = null;
            //bool retVal;
            try
            {
                responseData = pdaDAO.InsertTransactionForQRManual(activityItems);
                if (responseData.status)
                {
                    if (responseData.isUnique)
                    {
                        if (responseData.isItemUnique)
                        {
                            response.status = true;
                            response.message = "The pallet has been mapped successfully";
                            Log.Info(responseData.wmsData);
                        }
                        else
                        {
                            response.status = false;
                            response.message = responseData.message;
                        }
                    }
                    else
                    {
                        response.status = false;
                        response.message = responseData.message;
                    }
                }
                else
                {
                    response.status = false;
                    response.message = responseData.message;
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                Log.Error(ex);
            }
            return Ok(response);
        }
        [HttpPost, Route("GetDCDetailsForPDA")]
        public IHttpActionResult GetDCDetailsForPDA([FromBody] DCDetails dcTagID)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            TruckResponseDetails data = null;

            try
            {
                data = pdaDAO.GetDCDetailsForPDA(dcTagID);
                if (data.Status)
                {
                    responseData.status = true;
                    responseData.message = "";
                    responseData.data = data;
                }
                else
                {
                    responseData.status = false;
                    responseData.message = "This DC Tag is not registered yet";
                }
               
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }

            return Ok(responseData);
        }
        [HttpPost, Route("GetSKUDetailsForPDA")]
        public IHttpActionResult GetSKUDetailsForPDA([FromBody] SKUDetails sKUDetails)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            List<SKUResponse> data = null;

            try
            {
                data = pdaDAO.GetSKUDetailsForPDA(sKUDetails);
                responseData.status = true;
                responseData.message = "";
                responseData.data = data;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }

            return Ok(responseData);
        }
        [HttpGet, Route("GetAllSKUsPDA")]
        public IHttpActionResult GetAllSKUsPDA()
        {
            ResponseData responseData = new ResponseData();
            List<SKUData> data = null;

            try
            {
                data = pdaDAO.GetAllSKUsPDA();
                responseData.status = true;
                responseData.message = "";
                responseData.data = data;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }

            return Ok(responseData);
        }
        [HttpPost, Route("GetSOLineItemsLoading")]
        public IHttpActionResult GetSOLineItemsLoading([FromBody] DCDetails dcTagID)
        {
            bool retVal = false;
            Response responseData = new Response();

            try
            {
                retVal = pdaDAO.GetSOLineItemsLoading(dcTagID);
                if (retVal)
                {
                    responseData.status = true;
                    responseData.message = "";
                }
                else
                {
                    responseData.status = false;
                    responseData.message = "";
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                Log.Error(ex);
            }

            return Ok(responseData);
        }
        [HttpGet, Route("GetWarehouseDetails")]
        public IHttpActionResult GetWarehouseDetails()
        {
            ResponseData responseData = new ResponseData();
            List<WarehouseDetails> data = null;

            try
            {
                data = pdaDAO.GetWarehouseDetails();
                responseData.status = true;
                responseData.message = "";
                responseData.data = data;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }

            return Ok(responseData);
        }

        #region PLANT
        [HttpPost, Route("GetDCNumbersForPlant")]
        public IHttpActionResult GetDCNumbersForPlant(DeviceData deviceID)
        {
            ResponseData responseData = new ResponseData();
           

            try
            {
                responseData.data = pdaDAO.GetDCNumbersForPlant(deviceID);
                if (responseData.data != null)
                {
                    responseData.status = true;
                    responseData.message = "";
                    responseData.data = responseData.data;
                }
                else
                {
                    responseData.status = false;
                    responseData.message = "This DC Tag is not registered yet";
                }

            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }

            return Ok(responseData);
        }
        [HttpPost, Route("PlantTransactionForScannedItems")]
        public IHttpActionResult PlantTransactionForScannedItems(ScannedItemModel activity)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            PlantActivityResponse listdata = null;
            try
            {
               listdata = pdaDAO.PlantTransactionForScannedItems(activity);

                if (listdata.status)
                {
                    if (listdata.isUnique)
                    {
                        if (listdata.isItemUnique)
                        {
                            responseData.status = true;
                            responseData.message = "The pallet has been mapped successfully";
                        }
                        else
                        {
                            responseData.status = false;
                            responseData.message = listdata.message;
                        }
                    }
                    else
                    {
                        responseData.status = false;
                        responseData.message = listdata.message;
                    }
                }
                else
                {
                    responseData.status = false;
                    responseData.message = listdata.message;
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }
        [HttpPost, Route("PlantTransactionForWithoutScannedItems")]
        public IHttpActionResult PlantTransactionForWithoutScannedItems(ManualItemModel activityItems)
        {
            Response response = new Response();
            PlantActivityResponse responseData = null;
            //bool retVal;
            try
            {
                responseData = pdaDAO.PlantTransactionForWithoutScannedItems(activityItems);
                if (responseData.status)
                {
                    if (responseData.isUnique)
                    {
                        if (responseData.isItemUnique)
                        {
                            response.status = true;
                            response.message = "The pallet has been mapped successfully";
                        }
                        else
                        {
                            response.status = false;
                            response.message = responseData.message;
                        }
                    }
                    else
                    {
                        response.status = false;
                        response.message = responseData.message;
                    }
                }
                else
                {
                    response.status = false;
                    response.message = responseData.message;
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                Log.Error(ex);
            }
            return Ok(response);
        }
        [HttpPost, Route("PlantTransactionForQRManualItems")]
        public IHttpActionResult PlantTransactionForQRManualItems(ManualItemModel activityItems)
        {
            Response response = new Response();
            PlantActivityResponse responseData = null;
            //bool retVal;
            try
            {
                responseData = pdaDAO.PlantTransactionForQRManualItems(activityItems);
                if (responseData.status)
                {
                    if (responseData.isUnique)
                    {
                        if (responseData.isItemUnique)
                        {
                            response.status = true;
                            response.message = "The pallet has been mapped successfully";
                        }
                        else
                        {
                            response.status = false;
                            response.message = responseData.message;
                        }
                    }
                    else
                    {
                        response.status = false;
                        response.message = responseData.message;
                    }
                }
                else
                {
                    response.status = false;
                    response.message = responseData.message;
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                Log.Error(ex);
            }
            return Ok(response);
        }
        #endregion

        #endregion

        //For Display the ItemsQR
        #region Report
        [HttpPost, Route("GetItemsQRActivityDetails")]
        public IHttpActionResult GetItemsQRActivityDetails([FromBody] LocationDetails locationDetails)
        {
            ResponseData responseData = new ResponseData();
            List<QRTranscationDetails> data = null;

            try
            {
                int LocationId = Convert.ToInt32(locationDetails.LocationName);
                data = pdaDAO.GetItemsQRActivityDetails(LocationId);
                responseData.status = true;
                responseData.message = "";
                responseData.data = data;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }

            return Ok(responseData);
        }


        [HttpGet, Route("GetQRLocationName")]
        public IHttpActionResult GetQRLocationName()
        {
            ResponseData responseData = new ResponseData();
            List<LocationDetails> data = null;
            try
            {
                data = pdaDAO.GetQRLocationName();
                responseData.status = true;
                responseData.message = "";
                responseData.data = data;
            }
            catch (Exception ex)
            {

                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(data);
        }
        #endregion

        [HttpPost, Route("RegisterAsset")]
        public IHttpActionResult RegisterAsset(RegisterModel registerModel)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            bool register;
            try
            {
                register = pdaDAO.registerAsset(registerModel);
                responseData.status = true;
				if (register)
				{
                responseData.data = "Registered Succesfully";

				}
				else
				{
                    responseData.data = "Please Try Again later";
                }
            }
            catch (Exception ex) 
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        [HttpPost, Route("RegisterAssetMobile")]
        public IHttpActionResult RegisterAssetMobile(RegisterModelMobile registerModel)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            bool register;
            try
            {
                register = pdaDAO.RegisterAssetMobile(registerModel);
                responseData.status = true;
                if (register)
                {
                    responseData.data = "Registered Succesfully";

                }
                else
                {
                    responseData.data = "Please Try Again later";
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }


        //For Reader

        #region Reader

        [HttpPost, Route("InsertTransactionDetails")]
        public IHttpActionResult TransactionDetails([FromBody] TransactionDetails transactionDetails)
        {
            ResponseData responseData = new ResponseData();
            bool retval = false;
            string messageVal = string.Empty;
            try
            {
                if (transactionDetails != null)
                {
                    retval = pdaDAO.InsertTransactionDetails(transactionDetails);

                    if (retval)
                    {
                        responseData.status = true;
                        responseData.message = "Data Inserted";
                    }
                    else
                    {
                        responseData.status = false;
                        responseData.message = "Data Insertion failed.";
                        responseData.message = messageVal;
                    }
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.StackTrace;
                retval = false;
                Log.Error("TransactionDetailsController => " + ex.Message + "\n" + ex.StackTrace);
            }
            return Ok(responseData);
        }
        //[HttpPost, Route("InsertTransactionDetails")]
        //public IHttpActionResult TransactionDetails([FromBody] TransactionDetails transactionDetails)
        //{
        //    ResponseData responseData = new ResponseData();
        //    Response response = null;
        //    try
        //    {
        //        if (transactionDetails != null)
        //        {
        //            response = pdaDAO.InsertTransactionDetails(transactionDetails);

        //            if (response.status)
        //            {
        //                responseData.status = true;
        //                responseData.message = response.message;
        //            }
        //            else
        //            {
        //                responseData.status = false;
        //                responseData.message = response.message;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        responseData.status = false;
        //        responseData.message = ex.StackTrace;
        //        //retval = 0;
        //        Log.Error("TransactionDetailsController => " + ex.Message + "\n" + ex.StackTrace);
        //    }
        //    return Ok(responseData);
        //}
        [HttpPost, Route("InsertTagLoggerDetails")]
        public IHttpActionResult TransactionDetails1([FromBody] TagDetailsRequest transactionDetails)
        {
            ResponseData responseData = new ResponseData();
            bool retval = false;
            string messageVal = string.Empty;
            try
            {
                if (transactionDetails != null)
                {
                    retval = pdaDAO.InsertTagLoggerDetails(transactionDetails);

                    if (retval)
                    {
                        responseData.status = true;
                        responseData.message = "Data Inserted";
                    }
                    else
                    {
                        responseData.status = false;
                        responseData.message = "Data Insertion failed.";
                        responseData.message = messageVal;
                    }
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.StackTrace;
                //retval = 0;
                Log.Error("TransactionDetailsController => " + ex.Message + "\n" + ex.StackTrace);
            }
            return Ok(responseData);
        }

        //[HttpPost, Route("GetReaderWorkorderList")]
        //public IHttpActionResult GetReaderWorkorderList([FromBody] ReaderWorkorderListRequest readerWorkorderList)
        //{
        //    ReaderWorkorderListResponse readerWorkorder = new ReaderWorkorderListResponse();
        //    //ReaderTransactionState data = null;
        //    List<OrderDetails> orderDetails = null;
        //    try
        //    {
        //        readerWorkorder = pdaDAO.GetReaderWorkorderList(readerWorkorderList);
        //        readerWorkorder.status = true;
        //        readerWorkorder.message = "";
        //        readerWorkorder.configuration = new Configuration
        //        {
        //            RSSI = int.Parse(System.Configuration.ConfigurationManager.AppSettings["RSSI"]),
        //            Power = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Power"]),
        //            PollingTimer = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PollingTimer"])
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        readerWorkorder.status = false;
        //        readerWorkorder.message = ex.Message;
        //        readerWorkorder.data = null;
        //        Log.Error(ex);
        //    }
        //    return Ok(readerWorkorder);
        //}

        //[HttpPost, Route("UploadCurrentPallet")]
        //public IHttpActionResult UploadCurrentPallet(CurrentPalletRequest currentPallet)
        //{
        //    Response response = new Response();
        //    bool isValid;
        //    try
        //    {
        //        isValid = pdaDAO.UploadCurrentPallet(currentPallet);
        //        if (isValid)
        //        {
        //            response.status = true;
        //            response.message = "";
        //        }
        //        else
        //        {
        //            response.status = false;
        //            response.message = "Try again";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.status = false;
        //        response.message = ex.Message;
        //    }
        //    return Ok(response);
        //}
        [HttpPost, Route("GetReaderWorkorderList")]
        public IHttpActionResult GetReaderWorkorderList([FromBody] ReaderWorkorderListRequest readerWorkorderList)
        {
            ReaderWorkorderListResponse readerWorkorder = new ReaderWorkorderListResponse();
            //ReaderTransactionState data = null;
            List<OrderDetails> orderDetails = null;
            try
            {
                readerWorkorder = pdaDAO.GetReaderWorkorderList(readerWorkorderList);
                readerWorkorder.status = true;
                readerWorkorder.message = "";
                readerWorkorder.configuration = new Configuration
                {
                    RSSI = int.Parse(System.Configuration.ConfigurationManager.AppSettings["RSSI"]),
                    Power = int.Parse(System.Configuration.ConfigurationManager.AppSettings["Power"]),
                    PollingTimer = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PollingTimer"])
                };
            }
            catch (Exception ex)
            {
                readerWorkorder.status = false;
                readerWorkorder.message = ex.Message;
                readerWorkorder.data = null;
                Log.Error(ex);
            }
            return Ok(readerWorkorder);
        }
        [HttpPost, Route("UploadCurrentPallet")]
        public IHttpActionResult UploadCurrentPallet(CurrentPalletRequest currentPallet)
        {
            Response response = new Response();
            bool isValid;
            try
            {
                isValid = pdaDAO.UploadCurrentPalletV1(currentPallet);
                if (isValid)
                {
                    response.status = true;
                    response.message = "";
                }
                else
                {
                    response.status = false;
                    response.message = "Try again";
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }
        [HttpPost, Route("UploadCurrentBin")]
        public IHttpActionResult UploadCurrentBin(CurrentBinRequest currentBin)
        {
            Response response = new Response();
            bool isValid;
            try
            {
                isValid = pdaDAO.UploadCurrentBin(currentBin);
                if (isValid)
                {
                    response.status = true;
                    response.message = "";
                }
                else
                {
                    response.status = false;
                    response.message = "Try again";
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        #endregion


        //For MobileDisplay

        #region Mobile Display
        [HttpPost, Route("TabLogin")]
        public IHttpActionResult TabLogin(LoginRequest request)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            bool isValid;
            try
            {
                isValid = pdaDAO.CheckCredentials(request);

                if (isValid)
                {
                    responseData.status = true;
                    responseData.message = "Login Successfully";

                }
                else
                {
                    responseData.status = false;
                    responseData.message = "Please Try Again";
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        [HttpGet, Route("GetReaderStatus")]
        public IHttpActionResult GetReaderStatus([FromBody] DeviceData deviceData)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            ReaderStatusinfo data = null;
            try
            {
                data = pdaDAO.GetReaderStatus(deviceData);
                responseData.status = true;
                responseData.data = data;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        [HttpPost, Route("GetWorkorderList")]
        public IHttpActionResult GetWorkorderList([FromBody] DeviceData deviceData)
        {
            WorkOrderResponse response = new WorkOrderResponse();
            List<Workorderlist> data = null;
            try
            {
                ReaderStatusinfo readerStatusinfo = pdaDAO.GetReaderStatus(deviceData);
                data = pdaDAO.GetWorkorderlists(deviceData);
                response.status = true;
                response.message = "";
                response.ReaderStatus = readerStatusinfo.ReaderStatus;
                response.ApplicationStatus = readerStatusinfo.ApplicationStatus;
                response.data = data;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                response.data = null;
                Log.Error(ex);
            }
            return Ok(response);
        }

        [HttpPost, Route("UpdateWorkorderStatus")]
        public IHttpActionResult UpdateWorkorderStatus([FromBody] WorkorderStatusRequest workorderStatus)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            WorkorderStatusResponse result = null;
            try
            {
                result = pdaDAO.UpdateWorkorderStatus(workorderStatus);
                responseData.status = true;
                responseData.message = "";
                responseData.data = result;
            }
            catch (Exception ex)
            {

                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        //[HttpPost, Route("GetWorkorderListItems")]
        //public IHttpActionResult GetWorkorderListItems([FromBody] WorkorderListRequest workorderItems)
        //{
        //    ResponseObjectData responseData = new ResponseObjectData();
        //    WorkorderListResponse result = null;
        //    try
        //    {
        //        result = pdaDAO.GetWorkorderListItems(workorderItems);
        //        responseData.status = true;
        //        responseData.message = "";
        //        responseData.data = result;
        //    }
        //    catch (Exception ex)
        //    {

        //        responseData.status = false;
        //        responseData.message = ex.Message;
        //        responseData.data = null;
        //        Log.Error(ex);
        //    }
        //    return Ok(responseData);
        //}
        [HttpPost, Route("GetWorkorderListItemsV1")]
        public IHttpActionResult GetWorkorderListItemsV1([FromBody] WorkorderListRequest workorderItems)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            WorkorderListResponse result = null;
            try
            {
                result = pdaDAO.GetWorkorderListItemsV1(workorderItems);
                responseData.status = true;
                responseData.message = "";
                responseData.data = result;
            }
            catch (Exception ex)
            {

                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(responseData);
        }
        [HttpPost, Route("StartReader")]
        public IHttpActionResult StartReader(ReaderStartRequset startRequset)
        {
            Response response = new Response();
            bool isValid;
            try
            {
                isValid = pdaDAO.StartReader(startRequset);
                if (isValid)
                {
                    response.status = true;
                    response.message = "";
                }
                else
                {
                    response.status = false;
                    response.message = "Try again";
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
            }
            return Ok(response);
        }

        #endregion

        //For WorkorderCreation
        #region WebApp

        [HttpPost, Route("WebLogin")]
        public IHttpActionResult WebLogin(UserModel user)
        {
            Response response = new Response();
            bool isValid;
            try
            {
                isValid = pdaDAO.WebLogin(user);

                if (isValid)
                {
                    response.status = true;
                    response.message = "Login Successfully";

                }
                else
                {
                    response.status = false;
                    response.message = "Please Try Again";
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                Log.Error(ex);
            }
            return Ok(response);
        }

        [HttpGet, Route("GetTruckDetails")]
        public IHttpActionResult GetTruckDetails()
        {
            ResponseData responseData = new ResponseData();
            List<TruckDetails> data = null;
            try
            {
                data = pdaDAO.GetTruckDetails();
                responseData.status = true;
                responseData.message = "";
                responseData.data = data;
            }
            catch (Exception ex)
            {

                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(data);
        }

        [HttpPost, Route("GetPalletWithItemDetails")]
        public IHttpActionResult GetPalletWithItemDetails([FromBody] TruckDetails truckDetails)
        {
            ResponseData responseData = new ResponseData();
            List<PalletItemDetails> data = null;

            try
            {
                data = pdaDAO.GetPalletWithItemDetails(truckDetails);
                responseData.status = true;
                responseData.message = "";
                responseData.data = data;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }

            return Ok(responseData);
        }

        //[HttpPost, Route("CreateWorkOrder")]
        //public IHttpActionResult CreateWorkOrder(WorkorderCreationDetails workorderCreation)
        //{
        //    ResponseData responseData = new ResponseData();

        //    bool retval = false;
        //    string messageVal = string.Empty;
        //    try
        //    {
        //        if (workorderCreation != null)
        //        {
        //            retval = pdaDAO.InsertWorkorderDetails(workorderCreation);

        //            if (retval)
        //            {
        //                responseData.status = true;
        //                responseData.message = "Data Inserted";
        //            }
        //            else
        //            {
        //                responseData.status = false;
        //                responseData.message = "Data Insertion failed.";
        //                responseData.message = messageVal;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        responseData.status = false;
        //        responseData.message = ex.StackTrace;
        //        //retval = 0;
        //        Log.Error("WorkorderDetailsController => " + ex.Message + "\n" + ex.StackTrace);
        //    }
        //    return Ok(responseData);
        //}
        
        #endregion

        //For TV
        #region TV
        [HttpGet, Route("GetLocationName")]
        public IHttpActionResult GetLocationName()
        {
            ResponseData responseData = new ResponseData();
            List<LocationDetails> data = null;
            try
            {
                data = pdaDAO.GetLocationName();
                responseData.status = true;
                responseData.message = "";
                responseData.data = data;
            }
            catch (Exception ex)
            {

                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(data);
        }
        [HttpPost, Route("GetPalletItemDetailsByLocationName")]
        public IHttpActionResult GetPalletItemDetailsByLocationName([FromBody] LocationDetails locationDetails)
        {
            ResponseData responseData = new ResponseData();
            List<TruckResponseDetailsByLocation> data = null;

            try
            {
                data = pdaDAO.GetPalletItemDetailsByLocationName(locationDetails);
                responseData.status = true;
                responseData.message = "";
                responseData.data = data;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }

            return Ok(responseData);
        }
        #endregion

        #region Workorder Creation
        [HttpPost, Route("CreateWorkOrderU0")]
        public IHttpActionResult CreateWorkOrderU0(WorkorderCreationDetails workorderCreation)
        {
            ResponseData responseData = new ResponseData();

            bool retval = false;
            string messageVal = string.Empty;
            try
            {
                if (workorderCreation != null)
                {
                    retval = pdaDAO.InsertWorkorderDetailsU0(workorderCreation);

                    if (retval)
                    {
                        responseData.status = true;
                        responseData.message = "Data Inserted";
                    }
                    else
                    {
                        responseData.status = false;
                        responseData.message = "Data Insertion failed.";
                        responseData.message = messageVal;
                    }
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.StackTrace;
                //retval = 0;
                Log.Error("WorkorderDetailsController => " + ex.Message + "\n" + ex.StackTrace);
            }
            return Ok(responseData);
        }

        [HttpPost, Route("receivingWorkOrder")]
        public IHttpActionResult receivingWorkOrder(WorkorderCreationDetailsU1 workorderCreation)
        {
            Response responseData = new Response();

            bool retval = false;
            string messageVal = string.Empty;
            try
            {
                if (workorderCreation != null)
                {
                    retval = pdaDAO.receivingWorkOrder(workorderCreation);

                    if (retval)
                    {
                        responseData.status = true;
                        responseData.message = "Workorder Created Successfully";
                    }
                    else
                    {
                        responseData.status = false;
                        responseData.message = "Failed to create workorder";
                        responseData.message = messageVal;
                    }
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.StackTrace;
                //retval = 0;
                Log.Error("WorkorderDetailsController => " + ex.Message + "\n" + ex.StackTrace);
            }
            return Ok(responseData);
        }
        [HttpPost, Route("dispatchWorkOrder")]
        public IHttpActionResult dispatchWorkOrder(WorkorderCreationDetailsL0 workorderCreationL0)
        {
            Response responseData = new Response();

            bool retval = false;
            string messageVal = string.Empty;
            try
            {
                if (workorderCreationL0 != null)
                {
                    retval = pdaDAO.dispatchWorkOrder(workorderCreationL0);

                    if (retval)
                    {
                        responseData.status = true;
                        responseData.message = "Workorder Created Successfully";
                    }
                    else
                    {
                        responseData.status = false;
                        responseData.message = "Failed to create workorder";
                        responseData.message = messageVal;
                    }
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.StackTrace;
                //retval = 0;
                Log.Error("WorkorderDetailsController => " + ex.Message + "\n" + ex.StackTrace);
            }
            return Ok(responseData);
        }
        [HttpPost, Route("CreateWorkOrderL1")]
        public IHttpActionResult CreateWorkOrderL1(WorkorderCreationDetails workorderCreation)
        {
            ResponseData responseData = new ResponseData();

            bool retval = false;
            string messageVal = string.Empty;
            try
            {
                if (workorderCreation != null)
                {
                    retval = pdaDAO.InsertWorkorderDetailsL1(workorderCreation);

                    if (retval)
                    {
                        responseData.status = true;
                        responseData.message = "Data Inserted";
                    }
                    else
                    {
                        responseData.status = false;
                        responseData.message = "Data Insertion failed.";
                        responseData.message = messageVal;
                    }
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.StackTrace;
                //retval = 0;
                Log.Error("WorkorderDetailsController => " + ex.Message + "\n" + ex.StackTrace);
            }
            return Ok(responseData);
        }
        [HttpPost, Route("CloseWorkorderU0")]
        public IHttpActionResult CloseWorkorderU0(CloseWorkorderRequest request)
        {
            Response responseData = new Response();
            bool isClosed;
            try
            {
                isClosed = pdaDAO.CloseWorkorderU0(request);

                if (isClosed)
                {
                    responseData.status = true;
                    responseData.message = "Workorder Closed";

                }
                else
                {
                    responseData.status = false;
                    responseData.message = "";
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                Log.Error(ex);
            }
            return Ok(responseData);
        }
        [HttpPost, Route("internalWorkOrder")]
        public IHttpActionResult internalWorkOrder(WorkorderCreationDetailsI0 workorderCreation)
        {
            Response responseData = new Response();

            bool retval = false;
            string messageVal = string.Empty;
            try
            {
                if (workorderCreation != null)
                {
                    retval = pdaDAO.internalWorkOrder(workorderCreation);

                    if (retval)
                    {
                        responseData.status = true;
                        responseData.message = "Workorder Created Successfully";
                    }
                    else
                    {
                        responseData.status = false;
                        responseData.message = "Failed to create workorder";
                        responseData.message = messageVal;
                    }
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.StackTrace;
                //retval = 0;
                Log.Error("WorkorderDetailsController => " + ex.Message + "\n" + ex.StackTrace);
            }
            return Ok(responseData);
        }
        #endregion

        #region BotService
        [HttpGet, Route("GetPendingWorkorderDetails")]
        public IHttpActionResult GetPendingWorkorderDetails()
        {
            ResponseData responseData = new ResponseData();
            List<PendingWorkOrderDetails> data = null;
            try
            {
                data = pdaDAO.GetPendingWorkorderDetails();
                responseData.status = true;
                responseData.message = "";
                responseData.data = data;
            }
            catch (Exception ex)
            {

                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = null;
                Log.Error(ex);
            }
            return Ok(data);
        }
        [HttpPost, Route("InsertTransactionDetailsForBotService")]
        public IHttpActionResult InsertTransactionDetailsForBotService([FromBody] TransactionDetailsV1 transactionDetails)
        {
            ResponseData responseData = new ResponseData();
            bool retval = false;
            string messageVal = string.Empty;
            try
            {
                if (transactionDetails != null)
                {
                    retval = pdaDAO.InsertTransactionDetailsForBotService(transactionDetails);

                    if (retval)
                    {
                        responseData.status = true;
                        responseData.message = "Data Inserted";
                    }
                    else
                    {
                        responseData.status = false;
                        responseData.message = "Data Insertion failed.";
                        responseData.message = messageVal;
                    }
                }
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.StackTrace;
                //retval = 0;
                Log.Error("TransactionDetailsController => " + ex.Message + "\n" + ex.StackTrace);
            }
            return Ok(responseData);
        }
        #endregion
        //[HttpPost, Route("WorkorderStateUpdate")]
        //public IHttpActionResult WorkorderStateUpdate([FromBody] WorkorderStatusRequest workorderState)
        //{
        //    ResponseData responseData = new ResponseData();
        //    bool isValid;
        //    try
        //    {

        //        isValid = pdaDAO.WorkorderStateUpdate(workorderState);
        //        responseData.status=true;
        //        responseData.message = "Updated Successfully";
        //    }
        //    catch (Exception ex)
        //    {
        //        responseData.status = false;
        //        responseData.message = ex.Message;
        //        responseData.data = null;
        //        Log.Error(ex);
        //    }
        //    return Ok(responseData);
        //}

        //[HttpGet, Route("GetPalletName")]
        //public IHttpActionResult GetPalletName([FromBody] UserData clientData)
        //{
        //    //ResponseData responseData = new ResponseData();
        //    List<PalletsDetails> data = null;
        //    try
        //    {
        //        data = pdaDAO.GetPalletName(clientData);
        //        //responseData.status = true;
        //        //responseData.data = data;
        //    }
        //    catch (Exception ex)
        //    {

        //        //responseData.status = false;
        //        //responseData.message = ex.Message;
        //        //responseData.data = null;
        //        Log.Error(ex);
        //    }
        //    return Ok(data);
        //}
        //For MobileDisplay
        /*Warehouse code*/

    }
}
