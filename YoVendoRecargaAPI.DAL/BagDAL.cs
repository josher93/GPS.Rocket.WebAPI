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
    public class BagDAL
    {
        public List<PersonBagOperatorEN> GetUserOperatorBag(int pPersonID)
        {
            Connection connection = new Connection();
            List<PersonBagOperatorEN> userOperatorBagList = new List<PersonBagOperatorEN>();

            try
            {
                userOperatorBagList = connection.Cnn.Query<PersonBagOperatorEN>("SpGetUserOperatorBags",
                                new { personID = pPersonID }, commandType: CommandType.StoredProcedure).AsList();
            }
            catch (Exception ex)
            {
                userOperatorBagList = null;
                Console.WriteLine(ex.Message);
                EventViewerLoggerDAL.LogError("GetUserOperatorBag: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return userOperatorBagList;
        }
    }
}
