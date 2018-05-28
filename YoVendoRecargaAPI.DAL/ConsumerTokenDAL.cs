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
    public class ConsumerTokenDAL
    {
        Connection connection = new Connection();

        public string GenerateConsumerToken(int pConsumerID)
        {
            string consumerToken = "";

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("consumerid", pConsumerID);
                parameters.Add("token", dbType: DbType.String, direction: ParameterDirection.Output, size: 10);

                connection.Cnn.Query<String>("SP_Token_Phone_YCR", parameters, commandType: CommandType.StoredProcedure);
                consumerToken = parameters.Get<String>("token");
            }
            catch (Exception ex)
            {
                consumerToken = "";
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError("GenerateConsumerToken: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return consumerToken;
        }

        public ConsumerTokenEN GetConsumerSmsToken(int pConsumerID, string pToken, DateTime pDate)
        {
            ConsumerTokenEN consumerToken = null;

            try
            {
                consumerToken = connection.Cnn.GetList<ConsumerTokenEN>().Where(f => f.Token == pToken.ToUpper()
                    && f.ConsumerID == pConsumerID
                    && f.ExpirationDate >= pDate
                    && f.Status == false).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError("VerifyConsumerToken: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return consumerToken;
        }


        public ConsumerTokenEN GetLastConsumerSmsToken(int pConsumerID)
        {
            ConsumerTokenEN consumerToken = null;

            try
            {
                consumerToken = connection.Cnn.GetList<ConsumerTokenEN>().Where(f => f.ConsumerID == pConsumerID).LastOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError("GetLastConsumerToken: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return consumerToken;
        }

        public int UpdateToken(ConsumerTokenEN pNewToken)
        {
            int resultUpdate = default(int);

            try
            {
                resultUpdate = connection.Cnn.Update(pNewToken);
            }
            catch (Exception ex)
            {
                resultUpdate = 0;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerDAL.LogError("UpdateToken: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return resultUpdate;
        }

    }
}
