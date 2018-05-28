using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using Dapper;
using System.Data;

namespace YoVendoRecargaAPI.DAL
{
    public class ProfileDAL
    {
        private Connection cnn = new Connection();

        public ProfileEN GetProfile(int pProfileID)
        {
            ProfileEN userProfile = new ProfileEN();

            try
            {
                userProfile = cnn.Cnn.Query<ProfileEN>("SpGetProfile",
                    new { personid = pProfileID }, commandType: CommandType.StoredProcedure).FirstOrDefault();

                userProfile.LastSale = cnn.Cnn.Query<string>("SpGetLastSale",
                    new { personid = pProfileID }, commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error ProfileDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return userProfile;
        }
    }
}
