using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jose;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.BL
{
    public class JWT
    {
        private string input = "YoVendoRecargaAPI";
        private string inputYCR = "esta es una peueba";
        public String encode(PersonEN user)
        {
            byte[] secretKey = Encoding.ASCII.GetBytes(input);
            string token;
            try
            {
                token = Jose.JWT.Encode(createToken(user), secretKey, JwsAlgorithm.HS256);
            }
            catch (Exception)
            {
                throw;
            }
            return token;
        }

        public PersonEN decode(String token)
        {
            PersonEN user = new PersonEN();

            try
            {
                byte[] secretKey = Encoding.ASCII.GetBytes(input);
                var tokenDecoded = Jose.JWT.Decode<TokenEN>(token, secretKey);
                user = createPersonToken(tokenDecoded);
            }
            catch (Exception)
            {

                throw;
            }
            return user;
        }

        #region ExternalClients

        public String encodeClientKey(ExternalClientEN pClient)
        {
            string clientKey;
            byte[] secretKey = Encoding.ASCII.GetBytes(input);
            
            try
            {
                clientKey = Jose.JWT.Encode(pClient, secretKey, JwsAlgorithm.HS256);
            }
            catch (Exception)
            {
                throw;
            }
            return clientKey;
        }

        public ExternalClientEN decodeClientKey(String clientKey)
        {
            ExternalClientEN client = new ExternalClientEN();

            try
            {
                byte[] secretKey = Encoding.ASCII.GetBytes(input);
                client = Jose.JWT.Decode<ExternalClientEN>(clientKey, secretKey);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("decodeClientKey: " + ex.Message);
            }
            return client;
        }

        #endregion


        #region     YoComproRecarga

        public String encodeFacebookId(String profileID)
        {
            byte[] secretKey = Encoding.ASCII.GetBytes(inputYCR);
            string token;
            try
            {
                token = Jose.JWT.Encode(profileID, secretKey, JwsAlgorithm.HS256);
            }
            catch (Exception)
            {
                throw;
            }
            return token;
        }

        public String decodeFacebookID(String ProfileID)
        {
            String decodedProfileID = "";

            try
            {
                byte[] secretKey = Encoding.ASCII.GetBytes(inputYCR);
                decodedProfileID = Jose.JWT.Decode<String>(ProfileID, secretKey);
            }
            catch (Exception)
            {


                throw;
            }
            return decodedProfileID;
        }


        #endregion

        private TokenEN createToken(PersonEN pPerson)
        {
            TokenEN token = new TokenEN();
            try
            {
                token.PersonID = pPerson.PersonID;
                token.PersonEmail = pPerson.Email;
                token.Password = pPerson.Password;
                token.ExpirationDate = pPerson.TokenExpiration;
                token.VendorCode = pPerson.VendorCode;
                token.ProfileCompleted = pPerson.ProfileCompleted;
                token.DeviceIP = pPerson.DeviceIp;
                token.DeviceID = pPerson.DeviceID;
                token.MasterID = pPerson.MasterID;
                token.CountryID = pPerson.CountryID;
                token.State = pPerson.Active;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return token;
        }

        private PersonEN createPersonToken(TokenEN pToken)
        {
            PersonEN person = new PersonEN();
            try
            {
                person.PersonID = pToken.PersonID;
                person.Email = pToken.PersonEmail;
                person.Password = pToken.Password;
                person.TokenExpiration =  pToken.ExpirationDate;
                person.VendorCode = pToken.VendorCode;
                person.ProfileCompleted = pToken.ProfileCompleted;
                person.DeviceIp = pToken.DeviceIP = pToken.DeviceIP;
                person.DeviceID = pToken.DeviceID;
                person.MasterID = pToken.MasterID;
                person.CountryID = pToken.CountryID;
                person.Active = pToken.State;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return person;
        }
    }
}
