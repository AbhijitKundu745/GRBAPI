using Dapper;
using LaundryManagementSystem.Models;
using PSL.Warehouse.CentralService.DBContext;
//using PSL.Laundry.CentralService.Global;
using PSL.Laundry.CentralService.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using PSL.Warehouse.CentralService.Global;

namespace WarehouseManagementSystem.DatabaseAccessLayer
{
    public class AssetMappingDAL
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);

        //Getting assets for Mappin in combobox
        public List<AssetMasterModel> GetAllAssetsForMapping(string CustomerID)
        {
            List<AssetMasterModel> resultAssets = new List<AssetMasterModel>();

            try
            {
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
                    con.Open();

                SqlCommand cmd = new SqlCommand(AppSettings.StoredProcedure.GetAssetsForMapping, con); //correct the SP
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.ExecuteNonQuery();
                con.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        resultAssets.Add(new AssetMasterModel
                        {
                            AssetID = dr["AssetID"].ToString(),
                            AName = dr["AName"].ToString(),
                            ADescription = dr["ADescription"].ToString(),
                            ATypeID = Convert.ToInt32(dr["ATypeID"]),
                            //ATagID = dr["ATagID"].ToString(),
                            //SID=Convert.ToInt64( dr["SID"]),
                            //ASerialNo=Convert.ToInt32( dr["ASerialNo"]),
                            AssetName = dr["AssetName"].ToString(),
                            //IsActive = Convert.ToByte(dr[5]),
                            //UserID = dr["UID"].ToString(),
                            //AStateDateTime=Convert.ToDateTime( dr["AStateDateTime"]),
                            //AState = dr["AState"].ToString(),
                            //LastInventoryDateTime=Convert.ToDateTime( dr["LastInventoryDateTime"]),
                            //CustomerID = dr["CustomerID"].ToString(),
                            //TransactionDateTime = Convert.ToDateTime( dr["TransactionDateTime"]),


                        });

                    }
                }
                else
                {
                    //MessageBox("No Assets available for mapping.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                return resultAssets;

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<SerialNo> GetSerialNo(string CustomerID, long serialNo)
        {
            List<SerialNo> resultSerial = new List<SerialNo>();
            try
            {
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
                    con.Open();
                SqlCommand cmd = new SqlCommand(AppSettings.StoredProcedure.GetSID, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                cmd.Parameters.AddWithValue("@SerialNumber", serialNo);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.ExecuteNonQuery();
                con.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        resultSerial.Add(new SerialNo
                        {
                            SID = Convert.ToInt64(dr["SID"]),
                            SerialNumber = Convert.ToInt32(dr["SerialNumber"]),
                        });
                    }
                }
                else
                {
                    //MessageBox("No data present.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return resultSerial;

            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public Response MapAssets(AssetMasterModel assetMaster)
        {
            Response result = new Response();

            if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
                con.Open();
            var _params = new DynamicParameters();
            _params.Add("@AssetTypeID", assetMaster.ATypeID);
            _params.Add("@TagID", assetMaster.ATagID);
            _params.Add("@SID", assetMaster.SID);
            _params.Add("@ATagID", assetMaster.ATagID);
            _params.Add("@ASerialNo", assetMaster.ASerialNo);
            _params.Add("@UID", assetMaster.UserID);
            _params.Add("@TransactionDateTime", assetMaster.TransactionDateTime);
            _params.Add("@AssetID", assetMaster.AssetID);
            _params.Add("@IsRegistered", assetMaster.IsRegistered);

            con.Execute(AppSettings.StoredProcedure.MapAssets, _params, commandType: CommandType.StoredProcedure);
            con.Close();
            result.status = true;
            return result;
        }

        //Get Registered Assets
        public List<AssetMasterModel> GetRegisteredAssets(string CustomerID)
        {
            List<AssetMasterModel> resultRegAssets = new List<AssetMasterModel>();
            try
            {
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
                    con.Open();

                SqlCommand cmd = new SqlCommand(AppSettings.StoredProcedure.GetRegisteredAssets, con); //Write the Sp
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.ExecuteNonQuery();
                con.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        resultRegAssets.Add(new AssetMasterModel
                        {
                            AssetID = dr["AssetID"].ToString(),
                            AName = dr["AName"].ToString(),
                            ADescription = dr["ADescription"].ToString(),
                            ATypeID = Convert.ToInt32(dr["ATypeID"]),
                            ATagID = dr["ATagID"].ToString(),
                            SID = Convert.ToInt64(dr["SID"]),
                            ASerialNo = Convert.ToInt32(dr["ASerialNo"]),
                            AssetName = dr["AssetName"].ToString(),
                            IsActive = Convert.ToByte(dr["IsActive"]),
                            //UserID = dr["UID"].ToString(),
                            //AStateDateTime=Convert.ToDateTime( dr["AStateDateTime"]),
                            //AState = dr["AState"].ToString(),
                            //LastInventoryDateTime=Convert.ToDateTime( dr["LastInventoryDateTime"]),
                            //CustomerID = dr["CustomerID"].ToString(),
                            //TransactionDateTime = Convert.ToDateTime( dr["TransactionDateTime"]),
                            IsRegistered = Convert.ToByte(dr["IsRegistered"]),


                        });

                    }
                }
                return resultRegAssets;

            }
            catch (Exception ex)
            {
                throw;
            }


        }
    }

}

