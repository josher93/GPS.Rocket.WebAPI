using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace YoVendoRecargaAPI.DAL
{
    public class ConsumerDAL
    {

        private Connection cnn = new Connection();

        private Connection con { get; set; }
        private SqlConnection connection { get; set; }

        public ConsumerDAL()
        {
            this.con = new Connection();
        }

        public ConsumerEN InsertConsumer(string pPhone, int pCountryID, string pDeviceID, string pURL, string pEmail, string pProfileID, string pUserID, string pFirstName, string pMiddleName, string pLastName, string pNickName)
        {

            ConsumerEN registerConsumer = new ConsumerEN();

            try
            {
                cnn.Cnn.Open();

                int? resultInsert = default(int);

                registerConsumer.Phone = pPhone;
                registerConsumer.CountryID = pCountryID;
                registerConsumer.Active = false;
                registerConsumer.DeviceID = pDeviceID;
                registerConsumer.RegistrationDate = DateTime.Now;
                registerConsumer.ModificationDate = DateTime.Now;
                registerConsumer.URL = pURL;
                registerConsumer.Email = pEmail;
                registerConsumer.ProfileID = pProfileID;
                registerConsumer.UserID = pUserID;
                registerConsumer.FirstName = pFirstName;
                registerConsumer.MiddleName = pMiddleName;
                registerConsumer.LastName = pLastName;
                registerConsumer.Nickname = pNickName;

                //Insertando en base de datos, si resultado exitoso, resultInsert = 1
                resultInsert = cnn.Cnn.Insert(registerConsumer);

            }
            catch (Exception ex)
            {
                Console.WriteLine("RegisterConsumerDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return registerConsumer;

        }

        public ConsumerAuthKeyEN InsertConsumerAuth(ConsumerAuthKeyEN consumerAuth)
        {
            try
            {
                cnn.Cnn.Open();

                ConsumerAuthKeyEN insertConsumerAuth = new ConsumerAuthKeyEN();

                insertConsumerAuth.ConsumerAuthID = consumerAuth.ConsumerAuthID;
                insertConsumerAuth.ConsumerID = consumerAuth.ConsumerID;
                insertConsumerAuth.ConsumerAuthKey = consumerAuth.ConsumerAuthKey;
                insertConsumerAuth.RegDate = consumerAuth.RegDate;

                int? authKey = cnn.Cnn.Insert(insertConsumerAuth);

            }
            catch (Exception ex)
            {
                Console.WriteLine("RegisterConsumerDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return consumerAuth;
        }

        public ConsumerEN GetConsumerByProfileID(string pProfileID)
        {
            ConsumerEN getConsumer = new ConsumerEN();

            try
            {
                getConsumer = cnn.Cnn.Query<ConsumerEN>("SpGetConsumer",
                    new { ProfileID = pProfileID }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error RegisterConsumerDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return getConsumer;
        }

        public AuthConsumerEN GetAuthConsumer(int pConsumerID)
        {
            AuthConsumerEN getAuthConsumer = new AuthConsumerEN();

            try
            {
                getAuthConsumer = cnn.Cnn.Query<AuthConsumerEN>("SpGetConsumerToken",
                    new { ConsumerID = pConsumerID }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error RegisterConsumerDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return getAuthConsumer;
        }

        public ConsumerEN GetProfileIDByProfileID(string pProfileID)
        {
            ConsumerEN profileFinded = new ConsumerEN();

            try
            {
                profileFinded = cnn.Cnn.Query<ConsumerEN>("SpGetProfileIDbyProfileID",
                    new { ProfileID = pProfileID }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error RegisterConsumerDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return profileFinded;
        }

        public ConsumerEN UpdateConsumerProfile(string pPhone, int pCountryID, string pDeviceID, string pURL, string pEmail, string pProfileID, string pUserID, string pFirstName, string pMiddleName, string pLastName)
        {
            ConsumerEN updateThisConsumer = new ConsumerEN();

            try
            {
                updateThisConsumer = cnn.Cnn.Query<ConsumerEN>("SpUpdateConsumer",
                    new
                    {
                        Phone = pPhone,
                        CountryID = pCountryID,
                        DeviceID = pDeviceID,
                        URL = pURL,
                        Email = pEmail,
                        ProfileID = pProfileID,
                        UserID = pUserID,
                        Firstname = pFirstName,
                        MiddleName = pMiddleName,
                        Lastname = pLastName
                    }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                updateThisConsumer = null;
                Console.WriteLine("Error RegisterConsumerDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return updateThisConsumer;
        }

        public AuthConsumerEN GetConsumerAuthByToken(string pToken)
        {
            AuthConsumerEN authData = null;

            try
            {
                cnn.Cnn.Open();
                authData = cnn.Cnn.GetList<AuthConsumerEN>().Where(au => au.ConsumerAuthKey == pToken).FirstOrDefault();

            }
            catch (Exception ex)
            {
                authData = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError("GetConsumerByAuthToken: " + ex.Message);
            }

            return authData;
        }

        public int UpdateConsumer(ConsumerEN pConsumer)
        {
            int result = default(int);

            try
            {
                cnn.Cnn.Open();
                pConsumer.ModificationDate = DateTime.Now;
                result = cnn.Cnn.Update(pConsumer);
            }
            catch (Exception ex)
            {
                result = 0;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError("UpdateConsumerPhone: " + ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }
            return result;
        }

        public ConsumerEN GetConsumerByID(int pConsumerID)
        {
            ConsumerEN consumer = new ConsumerEN();

            try
            {
                cnn.Cnn.Open();
                consumer = cnn.Cnn.GetList<ConsumerEN>().Where(c => c.ConsumerID == pConsumerID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                consumer = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError("GetConsumerByID: " + ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return consumer;
        }

        public ConsumerEN GetConsumerByNickname(string pNickname)
        {
            ConsumerEN consumer = new ConsumerEN();

            try
            {
                consumer = cnn.Cnn.Query<ConsumerEN>("SpGetConsumerByNickname", new { nickname  = pNickname.Trim() }, 
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                consumer = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError("GetConsumerByID: " + ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return consumer;
        }

        public List<ForbiddenNicknameEN> GetForbiddenNickname(string pForbidden)
        {
            List<ForbiddenNicknameEN> forbidden = new List<ForbiddenNicknameEN>();

            pForbidden = pForbidden.Replace(@"-", string.Empty);
            pForbidden = pForbidden.Replace(@"_", string.Empty);
            pForbidden = pForbidden.Replace(@".", string.Empty);
            pForbidden = pForbidden.Trim();

            try
            {
                forbidden = cnn.Cnn.Query<ForbiddenNicknameEN>("SpGetForbiddenNickname", new { Nickname = pForbidden.Trim() },
                     commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                forbidden = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError("GetForbiddenNickname: " + ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return forbidden;
        }
        
    }
}
