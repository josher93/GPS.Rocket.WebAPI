using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.DAL;

namespace YoVendoRecargaAPI.BL
{
    public class FacebookBL
    {
        FacebookDAL facebookDAL = new FacebookDAL();

        public string AssociateFacebook(FacebookEN pFacebookProfile)
        {
            string result = "";

            try
            {
                pFacebookProfile.RegDate = DateTime.Now;

                var existsProfile = facebookDAL.GetFacebookProfile(pFacebookProfile.PersonID);
                
                if (existsProfile != null)
                {
                    pFacebookProfile.Id = existsProfile.Id;
                    result = (facebookDAL.UpdateFacebookProfile(pFacebookProfile) > 0) ? "updated" : "error";
                }
                else
                {
                    result = (facebookDAL.InsertFacebookProfile(pFacebookProfile) > 0) ? "inserted" : "error";
                }
            }
            catch (Exception ex)
            {
                result = "error";
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return result;
        }
    }
}
