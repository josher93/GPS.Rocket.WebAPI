using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using Dapper;

namespace YoVendoRecargaAPI.DAL
{
    public class BalanceRequestDAL
    {
        private Connection cnn = new Connection();

        public PersonEN RequestBalance(string VendorEmail)
        {
            PersonEN person = new PersonEN();

            try
            {
                cnn.Cnn.Open();
                person = cnn.Cnn.Query<PersonEN>("SpRequestBalance", new { vendorEmail = VendorEmail },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error BalanceRequestDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
                person = null;
            }
            finally
            {
                cnn.Cnn.Close();
            }

            return person;
        }
    }
}
