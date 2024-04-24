using Dapper;
using LaundryManagementSystem.Models;
//using PSL.Laundry.CentralService.DBContext;
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
    public class ProcessesDAL
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);

        public List<Models.Vendor> GetVendors(string CustomerID)
        {
            List<Models.Vendor> resultvendors = new List<Models.Vendor>();

            try
            {
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
                    con.Open();

                SqlCommand cmd = new SqlCommand(AppSettings.StoredProcedure.GetVendors, con);
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
                        resultvendors.Add(new Models.Vendor
                        {
                            VendorID = (int)dr["VendorID"],
                            Name = dr["Name"].ToString(),
                            Location = dr["Location"].ToString(),
                        });

                    }
                }
                return resultvendors;

            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public int InsertActivity(ActivityModelWeb activity)
        {
            List<ActivityModelWeb> resultInsActivity = new List<ActivityModelWeb>();

            try
            {
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
                    con.Open();

                SqlCommand cmd = new SqlCommand(AppSettings.StoredProcedure.InsertActivity, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActivityType", activity.ActivityType);
                cmd.Parameters.AddWithValue("@StartDate", activity.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", activity.EndDate);
                cmd.Parameters.AddWithValue("@TransactionDateTime", activity.TransactionDateTime);
                cmd.Parameters.AddWithValue("@VendorID ", activity.VendorID);
                cmd.Parameters.AddWithValue("@CustomerID", activity.CustomerID);
                cmd.Parameters.AddWithValue("@TouchPointID", activity.TouchPointID);
                cmd.Parameters.AddWithValue("@Count", activity.Count);
                cmd.Parameters.AddWithValue("@UID", activity.UID);
                cmd.Parameters.AddWithValue("@ActivityID", activity.ActivityID);



                // SqlDataAdapter da = new SqlDataAdapter(cmd);
                // DataSet ds = new DataSet();
                //da.Fill(ds);
                cmd.ExecuteNonQuery();
                con.Close();
                return 1;
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    foreach (DataRow dr in ds.Tables[0].Rows)
                //    {
                //        resultInsActivity.Add(new ActivityModelWeb
                //        {
                //            ActivityID = dr["ActivityID"].ToString(),


                //        });

                //    }
                //}
                //return resultInsActivity[0].ActivityID.ToString();

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<AssetMasterModel> GetTagDetails(string CustomerID)
        {
            List<AssetMasterModel> resultTagDetails = new List<AssetMasterModel>();
          //  SqlConnection con2 = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            try
            {
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
                    con.Open();

                SqlCommand cmd = new SqlCommand(AppSettings.StoredProcedure.GetTagDetails, con);
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
                        resultTagDetails.Add(new AssetMasterModel
                        {
                            AssetID = dr["AssetID"].ToString(),
                            AName = dr["AName"].ToString(),
                            ADescription = dr["ADescription"].ToString(),
                            ATypeID = Convert.ToInt32(dr["ATypeID"]),
                            ATagID = dr["ATagID"].ToString(),
                            //SID=Convert.ToInt64( dr["SID"]),
                            ASerialNo = Convert.ToInt32(dr["ASerialNo"]),
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
                return resultTagDetails;

            }
            catch (Exception ex)
            {
              //  MessageBox.Show(ex.Message);
                return resultTagDetails;
            }
        }

        public void InsertActivityDetails(ActivityModelWeb activity, string AssetID, string LastInvDatetime, int AssetLife)
        {


            DateTime? LastInventoryDateTime = DateTime.Now;//Convert.ToDateTime(LastInvDatetime);
            try
            {
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
                    con.Open();
                var _params = new DynamicParameters();
                _params.Add("@AssetID", AssetID);
                _params.Add("@ActivityID", activity.ActivityID);
                _params.Add("@UID", activity.UID);
                _params.Add("@AState", activity.ActivityType);
                _params.Add("@AssetLife", AssetLife);
                _params.Add("@LastInventoryDateTime", LastInvDatetime);

                con.Execute(AppSettings.StoredProcedure.InsertActivityDetails, _params, commandType: CommandType.StoredProcedure);
                con.Close();

            }
            catch (Exception ex)
            {
                throw;
            }
            //return result;


        }

        public int GetAssetLife(string AssetID)
        {
            int AssetLife = 0;
            try
            {
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
                    con.Open();

                SqlCommand cmd = new SqlCommand(AppSettings.StoredProcedure.GetAssetLife, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AssetID", AssetID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.ExecuteNonQuery();
                con.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        AssetLife = Convert.ToInt32(dr["AssetLife"]);
                    }
                }

                return AssetLife;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void InsertErrorActivity(ActivityModelWeb activity, string TagID)
        {
            try
            {


                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
                    con.Open();
                var _params = new DynamicParameters();
                _params.Add("@ActivityID", activity.ActivityID);
                _params.Add("@TagID", TagID);

                var result = con.Execute(AppSettings.StoredProcedure.InsertErrorActivity, _params, commandType: CommandType.StoredProcedure);
                con.Close();

            }
            catch (Exception ex)
            {
                throw;
            }


        }




    }
}
