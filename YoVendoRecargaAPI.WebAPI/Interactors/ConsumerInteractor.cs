using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.BL.Utils;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class ConsumerInteractor
    {
        ConsumerBL registerBL = new ConsumerBL();

        public RegisterConsumerResponse CreatorRegisterResponse(ConsumerEN pRegister)
        {
            RegisterConsumerResponse response = new RegisterConsumerResponse();

            response.firstName = pRegister.FirstName;
            response.middleName = pRegister.MiddleName;
            response.lastName = pRegister.LastName;
            response.nickname = pRegister.Nickname;
            response.phone = pRegister.Phone;
            response.email = pRegister.Email;
            response.countryID = pRegister.CountryID;
            response.authenticationKey = pRegister.ConsumerAuthKey;

            return response;
        }

        public RegisterConsumerResponse CreatorUpdateResponse(ConsumerEN pConsumerUpdated)
        {
            RegisterConsumerResponse response = new RegisterConsumerResponse();

            try
            {
                response.firstName = pConsumerUpdated.FirstName;
                response.middleName = pConsumerUpdated.MiddleName;
                response.lastName = pConsumerUpdated.LastName;
                response.nickname = pConsumerUpdated.Nickname;
                response.phone = pConsumerUpdated.Phone;
                response.email = pConsumerUpdated.Email;
                response.countryID = pConsumerUpdated.CountryID;
                response.authenticationKey = pConsumerUpdated.ConsumerAuthKey;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }

            return response;
        }

    }
}