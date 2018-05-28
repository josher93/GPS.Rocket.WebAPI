using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoVendoRecargaAPI.SVC.Utils
{
    public class Constants
    {
        //  **************************
        //
        //      AZURE MOBILE ENGAGEMENTE
        //
        //  **************************
        public const string TENANT_ID = "67d1eefd-a1ac-4a93-9178-8a5dd724ec87";
        public const string CLIENT_ID = "21b17995-963f-465b-9952-8e7d98d27269";
        public const string CLIENT_SECRET = "VV3ymKHK3XESvEL/rRtBHBq0e443MffRojI5QLUedso=";
        public const string SUBSCRIPTION_ID = "56e1a228-00c1-4ba0-9c99-1868298598e1";


        public const string RESOURCE_GROUP = "MobileEngagement";

        // For Mobile Engagement operations
        // App Collection Name 
        public const string APP_COLLECTION_NAME = "CEOAnalyticsYVS";


        public const string APP_RESOURCE_NAME_ANDROID = "AnalyticsYVS_Android";
        public const string APP_RESOURCE_NAME_IOS = "AnalyticsYVS";

        public const int DepositConfCampaign = 316;
        public const int TopupRequestCampaign = 304;


        //  **************************
        //
        //      ONE SIGNAL
        //
        //  **************************

        public const string OneSignalURL = "https://onesignal.com";

        public const string OneSignalNotifications = "https://onesignal.com/api/v1/notifications";

        //YoVendoRecarga
        public const string KEY_ONESIGNAL_YVR_IOS = "NjFlMTc5ZDUtZmJhNS00YmE1LWI3MjMtYWZjYzVhNmNhZmEw";

        public const string KEY_ONESIGNAL_YVR_ANDROID = "YzY4YjRkNDMtNzM4MC00YmNiLWJhOWMtNDQwOTBiYjk0MTBh";

        public const string APPID_ONESIGNAL_YVR_IOS = "c0ac64d0-07c1-4f0d-b2ae-850d1bd84149";

        public const string APPID_ONESIGNAL_YVR_ANDROID = "85d2fc7b-04f4-4aaf-8ebc-8d719bcadde0";

        //RecarGO
        public const string KEY_ONESIGNAL_RGO_ANDROID = "ZDJiYmZlMzEtZTJkMS00ZjRjLTg0NmUtYjM5OTA3ZjgyZTg3";

        public const string KEY_ONESIGNAL_RGO_IOS = "MGE4MzMzMjEtNTgxZi00MjljLWFmNDQtN2ZkZWE2Y2Y4ZDVh";

        public const string APPID_ONESIGNAL_RGO_ANDROID = "1ec1e355-543b-4e69-8651-d2d0ac579797";

        public const string APPID_ONESIGNAL_RGO_IOS = "880769b5-e7e3-480d-aeaf-3dda19f12821";


        //  **************************
        //
        //      TWILIO
        //
        //  **************************

        public const string TwilioAccountID = "AC18b7272bb43f410fc7b8cfc8474b9b0b";
        public const string TwilioAuthToken = "96d651ee68e79f4b481b08b9eec7c9ad";


        //Event Viewer values
        public static int EventLogID = 102;
        public static string EventLogAppName = "YoVendoRecargaAPI";
        public static short EventLogCategoryError = 1;
        public static short EventLogCategory = 2;

    }
}
