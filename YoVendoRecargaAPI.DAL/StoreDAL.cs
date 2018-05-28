using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.DAL
{
    public class StoreDAL
    {
        private Connection con { get; set; }
        private SqlConnection connection { get; set; }

        public StoreDAL()
        {
            this.con = new Connection();
        }
        public int? AirTimeReporting(string pStoreName, string pAddressStore, decimal pLongitude, decimal pLatitude, string pFirebaseID, int pConsumerID, DateTime pRegDate, DateTime pModDate, ref string error)
        {

            //int result = 0;
            StoreReportDetailsEN storeReport = new StoreReportDetailsEN();

            int? resultInsert = default(int);

            try
            {

                con.Cnn.Open();
                
                //result = con.Cnn.Execute("SpInsertAirTimeReporting", 
                //    new
                //    {
                //        StoreName = pStoreName,
                //        AddressStore = pAddressStore,
                //        Longitude = pLongitude,
                //        Latitude = pLatitude,
                //        FirebaseID = pFirebaseID,
                //        ConsumerID = pConsumerID
                //    },
                // commandType: CommandType.StoredProcedure);

                storeReport.StoreName = pStoreName;
                storeReport.AddressStore = pAddressStore;
                storeReport.Longitude = pLatitude;
                storeReport.Latitude = pLatitude;
                storeReport.FirebaseID = pFirebaseID;
                storeReport.ConsumerID = pConsumerID;
                storeReport.RegDate = DateTime.Now;
                storeReport.ModDate = DateTime.Now;

                resultInsert = con.Cnn.Insert(storeReport);
                
            }
            catch (Exception ex)
            {
                resultInsert = default(int);
                error = ex.Message;
                Console.WriteLine("Error StoreDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return resultInsert;
        }


        #region GetStoreItems
        public List<StoreEN> GetStoreItems(int ConsumerID)
        {
            List<StoreEN> result = new List<StoreEN>();

            try
            {

                con.Cnn.Open();

                result = con.Cnn.Query<StoreEN>("SpGetStoreItems", new { ConsumerID = ConsumerID }, commandType: CommandType.StoredProcedure).AsList();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error StoreDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                con.Cnn.Close();
            }

            return result;

        }
        #endregion
    }
}
