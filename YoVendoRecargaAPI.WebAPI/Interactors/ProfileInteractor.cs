using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class ProfileInteractor
    {
        public ProfileResponse CreateProfileResponse(ProfileEN pProfileID)
        {
            ProfileResponse response = new ProfileResponse();
            UserProfile profile = new UserProfile();
            response.profile = new UserProfile();
            
            try
            {
                    response.profile.Id = pProfileID.ProfileID;
                    response.profile.birthday = pProfileID.Birthday;
                    response.profile.first_name = pProfileID.FirstName;
                    response.profile.last_name = pProfileID.LastName;
                    response.profile.verified = pProfileID.Verified;
                    response.profile.email = pProfileID.Email;
                    response.profile.NickName = pProfileID.Nickname;
                    response.profile.vendorId = pProfileID.VendorID;
                    response.profile.Iso2code = pProfileID.Iso2Code;
                    response.profile.Symbol = pProfileID.Symbol;
                    response.profile.Personphone = pProfileID.PersonPhone;
                    response.profile.LastSale = pProfileID.LastSale;
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return response;
        }
    }
}