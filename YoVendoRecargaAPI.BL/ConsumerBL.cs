using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.DAL;
using YoVendoRecargaAPI.BL.Utils;
using System.Text.RegularExpressions;

namespace YoVendoRecargaAPI.BL
{
    public class ConsumerBL
    {
        ConsumerDAL consumerDAL = new ConsumerDAL();
        JWT jwt = new JWT();

        public ConsumerEN InsertConsumer(string pPhone, int pCountryID, string pDeviceID, string pURL, string pEmail, string pProfileID, string pUserID, string pFirstName, string pMiddleName, string pLastName, string pNickName, string pConsumerAuthKey)
        {
            ConsumerEN register = new ConsumerEN();

            try
            {
                //ingresa en base de datos un Consumer
                register = consumerDAL.InsertConsumer(pPhone, pCountryID, pDeviceID, pURL, pEmail, pProfileID, pUserID, pFirstName, pMiddleName, pLastName, pNickName);

                //Trae registro de la base de datos recien ingresado por ProfileID
                ConsumerEN consumerInDB = new ConsumerEN();
                consumerInDB = consumerDAL.GetConsumerByProfileID(pProfileID);


                //Codifica el profileID
                string encodedProfileID = jwt.encodeFacebookId(pProfileID);

                //Inserta en Consumer.ConsumerAuth un encodedProfileID
                ConsumerAuthKeyEN consumerAuth = new ConsumerAuthKeyEN();
                consumerAuth.ConsumerID = consumerInDB.ConsumerID;
                consumerAuth.ConsumerAuthKey = encodedProfileID;
                consumerAuth.RegDate = DateTime.Now;

                ConsumerAuthKeyEN insertConsumerAuth = consumerDAL.InsertConsumerAuth(consumerAuth);


                //Trae el Token del Consumer recien ingresado en la base de datos

                var authToken = consumerDAL.GetAuthConsumer(consumerInDB.ConsumerID);

                //asigno valor a propiedad ConusmerAuthToken de la entidad
                register.ConsumerAuthKey = authToken.ConsumerAuthKey;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return register;
        }

        public bool SelectConsumerProfile(string pProfileID)
        {
            bool consumerFound = false;

            ConsumerEN selectedConsumer = new ConsumerEN();

            try
            {
                selectedConsumer = consumerDAL.GetProfileIDByProfileID(pProfileID);

                if (selectedConsumer != null)
                {
                    consumerFound = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return consumerFound;
        }

        public ConsumerEN UpdateConsumerProfile(string pPhone, int pCountryID, string pDeviceID, string pURL, string pEmail, string pProfileID, string pUserID, string pFirstName, string pMiddleName, string pLastName, string pConsumerAuth)
        {
            ConsumerEN consumerToUpdate = new ConsumerEN();
            ConsumerEN bringConsumer = new ConsumerEN();

            try
            {
                consumerToUpdate = consumerDAL.UpdateConsumerProfile(pPhone, pCountryID, pDeviceID, pURL, pEmail, pProfileID, pUserID, pFirstName, pMiddleName, pLastName);

                bringConsumer = consumerDAL.GetConsumerByProfileID(pProfileID);

                if (bringConsumer != null)
                {
                    //var ConsumerUpdated = registerConsumerDAL.GetConsumerByProfileID(pProfileID);
                    ConsumerEN ConsumerUpdated = consumerDAL.GetConsumerByProfileID(pProfileID);

                    var Token = consumerDAL.GetAuthConsumer(bringConsumer.ConsumerID);

                    bringConsumer.ConsumerAuthKey = Token.ConsumerAuthKey;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return bringConsumer;
        }

        public ConsumerEN AuthenticateConsumer(string pToken)
        {
            ConsumerEN consumer = null;

            try
            {
                AuthConsumerEN authKey = consumerDAL.GetConsumerAuthByToken(pToken);

                if (authKey != null)
                {
                    string profileID = jwt.decodeFacebookID(authKey.ConsumerAuthKey);
                    consumer = consumerDAL.GetConsumerByProfileID(profileID);

                    if (consumer != null)
                    {
                        consumer.IsValidKey = true;
                    }
                }
            }
            catch (Exception ex)
            {
                consumer = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return consumer;

        }

        public SimpleOperationModel AddConsumerNickname(ConsumerEN pConsumer, string pNickname)
        {
            SimpleOperationModel nicknameOp = new SimpleOperationModel();
            bool isForbidden = false;
            try
            {
                var forbiddenNickname = consumerDAL.GetForbiddenNickname(pNickname);

                isForbidden = ValidateIsForbidden(pNickname, 0, forbiddenNickname);

                isForbidden = (isForbidden == false) ? ValidateIsForbidden(pNickname, 1, forbiddenNickname) : true;

                if(isForbidden)
                {
                    nicknameOp.Result = false;
                    nicknameOp.Message = "forbidden";
                }
                else
                {
                    var nickname = consumerDAL.GetConsumerByNickname(pNickname);

                    if (nickname == null)
                    {
                        pConsumer.Nickname = pNickname;
                        if (consumerDAL.UpdateConsumer(pConsumer) > 0)
                        {
                            nicknameOp.Result = true;
                            nicknameOp.Message = "updated";
                        }
                        else
                        {
                            nicknameOp.Result = false;
                            nicknameOp.Message = "error";
                        }
                    }
                    else
                    {
                        nicknameOp.Result = false;
                        nicknameOp.Message = "conflict";
                    }

                }


            }
            catch (Exception ex)
            {
                nicknameOp = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return nicknameOp;
        }

        public bool ValidateIsForbidden(string pNickname, int ApplyReplace, List<ForbiddenNicknameEN> forbiddenNickname)
        {
            var regex = new Regex("(.)\\1+");
            bool isForbidden = false;

            pNickname = (ApplyReplace == 1) ? regex.Replace(pNickname, "$1") : pNickname;

            foreach(var item in forbiddenNickname)
            {

                if (pNickname.ToLower().Contains(item.Forbidden.Trim().ToLower()))
                {
                    isForbidden = true;
                    break;
                }
            }
            return isForbidden;
        }
    }
}
