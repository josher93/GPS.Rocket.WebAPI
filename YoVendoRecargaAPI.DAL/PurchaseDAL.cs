using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using YoVendoRecargaAPI.Entities;
using System.Data;

namespace YoVendoRecargaAPI.DAL
{
    public class PurchaseDAL
    {
        Connection connection = new Connection();

        public bool InsertPurchase(int pPersonID, int pBankID, decimal pAmount, int pCountryID, string pDepositor, string pBankReference, DateTime pDepositDate)
        {
            bool result = false;

            try
            {
                connection.Cnn.Open();

                using (SqlTransaction transaction = connection.Cnn.BeginTransaction())
                {
                    var insertPurchase = connection.Cnn.Query("SpBankDeposit",
                        new
                        {
                            PersonID = pPersonID,
                            BankID = pBankID,
                            Amount = pAmount,
                            RegDate = DateTime.Now,
                            CountryID = pCountryID,
                            Name = pDepositor,
                            Number = pBankReference,
                            ImgReference = String.Empty,
                            DepositDate = pDepositDate
                        }, transaction, commandType: CommandType.StoredProcedure);
                    
                    transaction.Commit();
                    result = true;
                }

            }
            catch (Exception ex)
            {
                result = false;
                Console.WriteLine(ex.Message);
                EventViewerLoggerDAL.LogError("Error BankDepositDAL: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return result;
        }
    }
}