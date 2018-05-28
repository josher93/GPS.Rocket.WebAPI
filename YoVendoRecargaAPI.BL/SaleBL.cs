using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.DAL;

namespace YoVendoRecargaAPI.BL
{
    public class SaleBL
    {
        SaleDAL saleDAL = new SaleDAL();

        public List<SaleEN> GetIntervalPersonSaleHistory(PersonEN pPerson, string pInterval)
        {
            List<SaleEN> salesHistory = new List<SaleEN>();

            try
            {
                switch (pInterval)
                {
                    case "today":
                        salesHistory = saleDAL.GetPersonTodaySalesHistory(pPerson.PersonID, AssignTimeZone(pPerson.CountryID));
                        break;
                    case "yesterday":
                        salesHistory = saleDAL.GetPersonYesterdaySalesHistory(pPerson.PersonID, AssignTimeZone(pPerson.CountryID));
                        break;
                    case "week":
                        salesHistory = saleDAL.GetPersonIntervalSalesHistory(pPerson.PersonID, AssignTimeZone(pPerson.CountryID), Constants.Week);
                        break;
                    default:
                        salesHistory = saleDAL.GetPersonIntervalSalesHistory(pPerson.PersonID, AssignTimeZone(pPerson.CountryID), Constants.Week);
                        break;
                }
            }
            catch (Exception ex)
            {
                salesHistory = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return salesHistory;
        }

        /// <summary>
        /// Determina la zona horaria dependiendo del país. La mejor practica sería que se maneje en BD
        /// </summary>
        /// <param name="pCountryID">ID del país segun Base de Datos</param>
        /// <returns>ID de la zona horaria según Windows.</returns>
        public string AssignTimeZone(int pCountryID)
        {
            string timeZone; 

            switch (pCountryID)
            { 
                case Constants.ElSalvador:
                    timeZone = Constants.TimeZoneCentralAmerica;
                    break;
                case Constants.Panama:
                    timeZone = Constants.TimeZoneSAPacificStandardTime;
                    break;
                default:
                    timeZone = Constants.TimeZoneCentralAmerica;
                    break;
            }

            return timeZone;
        }

        public bool UpdateSalePayment(int pSaleID, bool pPaid)
        {
            bool updated = false;

            try
            {
                updated = (saleDAL.UpdateSalePayment(pPaid, pSaleID) > 0) ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("UpdateSalePayment: " + ex.Message);
            }

            return updated;
        }

        //Claro Rocket

        public bool UpdateRocketSalePayment(int BalanceID, int pPaid)
        {
            bool updated = false;

            try
            {
                updated = (saleDAL.UpdateRocketSalePayment(BalanceID, pPaid) > 0) ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("UpdateRocketSalePayment: " + ex.Message);
            }

            return updated;
        }



        public string GetRocketDealerPin(int PersonID)
        {
            string DealerPin = "";
            try
            {
                DealerPin= saleDAL.GetRocketDealerPin(PersonID);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("GetRocketDealerPin: " + ex.Message);
            }

            return DealerPin;
        }

        public RocketBalanceEN GetBalanceRocket(int pPersonID)
        {
            RocketBalanceEN balanceRocket = new RocketBalanceEN();

            try
            {
                balanceRocket = saleDAL.GetBalanceRocket(pPersonID);   
            }
            catch (Exception ex)
            {
                balanceRocket = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return balanceRocket;
        }

        public List<RocketBalanceEN> GetPaymentsHistoryRocket(int pPersonID)
        {
            List<RocketBalanceEN> PaymentsRocket = new List<RocketBalanceEN>();

            try
            {
                PaymentsRocket = saleDAL.GetPaymentsHistoryRocket(pPersonID);
            }
            catch (Exception ex)
            {
                PaymentsRocket = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return PaymentsRocket;
        }

        public RocketSaleDetailEN GetSaleDetail(int pMasterID, int pPersonID)
        {
            RocketSaleDetailEN councilCount = new RocketSaleDetailEN();

            try
            {
                councilCount = saleDAL.GetSalesDetail(pMasterID, pPersonID);
            }
            catch (Exception ex)
            {
                councilCount = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return councilCount;
        }
    }
}
