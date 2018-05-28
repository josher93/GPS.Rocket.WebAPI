using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.DAL
{
    public class CampaignDAL
    {
        Connection connection = new Connection();

        public int InsertCampaign(CampaignEN pCampaign)
        {
            int campaignID = default(int);

            try
            {
                connection.Cnn.Open();
                campaignID = connection.Cnn.Insert(pCampaign) ?? default(int);
            }
            catch (Exception ex)
            {
                campaignID = 0;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return campaignID;
        }

        public List<CampaignEN> GetNotificationsByUserID(int pPersonID)
        {
            List<CampaignEN> notifications = new List<CampaignEN>();

            try
            {
                connection.Cnn.Open();
                notifications = connection.Cnn.Query<CampaignEN>("SpGetNotificationsByUser", new { personID = pPersonID },
                    commandType: CommandType.StoredProcedure).AsList();

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

            return notifications;
        }

        public bool MarkNotificationAsRead(int pNotificationID)
        {
            bool read = false;
            int result = 0;

            try
            {
                connection.Cnn.Open();
                result = connection.Cnn.Execute("SpMarkNotificationAsRead", new { notifID = pNotificationID },
                    commandType: CommandType.StoredProcedure);

                read = (result > 0) ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("CampaignDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return read;
        }

        /// <summary>
        /// Assigns the campaign ID (notification content) to each valid user.
        /// </summary>
        /// <param name="pISO2Code">User country's ISO2 code</param>
        /// <param name="pCampaignID">Data base entry's ID of campaign</param>
        /// <returns></returns>
        public bool InsertUsersNotifications(string pISO2Code, int pCampaignID)
        {
            bool result = false;

            SqlConnection sqlConnection = connection.Cnn;
            int transactionResult = 0;

            sqlConnection.Open();

            using (var transaction = sqlConnection.BeginTransaction())
            {
                try
                {
                    transactionResult = connection.Cnn.Execute("SpInsertUsersNotifications",
                        new
                        {
                            iso2code = pISO2Code,
                            campaignID = pCampaignID
                        },
                        transaction,
                        commandType: CommandType.StoredProcedure);

                    transaction.Commit();
                    result = (transactionResult > 0) ? true : false;
                }
                catch (Exception ex)
                {
                    transactionResult = 0;
                    result = false;
                    transaction.Rollback();
                    Console.WriteLine(ex.InnerException);
                    EventViewerLoggerDAL.LogError(ex.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }

            return result;
        }


        public int InsertNotificationByUser(int pCampaignID, int pPersonID)
        {
            int result = 0;

            try
            {
                connection.Cnn.Open();
                result = connection.Cnn.Execute("SpInsertNotificationByUser", new { campaignID = pCampaignID, personID = pPersonID },
                    commandType: CommandType.StoredProcedure);
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

            return result;
        }

    }
}
