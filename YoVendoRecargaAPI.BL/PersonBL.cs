using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.DAL;
using YoVendoRecargaAPI.BL.Utils;
using System.Configuration;
using YoVendoRecargaAPI.SVC;

namespace YoVendoRecargaAPI.BL
{
    public class PersonBL
    {
        PersonDAL personDAL = new PersonDAL();
        SessionDAL sessionDAL = new SessionDAL();
        BagDAL bagDAL = new BagDAL();
        JWT jwt = new JWT();
        EmailSender emailSender = new EmailSender();
        IPAddressClient ipAddressClient = new IPAddressClient();

        /// <summary>
        /// Verifies user credentials (Email and Password) and returns user's initial data.
        /// </summary>
        /// <param name="pEmail">Email</param>
        /// <param name="pPassword">Password</param>
        /// <param name="pDeviceInfo">Devices manufacturer, operative system, and version.</param>
        /// <param name="pDeviceIP">Client's IP address</param>
        /// <param name="pDeviceID">Device identification</param>
        /// <param name="pEncrypted">Determinates if password proveded is encrypted or not</param>
        /// <returns>Autehnticated person's initial data</returns>
        public PersonEN AuthenticatePerson(string pEmail, string pPassword, string pDeviceInfo, string pDeviceIP, string pDeviceID, bool pEncrypted)
        {
            PersonEN person = new PersonEN();
            try
            {
                if (pEncrypted)
                {
                    pPassword = Encrypt.DecryptStringAESVector2(pPassword);
                }

                var nepass = Encrypt.EncryptStringAES(pPassword);
                pPassword = nepass;
                var available = false;
                person = personDAL.AuthenticatePerson(pEmail, pPassword);
                if (person.RollID == 1 && person.CategoryID == 0)
                {
                    available = true;
                }
                else if (person.RollID == 1 && person.CategoryID == 1)
                {
                    available = true;
                }
                else if (person.RollID == 2 && person.CategoryID == 1)
                {
                    available = true;
                }
                else if (person.RollID == 2 && person.CategoryID == 0)
                {
                    available = true;
                }
                else
                {
                    available = true;
                }


                if (person != null)
                {
                    if ((available == false && !person.Active))
                    {
                        EventViewerLoggerBL.LogInformation("Signin Person: HDM desactivado");
                        return null;
                    }
                    else
                    {
                        person.TokenExpiration = DateTime.Now.AddHours(Constants.TokenLifetime);
                        person.CurrentToken = jwt.encode(person);
                        person.VendorM = (person.MasterID != 0) ? true : false;
                        person.DeviceIp = pDeviceIP;
                        person.DeviceInfo = pDeviceInfo;
                        person.DeviceID = pDeviceID;

                        int sessionID = sessionDAL.InsertAuthenticationLog(person.PersonID, person.DeviceIp, person.DeviceInfo, person.DeviceID, true, CentralAmericaDateTime(DateTime.Now));
                        person.SessionID = sessionID;

                        ManageUserSessions(sessionID, person);

                        //UserBag
                        person.OperatorsBalance = bagDAL.GetUserOperatorBag(person.PersonID);

                        if (person.ReferredStatus == 0)
                        {
                            personDAL.UpdatePersonReferredStatus(1, person.Email);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                person = null;
                //Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("AuthenticatePerson, Error: " + ex.StackTrace);
            }


            return person;
        }

        /// <summary>
        /// Verifies if authentication token has not expired. Returns user's initial data
        /// </summary>
        /// <param name="pToken">Current token</param>
        /// <returns>PersonEN: Person's initial data</returns>
        public PersonEN VerifyPersonAuthentication(IEnumerable<String> pToken)
        {
            PersonEN verifiedPerson = new PersonEN();

            try
            {
                var decryptedData = jwt.decode(pToken.First().ToString());

                verifiedPerson = personDAL.VerifyPerson(decryptedData);

                if (verifiedPerson != null)
                {
                    verifiedPerson.IsValidToken = (decryptedData.TokenExpiration < DateTime.Now) ? false : true;
                }
            }
            catch (Exception ex)
            {
                verifiedPerson = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return verifiedPerson;
        }

        /// <summary>
        /// Generates a new token with renewed expiration date.
        /// </summary>
        /// <param name="pPerson">Vendor data to encrypt</param>
        /// <returns>String: New generated token.</returns>
        public string RenewAuthToken(PersonEN pPerson)
        {
            try
            {
                pPerson.TokenExpiration = DateTime.Now.AddHours(Constants.TokenLifetime);
                pPerson.CurrentToken = jwt.encode(pPerson);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return pPerson.CurrentToken;
        }


        private void ManageUserSessions(int pSessionID, PersonEN pPerson)
        {
            int quantityMobile = 0;
            int quantityOther = 0;
            string lastMobileDevice = "";
            string lastOtherDevice = "";

            try
            {
                var sessions = sessionDAL.GetActiveUserSessions(pPerson.PersonID, true);
                if (sessions != null)
                {
                    if (sessions.Count >= 2)
                    {
                        //Cuenta cuantos dispositivos estan activos por tipo
                        foreach (var item in sessions)
                        {
                            if (item.MobileDevice)
                                quantityMobile++;
                            else
                                quantityOther++;
                        }

                        var lastDevice = sessions.ElementAt(1); //Obtiene el ultimo segundo de la lista

                        if (lastDevice.MobileDevice)
                            lastMobileDevice = lastDevice.DeviceID;
                        else
                            lastOtherDevice = lastDevice.DeviceID;

                        string vendorFullname = String.Format("{0} {1}", pPerson.Firstname, pPerson.Lastname);

                        if (quantityMobile > 1)
                        {
                            //Modifica el estado de la penultima sesión 'Activa' a 'Inactiva'
                            //en el último dispositivo Movil
                            var closeSession = sessionDAL.ClosePenultimateSession(pPerson.PersonID, true);

                            if (lastMobileDevice != pPerson.DeviceID)
                            {
                                if (!Boolean.Parse(ConfigurationManager.AppSettings["SilentSigninMode"]))
                                    emailSender.EmailNewLogin(vendorFullname, pPerson.Email, pPerson.DeviceInfo, CentralAmericaDateTime(DateTime.Now), ipAddressClient.GetLocationByIpAddress(pPerson.DeviceIp));
                            }
                        }

                        if (quantityOther > 1)
                        {
                            var closeSession = sessionDAL.ClosePenultimateSession(pPerson.PersonID, false);

                            if (lastOtherDevice != pPerson.DeviceID)
                            {
                                if (!Boolean.Parse(ConfigurationManager.AppSettings["SilentSigninMode"]))
                                    emailSender.EmailNewLogin(vendorFullname, pPerson.Email, pPerson.DeviceInfo, CentralAmericaDateTime(DateTime.Now), ipAddressClient.GetLocationByIpAddress(pPerson.DeviceIp));
                            }
                        }
                    }
                }
                else
                {
                    EventViewerLoggerBL.LogError("No sessions were found for current user");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("ManageUserSessions " + ex.Message);
            }
        }



        private DateTime CentralAmericaDateTime(DateTime pDate)
        {
            DateTime utcDateTime = pDate.ToUniversalTime();

            string ca_TimeZoneKey = "Central America Standard Time";
            TimeZoneInfo caTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ca_TimeZoneKey);
            DateTime caDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, caTimeZone);

            return caDateTime;
        }


        public SimpleOperationModel AddPersonNickname(PersonEN pPerson, string pNickname)
        {
            SimpleOperationModel result = new SimpleOperationModel();
            result.Message = "error";
            result.Result = false;

            try
            {
                var profile = personDAL.GetGamerProfileByNickname(pNickname);

                if (profile == null)
                {
                    profile = new GamerProfileEN();
                    profile.Nickname = pNickname;
                    profile.PersonID = pPerson.PersonID;
                    profile.RegDate = DateTime.Now;

                    if (personDAL.InsertGamerProfile(profile) > 0)
                    {
                        result.Result = true;
                        result.Message = "inserted";
                    }
                }
                else
                {
                    result.Result = false;
                    result.Message = "conflict";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("AddPersonNickname: " + ex.Message);
            }

            return result;
        }


        #region YCR

        public FacebookConsumerEN authenticateConsumer(string Token)
        {
            FacebookConsumerEN ConsumerDecoded = new FacebookConsumerEN();

            try
            {
                ConsumerAuthEN AuthKey = sessionDAL.GetAuthKey(Token);

                string decodedProfileId = jwt.decodeFacebookID(AuthKey.ConsumerAuthKey);

                ConsumerDecoded = sessionDAL.GetFacebookConsumer(decodedProfileId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return ConsumerDecoded;
        }

        #endregion
    }
}
