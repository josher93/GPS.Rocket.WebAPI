using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using System.Data;

namespace YoVendoRecargaAPI.DAL
{
    public class SaleDAL
    {
        Connection connection = new Connection();

        public List<SaleEN> GetPersonIntervalSalesHistory(int pPersonID, string pUtcTimeZone, int pDaysInterval)
        {
            List<SaleEN> saleHistory = new List<SaleEN>();

            try
            {
                connection.Cnn.Open();
                saleHistory = connection.Cnn.Query<SaleEN>("SpGetPersonSaleHistory", new { personID = pPersonID, utcTimeZone = pUtcTimeZone, days = pDaysInterval },
                    commandType: CommandType.StoredProcedure).AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error SaleDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return saleHistory;
        }

        public List<SaleEN> GetPersonYesterdaySalesHistory(int pPersonID, string pUtcTimeZone)
        {
            List<SaleEN> saleHistory = new List<SaleEN>();

            try
            {
                connection.Cnn.Open();
                saleHistory = connection.Cnn.Query<SaleEN>("SpGetYesterdayPersonSaleHistory", new { personID = pPersonID, utcTimeZone = pUtcTimeZone },
                    commandType: CommandType.StoredProcedure).AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error SaleDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError("GetPersonYesterdaySalesHistory: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return saleHistory;
        }

        public List<SaleEN> GetPersonTodaySalesHistory(int pPersonID, string pUtcTimeZone)
        {
            List<SaleEN> saleHistory = new List<SaleEN>();

            try
            {
                connection.Cnn.Open();
                saleHistory = connection.Cnn.Query<SaleEN>("SpGetTodayPersonSaleHistory", new { personID = pPersonID, utcTimeZone = pUtcTimeZone },
                    commandType: CommandType.StoredProcedure).AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error SaleDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return saleHistory;
        }

        public int UpdateSalePayment(bool pPaid, int pSaleID)
        {
            int updated = default(int);

            try
            {
                connection.Cnn.Open();
                updated = connection.Cnn.Execute("SpUpdateTransactionPayment", new { id = pSaleID, payment = pPaid },
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                updated = 0;
                Console.WriteLine(ex.Message);
                EventViewerLoggerDAL.LogError("UpdateSalePayment: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return updated;
        }

        //CLARO ROCKET
        public int UpdateRocketSalePayment(int BalanceID, int pPaid)
        {
            int updated = default(int);

            try
            {
                connection.Cnn.Open();
                updated = connection.Cnn.Execute("SpUpdateRocketTransactionPayment", new { id = BalanceID, status = pPaid }, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                updated = 0;
                Console.WriteLine(ex.Message);
                EventViewerLoggerDAL.LogError("SpUpdateRocketTransactionPayment: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return updated;
        }

        public string GetRocketDealerPin(int PersonID)
        {
            string DealerPin = "";

            try
            {
                connection.Cnn.Open();
               DealerPin = connection.Cnn.Query<string>("SpGetRocketDealerPin", new { personID = PersonID }, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error SaleDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError("SpGetRocketDealerPin: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return DealerPin;
        }

        #region

        public RocketBalanceEN GetBalanceRocket(int pPersonID)
        {
            RocketBalanceEN RocketBalance = new RocketBalanceEN();

            try
            {
                connection.Cnn.Open();
                RocketBalance = connection.Cnn.Query<RocketBalanceEN>("SpGetBalanceRocket", new { personID = pPersonID },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error SaleDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return RocketBalance;
        }

        public List<RocketBalanceEN> GetPaymentsHistoryRocket(int pPersonID)
        {
            List<RocketBalanceEN> RocketBalanceHistory = new List<RocketBalanceEN>();

            try
            {
                connection.Cnn.Open();
                RocketBalanceHistory = connection.Cnn.Query<RocketBalanceEN>("SpGetPaymentsHistory", new { personID = pPersonID },
                    commandType: CommandType.StoredProcedure).AsList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error SaleDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return RocketBalanceHistory;
        }

        public RocketSaleDetailEN GetSalesDetail(int pMasterID, int pPersonID)
        {
            RocketSaleDetailEN SalesDetail = new RocketSaleDetailEN();

            try
            {
                connection.Cnn.Open();
                SalesDetail = connection.Cnn.Query<RocketSaleDetailEN>("SpSalesDetail", new { Distributor = pMasterID, PDV = pPersonID },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error SaleDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return SalesDetail;
        }

        #endregion
    }
}
