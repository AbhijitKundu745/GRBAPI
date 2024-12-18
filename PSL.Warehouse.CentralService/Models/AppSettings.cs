﻿using System;

namespace LaundryManagementSystem.Models
{
    public static class AppSettings
    {
        public static string connectionString { get; set; }
        public static Guid? UserID { get; set; }

        public static Guid? CustomerID { get; set; }

        public static class StoredProcedure
        {

            //Login
            public static string GetUserDetails { get; set; } = "Prc_GetUserLogin";
            
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
        }
    }


}
