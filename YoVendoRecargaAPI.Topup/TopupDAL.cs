using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.Topup.Utils;

namespace YoVendoRecargaAPI.Topup
{
    public class TopupDAL
    {
        Connection connection = new Connection();

        #region Products
        public ProductEN GetAvailableTopupProduct(string pOperatorName, int pCountryID, decimal pAmount, int pPackCode)
        {
            ProductEN topupProduct = new ProductEN();
            try
            {
                connection.Cnn.Open();
                topupProduct = connection.Cnn.Query<ProductEN>("SpGetAvailableOperatorProduct",
                    new { @operatorName = pOperatorName, @countryID = pCountryID, @amount = pAmount, @packCode = pPackCode },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                topupProduct = null;
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("GetAvailableTopupProduct: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return topupProduct;
        }

        #endregion

        #region Bag

        public Decimal GetPersonAvailableAmount(int pPersonID, int pCountryID, string pOperatorName)
        {
            Decimal avaiable;

            try
            {
                connection.Cnn.Open();
                avaiable = connection.Cnn.Query<Decimal>("SpGetUserAvailableAmmount", new { @operatorName = pOperatorName, @countryID = pCountryID, @personID = pPersonID },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                avaiable = 0;
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("GetPersonAvailableAmount: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return avaiable;
        }

        public List<PersonBagOperatorEN> GetUserOperatorBags(int pPersonID)
        {
            List<PersonBagOperatorEN> personBags = new List<PersonBagOperatorEN>();

            try
            {
                connection.Cnn.Open();
                personBags = connection.Cnn.Query<PersonBagOperatorEN>("SpGetUserOperatorBags", new { @personID = pPersonID },
                    commandType: CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                personBags = null;
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("GetUserOperatorBag: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return personBags;
        }

        public int UpdateUserBag(Decimal Amount, int pPersonID, string pOperator, int Total, int Producto = 0)
        {

            int update = 0;

            try
            {
                connection.Cnn.Open();
                update = connection.Cnn.Execute("SpUpdateUserBagFull",
                    new { @Amount = Amount, @personID = pPersonID, @operators = pOperator, @totaloperator = Total, @product = Producto },
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                update = 0;
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("UpdateUserBag: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return update;
        }

        public int UpdateUserBagRefund(Decimal Amount, int pPersonID, string pOperator, int Total)
        {
            int update = 0;

            try
            {
                connection.Cnn.Open();
                update = connection.Cnn.Execute("SpUpdateUserBagFullRefund",
                    new { @Amount = Amount, @personID = pPersonID, @operators = pOperator, @totaloperator = Total },
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                update = 0;
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("UpdateUserBag: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return update;
        }

        public int InsertUserBagHistory(UserBagHistoryEN pBagHistory)
        {
            int userBagHistoryID = default(int);

            try
            {
                connection.Cnn.Open();
                userBagHistoryID = connection.Cnn.Insert(pBagHistory) ?? default(int);
            }
            catch (Exception ex)
            {
                userBagHistoryID = 0;
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError("InsertUserBagHistory: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return userBagHistoryID;
        }

        public int UpdateUserBagHistory(UserBagHistoryEN pBagHistory)
        {
            int updated = default(int);

            try
            {
                connection.Cnn.Open();
                updated = connection.Cnn.Update(pBagHistory);
            }
            catch (Exception ex)
            {
                updated = 0;
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError("UpdateUserBagHistory: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return updated;
        }

        #endregion

        #region Country

        public CountryEN GetCountryByID(int pCountryID)
        {
            CountryEN country = new CountryEN();

            try
            {
                connection.Cnn.Open();
                country = connection.Cnn.Query<CountryEN>(@" SELECT Name AS Name
                                                                FROM core.Country
                                                                WHERE Id = @countryID", new { countryID = pCountryID }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                country = null;
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("GetCountryByID: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return country;
        }

        #endregion

        #region Operator

        public OperatorEN GetOperatorByBrand(int pCountryID, string pOperatorBrand)
        {
            OperatorEN oprtr;

            try
            {
                connection.Cnn.Open();
                oprtr = connection.Cnn.Query<OperatorEN>("SpGetCountryByBrand", new { @countryID = pCountryID, @brand = pOperatorBrand },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                oprtr = null;
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("GetOperatorByBrand: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return oprtr;
        }

        #endregion

        #region Transactions

        /// <summary>
        /// Logs topup transaction data
        /// </summary>
        /// <param name="pTransaction">Transaction entity</param>
        /// <param name="pPersonID">Vendor ID</param>
        /// <param name="pCountryID">Country ID</param>
        /// <param name="pOperatorName">Operator name</param>
        /// <param name="pInventoryDisc">Inventory discount amount</param>
        /// <param name="pPersonDisc">Person discount amount</param>
        public void LogTopupTransaction(GatsTransactionEN pTransaction, int pPersonID, int pCountryID, string pOperatorName, decimal pInventoryDisc, decimal pPersonDisc)
        {
            TopupTransactionEN topup = new TopupTransactionEN();

            try
            {
                connection.Cnn.Open();
                Byte status = 0;
                //if (pTransaction.ResponseCode == "Success" || pTransaction.ResponseCode == "02")
                //{
                //    status = 1;
                //}
                //var resultInsert = connection.Cnn.Insert<Int64>(pTransaction);              

                //topup.AmountRequested = pTransaction.Amount.ToString();
                //topup.InventoryDiscount = pInventoryDisc;
                //topup.PersonDiscount = pPersonDisc;
                //topup.GATSTransactionID = resultInsert;
                //topup.PersonID = pPersonID.ToString();
                //topup.Status = status;
                //topup.RegDate = DateTime.Now;
                //topup.CountryID = pCountryID;
                //topup.Operator = pOperatorName;
                //connection.Cnn.Insert(topup);
                connection.Cnn.Query("SP_Inser_Logs", new { CountryID = pCountryID, PhoneNumber = pTransaction.PhoneNumber, Amount = pTransaction.Amount, Request = pTransaction.Request, Response = pTransaction.Response, ResponseCode = pTransaction.ResponseCode, TransactionID = pTransaction.TransactionID, ProviderTransactionID = pTransaction.ProviderTransactionID, personid = pPersonID, persondiscount = pPersonDisc, operatordiscount = pInventoryDisc, operador = pOperatorName }, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("LogTopupTransaction: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }
        }
        #endregion

        #region Requests

        public List<TopupRequestEN> GetTopupRequestsByVendorCode(int pVendorCode)
        {
            List<TopupRequestEN> requests = new List<TopupRequestEN>();

            try
            {
                connection.Cnn.Open();
                requests = connection.Cnn.Query<TopupRequestEN>("SpGetTopupRequests", new { @vendorID = pVendorCode },
                    commandType: CommandType.StoredProcedure).AsList();
            }
            catch (Exception ex)
            {
                requests = null;
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("GetTopupRequestsByVendorCode: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return requests;
        }

        public int InsertTopupRequest(TopupRequestEN pRequest)
        {
            int request = default(int);

            try
            {
                request = connection.Cnn.Insert(pRequest) ?? default(int);
            }
            catch (Exception ex)
            {
                request = 0;
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("InsertTopupRequest: " + ex.Message);
            }

            return request;

        }

        public int UpdateTopupRequest(TopupRequestEN pTopupRequest)
        {
            int update = default(int);

            try
            {
                update = connection.Cnn.Update(pTopupRequest);
            }
            catch (Exception ex)
            {
                update = 0;
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("UpdateTopupRequest: " + ex.Message);
            }

            return update;
        }

        public TopupRequestEN GetTopupRequestData(int pTopupRequestID)
        {
            TopupRequestEN request = new TopupRequestEN();

            try
            {
                request = connection.Cnn.Query<TopupRequestEN>("SpGetOperatorTopupRequest", new { topupRequestID = pTopupRequestID },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                request = null;
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("GetTopupRequestByID: " + ex.Message);
            }

            return request;
        }

        #endregion

        #region Persons

        public PersonEN GetPersonByVendorCode(int pVendorCode)
        {
            PersonEN person = new PersonEN();
            try
            {
                connection.Cnn.Open();
                person = connection.Cnn.Query<PersonEN>("SpGetPersonByVendorCode", new { vendorCode = pVendorCode },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                person = null;
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("GetPersonByVendorCode: " + ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return person;
        }


        #endregion
    }
}
