using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class SalesHistoryInteractor
    {
        public IResponse createHistoryResultsResponse(List<SaleEN> pSalesList, string pToken)
        {
            TransactionsResponse response = new TransactionsResponse();
            HistoryResponse history = new HistoryResponse();

            history.transactions = new List<Sale>();

            foreach (var item in pSalesList)
            {
                Sale sale = new Sale();
                sale.Amount = item.Amount;
                sale.currency = item.Currency;
                sale.date = item.Date; //Date is actually server's date. Used by Android clients, as android sets displayed dates based on TimeZone set on device
                sale.FormattedAmount = item.FormattedAmount;
                sale.id = item.ID.ToString();
                sale.msisdn = item.Msisdn;
                sale.Operator = item.OperatorName;
                sale.paid = item.Paid;
                sale.Pais = item.CountryName;
                sale.salesman = item.PersonName;
                sale.serverDateGMT = item.ISOServerDate;
                sale.status = (item.Success) ? "Exitosa" : "Fallida";
                sale.transaction_id = item.TransactionID;
                history.transactions.Add(sale);
            }

            response.History = history;
            response.count = history.transactions.Count;
            response.token = pToken;

            return response;
        }

        public IResponse createHistoryResultsResponseGMT(List<SaleEN> pSalesList, string pToken)
        {
            TransactionsResponse response = new TransactionsResponse();
            HistoryResponse history = new HistoryResponse();

            history.transactions = new List<Sale>();

            foreach (var item in pSalesList)
            {
                Sale sale = new Sale();
                sale.Amount = item.Amount;
                sale.currency = item.Currency;
                sale.date = item.Date;      //Server date on 'date' property in response for iOS clients
                sale.FormattedAmount = item.FormattedAmount;
                sale.id = item.ID.ToString();
                sale.msisdn = item.Msisdn;
                sale.Operator = item.OperatorName;
                sale.paid = item.Paid;
                sale.Pais = item.CountryName;
                sale.salesman = item.PersonName;
                sale.serverDateGMT = item.ISOServerDate;
                sale.status = (item.Success) ? "Exitosa" : "Fallida";
                sale.transaction_id = item.TransactionID;
                history.transactions.Add(sale);
            }

            response.History = history;
            response.count = history.transactions.Count;
            response.token = pToken;

            return response;
        }

        public IResponse CreateBalanceRocketResponse(RocketBalanceEN RocketBalance)
        {
            RocketBalance response = new RocketBalance();


            response.balanceID = RocketBalance.BalanceID;
            response.balanceAmount = RocketBalance.BalanceAmount;
            response.fromDate = RocketBalance.FromDate;
            response.toDate = RocketBalance.ToDate;
            response.status = RocketBalance.Status;
            response.conciliationDate = RocketBalance.ConciliationDate;
            response.profit = RocketBalance.Profit;

            return response;
        }

        public IResponse createPaymentsHistoryRocket(List<RocketBalanceEN> pPaymentRocket, string pToken)
        {
            PaymentsRocketResponse response = new PaymentsRocketResponse(); //respuesta json
            PaymentsListResponse history = new PaymentsListResponse(); //objeto contiene lista

            history.RocketBalanceList = new List<RocketBalance>();

            foreach (var item in pPaymentRocket)
            {

                RocketBalance rocketPaymentList = new RocketBalance();
                rocketPaymentList.balanceID = item.BalanceID;
                rocketPaymentList.balanceAmount = item.BalanceAmount;
                rocketPaymentList.fromDate = item.FromDate;
                rocketPaymentList.toDate = item.ToDate;
                rocketPaymentList.status = item.Status;
                rocketPaymentList.conciliationDate = item.ConciliationDate;
                rocketPaymentList.profit = item.Profit;
                history.RocketBalanceList.Add(rocketPaymentList);
            }

            response.History = history;
            response.count = history.RocketBalanceList.Count;
            response.token = pToken;

            return response;
        }

        public IResponse createSaleDetailResponse(RocketSaleDetailEN pSaleDetail, string pToken)
        {
            //RocketSalesDetailResponse response = new RocketSalesDetailResponse(); //respuesta json
            //SaleDetailListResponse AllSales = new SaleDetailListResponse(); //objeto contiene lista


                RocketSaleDetail rocketSalesDetail = new RocketSaleDetail();
                rocketSalesDetail.balanceID = pSaleDetail.BalanceID;
                rocketSalesDetail.personID = pSaleDetail.PersonID;
                rocketSalesDetail.personMasterID = pSaleDetail.PersonMasterID;
                rocketSalesDetail.receivable = pSaleDetail.Receivable;
                rocketSalesDetail.reconcileCount = pSaleDetail.ReconcileCount;
                rocketSalesDetail.name = pSaleDetail.Name;
                rocketSalesDetail.bStatus = pSaleDetail.BStatus;
                rocketSalesDetail.zone = pSaleDetail.Zone;
                rocketSalesDetail.minDate = pSaleDetail.MinDate;
                rocketSalesDetail.maxDate = pSaleDetail.MaxDate;
                rocketSalesDetail.distributor = pSaleDetail.Distributor;
                rocketSalesDetail.sale = pSaleDetail.Sale;


            return rocketSalesDetail;
        }



    }
}