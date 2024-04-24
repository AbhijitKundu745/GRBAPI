using LaundryManagementSystem.DatabaseAccessLayer;
using LaundryManagementSystem.Models;
using PSL.Laundry.CentralService.Logger;
using PSL.Laundry.CentralService.Models;
using PSL.Warehouse.CentralService.Controllers;
using PSL.Warehouse.CentralService.IDataAccessLayer;
using PSL.Warehouse.CentralService.Logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WarehouseManagementSystem.DatabaseAccessLayer;

namespace PSL.Warehouse.CentralService.Controllers
{
    [RoutePrefix("Web")]
    public class WebController : ApiController
    {

        #region properties
        private readonly ILogger<PDAController> Log;
        AssetMappingDAL assetmappingDB = new AssetMappingDAL();
        DBAccessLayer dBAccessLayer = new DBAccessLayer();
        LoginDAL loginDAL = new LoginDAL();
        ProcessesDAL processesDAL = new ProcessesDAL();
        private readonly IPDADAL pdaDAO;
        #endregion

        public WebController(ILogger<PDAController> logger, IPDADAL pdaDAO)
        {
            this.Log = logger;
            this.pdaDAO = pdaDAO;
            //this.assetMappingDAO = AssetMappingDAL;
        }

        [HttpGet, Route("GetAllUnMappedAssets")]
        public IHttpActionResult GetAllAssetsForMapping(string customerID)
        {
            ResponseData responseData = new ResponseData();
            List<AssetMasterModel> assets = null;
            try
            {
                assets = assetmappingDB.GetAllAssetsForMapping(customerID);
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

        [HttpGet, Route("GetRegisteredAssetsByCustomer")]
        public IHttpActionResult GetRegisteredAssets(string customerID)
        {
            ResponseData responseData = new ResponseData();
            List<AssetMasterModel> assets = null;
            try
            {
                assets = assetmappingDB.GetRegisteredAssets(customerID);
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


        [HttpGet, Route("GetAssetTypesByCustomer")]
        public IHttpActionResult GetAssetTypesByCustomer(string customerID)
        {
            ResponseData responseData = new ResponseData();
            List<AssetModel> assets = null;
            try
            {
                assets = dBAccessLayer.GetAssetTypesByCustomer(customerID);
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

        [HttpGet, Route("GetAllAssetsByCustomer")]
        public IHttpActionResult GetAllAssetsList(string customerID)
        {
            ResponseData responseData = new ResponseData();
            List<AssetMasterModel> assets = null;
            try
            {
                assets = dBAccessLayer.GetAllAssets(customerID);
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


        [HttpGet, Route("GetSIDForCustomerBySerialNo")]
        public IHttpActionResult GetSerialNo(string customerID, long serialNo)
        {
            ResponseData responseData = new ResponseData();
            List<SerialNo> serialNos = null;
            try
            {
                serialNos = assetmappingDB.GetSerialNo(customerID, serialNo);
                responseData.status = true;
                responseData.data = serialNos;
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

        [HttpGet, Route("GetVendorsByCustomer")]
        public IHttpActionResult GetVendors(string customerID)
        {
            ResponseData responseData = new ResponseData();
            List<Vendor> vendor = null;
            try
            {
                vendor = processesDAL.GetVendors(customerID);
                responseData.status = true;
                responseData.data = vendor;
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
        [HttpGet, Route("GetAssetLifeByAssetID")]
        public IHttpActionResult GetAssetLife(string assetID)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            int data = 0;
            try
            {
                data = processesDAL.GetAssetLife(assetID);
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

        [HttpGet, Route("GetTagDetailsByCustomer")]
        public IHttpActionResult GetTagDetails(string customerID)
        {
            ResponseData responseData = new ResponseData();
            List<AssetMasterModel> assetMasterModel = null;
            try
            {
                assetMasterModel = processesDAL.GetTagDetails(customerID);
                responseData.status = true;
                responseData.data = assetMasterModel;
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

        [HttpPost, Route("MapAsset")]
        public IHttpActionResult MapAsset(AssetMasterModel assetMasterModel)
        {
            ResponseData responseData = new ResponseData();
            Response response;
            try
            {
                response = assetmappingDB.MapAssets(assetMasterModel);
                responseData.status = true;
                responseData.data = null;
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

        [HttpPost, Route("CreateAsset")]
        public IHttpActionResult CreateAsset(AssetMasterModel assetMasterModel)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            try
            {
                var data = dBAccessLayer.CreateAsset(assetMasterModel);
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

        [HttpPost, Route("InsertActivity")]
        public IHttpActionResult InsertActivity(ActivityModelWeb activityModelWeb)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            try
            {
                var data = processesDAL.InsertActivity(activityModelWeb);
                responseData.status = true;
                responseData.data = data;
            }
            catch (Exception ex)
            {
                responseData.status = false;
                responseData.message = ex.Message;
                responseData.data = 0;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        [HttpPost, Route("InsertActivityDetails")]
        public IHttpActionResult InsertActivityDetails(ActivityModelWeb activityModelWeb, string AssetID, string LastInvDatetime, int AssetLife)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            try
            {
                processesDAL.InsertActivityDetails(activityModelWeb, AssetID, LastInvDatetime, AssetLife);
                responseData.status = true;
                responseData.data = "";
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


        [HttpPost, Route("InsertErrorActivity")]
        public IHttpActionResult InsertErrorActivity(ActivityModelWeb activityModelWeb, string tagID)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            // Response response;
            try
            {
                processesDAL.InsertErrorActivity(activityModelWeb, tagID);
                responseData.status = true;
                responseData.data = 1;
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

        [HttpPost, Route("UpdateAsset")]
        public IHttpActionResult UpdateAsset(AssetMasterModel assetMasterModel)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            // Response response;
            try
            {
                dBAccessLayer.UpdateAssets(assetMasterModel);
                responseData.status = true;
                responseData.data = null;
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

        //[HttpPost, Route("Login")]
        //public IHttpActionResult MobileLogin(LoginModel login)
        //{
        //    ResponseObjectData responseData = new ResponseObjectData();
        //    LoginResponse userdetails;
        //    try
        //    {
        //        userdetails = pdaDAO.MobileLogin(login);
        //        responseData.status = true;
        //        responseData.data = userdetails;
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

        [HttpPost, Route("Login")]
        public IHttpActionResult Login(LoginModel login)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            List<LoginModelDesktop> userdetails;
            try
            {
                userdetails = loginDAL.GetUserDetails(login.UserName, login.Password,login.ClientDeviceID);
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

        [HttpPost, Route("InsertSerialNo")]
        public IHttpActionResult InsertSerialNo(SerialNumberMaster serialNumberMaster)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            // Response response;
            try
            {
                var data = dBAccessLayer.InsertSerialNo(serialNumberMaster.serialno, serialNumberMaster.tagID, serialNumberMaster.custiD, serialNumberMaster.transaction, serialNumberMaster.chipID);
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

        //[HttpPost, Route("Transaction")]
        //public IHttpActionResult Transaction(ActivityModel activity)
        //{
        //    ResponseObjectData responseData = new ResponseObjectData();
        //    bool result;
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        result = pdaDAO.CreateActivityMobile(activity);
        //        responseData.status = true;
        //        responseData.data = 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        responseData.status = false;
        //        responseData.message = ex.Message;
        //        responseData.data = 0;
        //        Log.Error(ex);
        //    }
        //    return Ok(responseData);
        //}


        [HttpPost, Route("UpdateChipID")]
        public IHttpActionResult UpdateChipID(ChipMaster chipMaster)
        {
            ResponseObjectData responseData = new ResponseObjectData();
            // Response response;
            try
            {
                var data = dBAccessLayer.UpdateChipID(chipMaster.tagID, chipMaster.custiD, chipMaster.chipID);
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


        [HttpDelete, Route("DeleteAsset")]
        public IHttpActionResult DeleteAsset(string assetID)
        {
            ResponseData responseData = new ResponseData();
            bool res = false;
            try
            {
                res = dBAccessLayer.DeleteRegisteredAsset(assetID);
                responseData.status = true;
            }
            catch (Exception ex)
            {
                responseData.status = true;
                //  responseData.message = ex.Message;
                Log.Error(ex);
            }
            return Ok(responseData);
        }

        // GET: api/Web
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Web/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Web
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Web/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Web/5
        public void Delete(int id)
        {
        }
    }
}
