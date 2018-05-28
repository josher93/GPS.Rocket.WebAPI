using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using YoVendoRecargaAPI.Entities;
using System.Data;

namespace YoVendoRecargaAPI.DAL
{
    public class SessionDAL
    {
        private Connection connection;

        public SessionDAL()
        {
            connection = new Connection();
        }

        /// <summary>
        /// Inserts new authentication entry to logs (dbo.SessionsLog) 
        /// </summary>
        /// <param name="pPersonID">Vendor ID</param>
        /// <param name="pDeviceIP">External device's IP Address</param>
        /// <param name="pDeviceInfo">Brand, model, OS version.</param>
        /// <param name="pDeviceID">Device identification number.</param>
        /// <returns>Returns inserted 'SessionsLog' row's ID.</returns>
        public int InsertAuthenticationLog(int pPersonID, string pDeviceIP, string pDeviceInfo, string pDeviceID, bool pMobileDevice, DateTime pDate)
        {
            int sessionID = default(int);

            Connection connection = new Connection();

            try
            {
                SessionEN session = new SessionEN();
                session.PersonID = pPersonID;
                session.RegDate = pDate;
                session.ActiveSession = true;
                session.DeviceInfo = pDeviceInfo;
                session.MobileDevice = pMobileDevice;
                session.DeviceIP = pDeviceIP;
                session.DeviceID = pDeviceID;

                connection.Cnn.Open();
                sessionID = connection.Cnn.Insert(session) ?? default(int);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return sessionID;
        }

        public SessionEN GetSessionByID(int pSessionID)
        {
            SessionEN session = new SessionEN();

            try
            {
                connection.Cnn.Open();
                session = connection.Cnn.GetList<SessionEN>().Where(s => s.SessionID == pSessionID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError("GetSessionByID: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return session;
        }

        public int UpdateSession(SessionEN pSession)
        {
            int updated = default(int);

            try
            {
                connection.Cnn.Open();
                updated = connection.Cnn.Update(pSession);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError("UpdateSession: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return updated;
        
        }

        public List<SessionEN> GetActiveUserSessions(int pPersonID, bool pIsMobile)
        {
            List<SessionEN> sessions = new List<SessionEN>();

            try
            {
                connection.Cnn.Open();
                sessions = connection.Cnn.Query<SessionEN>("SpGetActiveUserSessions", new { personID = pPersonID, isMobile = pIsMobile }, 
                    commandType: CommandType.StoredProcedure).AsList();
            }
            catch (Exception ex)
            {
                sessions = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError("GetActiveUserSessions: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return sessions;
        }

        public int ClosePenultimateSession(int pPersonID, bool pIsMobile)
        {
            int result = default(int);

            try
            {
                connection.Cnn.Open();
                result = connection.Cnn.Execute("SpClosePenultimateSession", new { personID = pPersonID, mobile = pIsMobile }, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                result = 0;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError("ClosePenultimateSession: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return result;
        }


        #region AuthenticationYCR

        public ConsumerAuthEN GetAuthKey(string Token)
        {

            ConsumerAuthEN AuthKey = new ConsumerAuthEN();
            

            try
            {
                connection.Cnn.Open();
                AuthKey = connection.Cnn.Query<ConsumerAuthEN>(@"SELECT TOP 1 ConsumerAuthID, ConsumerID, ConsumerAuthKey, RegDate
                                                              FROM Consumer.ConsumerAuth WHERE ConsumerAuthKey = @consumerAuthKey",
                   new
                   {
                       consumerAuthKey = Token,
                   }).FirstOrDefault();

                EventViewerLoggerDAL.LogError("GetAuthKey... ConsumerID: " + AuthKey.ConsumerID.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GetAuthKey: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);

                AuthKey = null;
            }
            finally
            {
                connection.Cnn.Close();
            }

            return AuthKey;


        }

        public FacebookConsumerEN GetFacebookConsumer(string decodedProfileId)
        {

            FacebookConsumerEN ConsumerDecoded = new FacebookConsumerEN();


            try
            {
                connection.Cnn.Open();


                ConsumerDecoded = connection.Cnn.Query<FacebookConsumerEN>(@"select ConsumerID, Phone, CountryID, DeviceID, URL, Email, ProfileID, UserID, Firstname, MiddleName, Lastname, Nickname from [Consumer].[Consumer] where ProfileID = @profileID",
                    new
                    {
                        profileID = decodedProfileId
                    }).FirstOrDefault();
                connection.Cnn.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error GetFacebookConsumer: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);

                ConsumerDecoded = null;
            }
            finally
            {
                connection.Cnn.Close();
            }

            return ConsumerDecoded;


        }

        #endregion
    }
}
