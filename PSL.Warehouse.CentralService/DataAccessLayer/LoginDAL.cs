using LaundryManagementSystem.Models;
using Psl.Chase.Utils;
//using PSL.Laundry.CentralService.Global;
using PSL.Laundry.CentralService.Models;
using PSL.Warehouse.CentralService.Global;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LaundryManagementSystem.DatabaseAccessLayer
{
    public class LoginDAL
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);


        public List<LoginModelDesktop> GetUserDetails(string UserName, string password, string ClientDeviceID)
        {
            //string pass = CryptorEngine.Encrypt(password.Trim(), true);
            //string de = CryptorEngine.Decrypt(pass.Trim(), true);

            List<LoginModelDesktop> userdetails = new List<LoginModelDesktop>();
            try
            {
                if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
                    con.Open();

                SqlCommand cmd = new SqlCommand(AppSettings.StoredProcedure.GetUserDetails, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ClientDeviceId", ClientDeviceID);
                cmd.Parameters.AddWithValue("@username", UserName);
                cmd.Parameters.AddWithValue("@password", password);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.ExecuteNonQuery();
                con.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        userdetails.Add(new LoginModelDesktop
                        {

                            UserID = dr["UserID"].ToString(),
                            //UserName = dr["UserName"].ToString(),
                            //FirstName = dr["FirstName"].ToString(),
                            CustomerID = dr["CustomerID"].ToString(),
                            //CustomerName = dr["CustomerName"].ToString(),
                            CompanyCode = dr["CompanyCode"].ToString()

                        });

                    }
                }
                return userdetails;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        //public List<LoginModelWeb> GetUserDetails(string UserName, string CustomerID, string password)
        //{
        //    //string pass = CryptorEngine.Encrypt(password.Trim(), true);
        //    //string de = CryptorEngine.Decrypt(pass.Trim(), true);

        //    List<LoginModelWeb> userdetails = new List<LoginModelWeb>();
        //    try
        //    {
        //        if (con.State == ConnectionState.Closed || con.State == ConnectionState.Connecting)
        //            con.Open();

        //        SqlCommand cmd = new SqlCommand(AppSettings.StoredProcedure.GetUserDetails, con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@ComapnyId", CustomerID);
        //        cmd.Parameters.AddWithValue("@UserName", UserName);
        //        cmd.Parameters.AddWithValue("@password", password);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataSet ds = new DataSet();
        //        da.Fill(ds);
        //        cmd.ExecuteNonQuery();
        //        con.Close();

        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in ds.Tables[0].Rows)
        //            {
        //                userdetails.Add(new LoginModelWeb
        //                {

        //                    UserName = dr["UserName"].ToString(),
        //                    Password = dr["Password"].ToString(),
        //                    UserID = dr["UserID"].ToString(),
        //                    FirstName = dr["FirstName"].ToString(),
        //                    CustomerName = dr["CustomerName"].ToString(),

        //                });

        //            }
        //        }
        //        return userdetails;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //}

    }
}
