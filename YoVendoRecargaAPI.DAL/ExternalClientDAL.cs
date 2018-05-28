using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.DAL
{
    public class ExternalClientDAL
    {
        Connection connection = new Connection();

        public ExternalClientEN AuthenticateClient(string pGuid, string pPassword, DateTime pRegistrationDate)
        {
            ExternalClientEN client = new ExternalClientEN();
            try
            {
                connection.Cnn.Open();
                client = connection.Cnn.Query<ExternalClientEN>("SpVerifyExternalClient", new { guid = pGuid, password = pPassword, date = pRegistrationDate.Date },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                client = null;
                Console.WriteLine("ExternalClientDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return client;
        }

        public int InsertExternalClient(ExternalClientEN pClient)
        {
            int clientID = default(int);

            try
            {
                connection.Cnn.Open();
                clientID = connection.Cnn.Insert(pClient) ?? default(int);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExternalClientDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return clientID;
        }

    }
}
