using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using Dapper;
using System.Data;

namespace YoVendoRecargaAPI.DAL
{
    public class OperatorDAL
    {
        private Connection cnn = new Connection();

        public List<OperatorEN> GetOperators(int pCountryID)
        {
            List<OperatorEN> operatorList = new List<OperatorEN>();
            try
            {

                operatorList = cnn.Cnn.Query<OperatorEN>("SpGetOperators",
                    new { countryID = pCountryID }, commandType: CommandType.StoredProcedure).AsList();

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error OperatorDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return operatorList;
        }

        public List<OperatorEN> GetOperatorsYCR(int pCountryID)
        {
            List<OperatorEN> operatorList = new List<OperatorEN>();
            try
            {

                operatorList = cnn.Cnn.Query<OperatorEN>("SpGetOperators",
                    new { countryID = pCountryID }, commandType: CommandType.StoredProcedure).AsList();


                operatorList = operatorList.Where(item => item.ConsumerAllowed == true).ToList();
                               
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error OperatorDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return operatorList;
        }
    }
}
