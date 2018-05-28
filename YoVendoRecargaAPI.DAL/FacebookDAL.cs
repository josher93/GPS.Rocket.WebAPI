using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.DAL
{
    public class FacebookDAL
    {
        Connection connection = new Connection();

        #region YoVendoRecarga


        public FacebookEN GetFacebookProfile(int pPersonID)
        {
            FacebookEN profile = null;

            try
            {
                connection.Cnn.Open();
                profile = connection.Cnn.GetList<FacebookEN>().Where(f => f.PersonID == pPersonID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                profile = null;
                Console.WriteLine(ex.Message);
                EventViewerLoggerDAL.LogError("GetFacebookProfile: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return profile;
        }

        public int UpdateFacebookProfile(FacebookEN pNewProfileData)
        {
            int updated = default(int);
            try
            {
                connection.Cnn.Open();
                updated = connection.Cnn.Update(pNewProfileData);
            }
            catch (Exception ex)
            {
                updated = 0;
                Console.WriteLine(ex.Message);
                EventViewerLoggerDAL.LogError("UpdateFacebookProfile: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return updated;
        }

        public int InsertFacebookProfile(FacebookEN pProfileData)
        {
            int inserted = default(int);
            try
            {
                connection.Cnn.Open();
                inserted = connection.Cnn.Insert(pProfileData) ?? default(int);
            }
            catch (Exception ex)
            {
                inserted = 0;
                Console.WriteLine(ex.Message);
                EventViewerLoggerDAL.LogError("InsertFacebookProfile: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return inserted;
        }

        #endregion

    }
}
