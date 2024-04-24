using Dapper;
using LaundryManagementSystem.Models;
//using PSL.Laundry.CentralService.Global;
using PSL.Laundry.CentralService.Models;
using PSL.Warehouse.CentralService.Global;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace LaundryManagementSystem.DatabaseAccessLayer
{
    //AssetRegUC , asset registration user control
    public class DBAccessLayer
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);

        //Get AssetType By customer to load Combobox 
        //public DataSet GetAssetByCustomer(string CustomerID)
        //{
        //    if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
        //        con.Open();
        //    SqlCommand cmd = new SqlCommand(AppSettings.StoredProcedure.GetAssetTypeByCustomer, con);

        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);
        //    cmd.ExecuteNonQuery();

        //    con.Close();
        //    return ds;
        //}

        public List<AssetModel> GetAssetTypesByCustomer(string CustomerID)
        {
            List<AssetModel> AssetList = new List<AssetModel>();
            if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
                con.Open();
            SqlCommand cmd = new SqlCommand(AppSettings.StoredProcedure.GetAssetTypeByCustomer, con);

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
                    AssetList.Add(new AssetModel
                    {
                        AssetName = dr["AssetName"].ToString(),
                        ATypeID = Convert.ToInt32(dr["ATypeID"])
                    });
                }
            }

            return AssetList;
        }

        // Insert Data Into AssetTable
        public int CreateAsset(AssetMasterModel assetMaster)
        {
            try
            {



                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
                    con.Open();

                SqlCommand cmd = new SqlCommand(AppSettings.StoredProcedure.InsertAssetRegistration, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", assetMaster.CustomerID);
                cmd.Parameters.AddWithValue("@UID", assetMaster.UserID);
                cmd.Parameters.AddWithValue("@AName", assetMaster.AName);
                cmd.Parameters.AddWithValue("@ADescription", assetMaster.ADescription);
                cmd.Parameters.AddWithValue("@ATypeID", assetMaster.ATypeID);
                cmd.Parameters.AddWithValue("@IsActive", assetMaster.IsActive);
                cmd.Parameters.AddWithValue("@TransactionDateTime", assetMaster.TransactionDateTime);
                cmd.Parameters.AddWithValue("@IsRegistered", assetMaster.IsRegistered);
                cmd.Parameters.AddWithValue("@AssetLife", assetMaster.AssetLife);
                if (cmd.ExecuteNonQuery() > 0)
                {

                    con.Close();
                    return 1;
                }
                else
                {
                    con.Close();
                    return 0;
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw ex;
            }

        }
        public List<AssetMasterModel> GetAllAssets(string CustomerID)
        {
            List<AssetMasterModel> result = new List<AssetMasterModel>();
            try
            {
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
                    con.Open();

                SqlCommand cmd = new SqlCommand(AppSettings.StoredProcedure.GetAllAsset, con);
                //cmd.Connection.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TenantID", CustomerID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.ExecuteNonQuery();
                //cmd.Connection.Close();
                con.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(new AssetMasterModel
                        {
                            AssetID = dr["AssetID"].ToString(),
                            AName = dr["AName"].ToString(),
                            ADescription = dr["ADescription"].ToString(),
                            ATypeID = Convert.ToInt32(dr["ATypeID"]),
                            //ATagID = dr["ATagID"].ToString(),
                            //SID=Convert.ToInt64( dr["SID"]),
                            //ASerialNo=Convert.ToInt32( dr["ASerialNo"]),
                            IsActive = Convert.ToByte(dr["IsActive"]),
                            UserID = dr["UID"].ToString(),
                            //AStateDateTime=Convert.ToDateTime( dr["AStateDateTime"]),
                            //AState = dr["AState"].ToString(),
                            //LastInventoryDateTime=Convert.ToDateTime( dr["LastInventoryDateTime"]),
                            CustomerID = dr["CustomerID"].ToString(),
                            //TransactionDateTime = Convert.ToDateTime( dr["TransactionDateTime"]),
                            AssetName = dr["AssetName"].ToString(),
                            IsRegistered = Convert.ToByte(dr["IsRegistered"]),

                        });

                    }


                }
                //else
                //{

                //    MessageBox.Show("No data present.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //}

                return result;

            }
            catch (Exception ex)
            {

                throw;

            }

        }

        //TO Load DataGrid
        //public List<AssetMasterModel> GetAllAssets(string CustomerID)
        //{
        //    List<AssetMasterModel> result = new List<AssetMasterModel>();
        //    try
        //    {
        //        if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
        //            con.Open();

        //        SqlCommand cmd = new SqlCommand(AppSettings.StoredProcedure.GetAllAsset, con);
        //        //cmd.Connection.Open();
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@TenantID", CustomerID);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);
        //        cmd.ExecuteNonQuery();
        //        //cmd.Connection.Close();
        //        con.Close();

        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in ds.Tables[0].Rows)
        //            {
        //                result.Add(new AssetMasterModel
        //                {
        //                    AssetID = dr["AssetID"].ToString(),
        //                    AName = dr["AName"].ToString(),
        //                    ADescription = dr["ADescription"].ToString(),
        //                    ATypeID = Convert.ToInt32(dr["ATypeID"]),
        //                    //ATagID = dr["ATagID"].ToString(),
        //                    //SID=Convert.ToInt64( dr["SID"]),
        //                    //ASerialNo=Convert.ToInt32( dr["ASerialNo"]),
        //                    IsActive = Convert.ToByte(dr["IsActive"]),
        //                    UserID = dr["UID"].ToString(),
        //                    //AStateDateTime=Convert.ToDateTime( dr["AStateDateTime"]),
        //                    //AState = dr["AState"].ToString(),
        //                    //LastInventoryDateTime=Convert.ToDateTime( dr["LastInventoryDateTime"]),
        //                    CustomerID = dr["CustomerID"].ToString(),
        //                    //TransactionDateTime = Convert.ToDateTime( dr["TransactionDateTime"]),
        //                    AssetName = dr["AssetName"].ToString(),
        //                    IsRegistered = Convert.ToByte(dr["IsRegistered"]),

        //                });

        //            }


        //        }
        //        //else
        //        //{

        //        //    MessageBox.Show("No data present.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //        //}

        //        return result;

        //    }
        //    catch (Exception ex)
        //    {

        //        throw;

        //    }

        //}

        public bool DeleteRegisteredAsset(string AssetId)
        {
            try
            {
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
                    con.Open();
                var _params = new DynamicParameters();
                _params.Add("@AssetID", AssetId);
                con.Query(AppSettings.StoredProcedure.DeleteRegisteredAssets, _params, commandType: CommandType.StoredProcedure);
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public void UpdateAssets(AssetMasterModel assetMaster)
        {
            try
            {
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
                    con.Open();
                var _params = new DynamicParameters();
                _params.Add("@AssetID", assetMaster.AssetID);
                _params.Add("@AName", assetMaster.AName);
                _params.Add("@ADescription", assetMaster.ADescription);
                _params.Add("@IsActive", assetMaster.IsActive);

                var result = con.Query<Response>(AppSettings.StoredProcedure.UpdateRegisteredAssets, _params, commandType: CommandType.StoredProcedure).FirstOrDefault();
                con.Close();
                //return result;
            }
            catch (Exception ex)
            {
                //  MessageBox.Show("Error while updating the Asset", ex.Message);
            }


        }

        public int InsertSerialNo(string serialno, string tagID, string custiD, DateTime transaction, string chipID)

        {

            try

            {

                int retValue = 0;
                SqlCommand com = new SqlCommand(AppSettings.StoredProcedure.InsertSerialNo, con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@SerialNo", serialno);
                com.Parameters.AddWithValue("@tagID", tagID);
                com.Parameters.AddWithValue("@CustomerID", custiD);
                com.Parameters.AddWithValue("@servertime", transaction);
                com.Parameters.AddWithValue("@chipID", chipID);
                con.Open();
                retValue = com.ExecuteNonQuery();
                con.Close();
                return retValue;
                //MessageBox.Show("Serial Number was added sucessfully", "Information...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public int UpdateChipID(string tagID, string custiD, string chipID)

        {

            try

            {

                int retValue = 0;



                SqlCommand com = new SqlCommand(AppSettings.StoredProcedure.UpdateChipID, con);

                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.AddWithValue("@tagID", tagID);

                com.Parameters.AddWithValue("@CustomerID", custiD);

                com.Parameters.AddWithValue("@chipID", chipID);

                con.Open();

                retValue = com.ExecuteNonQuery();

                con.Close();

                return retValue;

                //MessageBox.Show("Serial Number was added sucessfully", "Information...", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            catch (Exception ex)

            {

                throw ex;

            }

        }






    }
}
