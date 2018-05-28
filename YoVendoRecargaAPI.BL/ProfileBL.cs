using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.DAL;

namespace YoVendoRecargaAPI.BL
{
    public class ProfileBL
    {
        ProfileDAL profileDAL = new ProfileDAL();

        public ProfileEN GetProfile(int pProfileID)
        {
            ProfileEN userProfile = new ProfileEN();

            try
            {
                userProfile = profileDAL.GetProfile(pProfileID);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return userProfile;
        }
    }
}
