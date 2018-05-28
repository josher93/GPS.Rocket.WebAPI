using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.Topup.Models;
using YoVendoRecargaAPI.Topup.Utils;

namespace YoVendoRecargaAPI.Topup
{
    public class TopupBL
    {
        TopupDAL topupDAL = new TopupDAL();
        AzureSearchManager azureSearch = new AzureSearchManager();
        TopupClient topupClient = new TopupClient();
        CampaignBL campaignBL = new CampaignBL();
        GameBL gameBL = new GameBL();


        #region TopUp

        public TopupTransactionEN SendTopup(PersonEN pPerson, string pOperatorName, decimal pAmount, string pPhone, int pPackCode)
        {
            TopupTransactionEN topupResult = new TopupTransactionEN();
            TopupEN topup = new TopupEN();

            ProductEN product = new ProductEN();
            topup.Operator = pOperatorName;
            topup.Amount = pAmount;
            topup.Phone = pPhone;
            topup.PackageCode = pPackCode;

            try
            {
                var topupData = ValidateTopupData(pPerson, topup, ref product);

                if (topupData.IsValid)
                {
                    List<PersonBagOperatorEN> operatorUserBags = topupDAL.GetUserOperatorBags(pPerson.PersonID);

                    //Register initial bags state
                   // var states = RegisterInitialUserBagState(operatorUserBags, product.ProductID);

                    try
                    {
                        //Discounts product amount from user's bag
                        if (UpdateUserBags(product, operatorUserBags, pPerson.PersonID))
                        {
                            #region AzureSearch
                            //if (!azureSearch.CheckExistingEntry(topupData.Phone))
                            //    azureSearch.InsertEntry(pPerson.PersonID, pOperatorName, pPhone, pPerson.CountryID);
                            //else
                            //    azureSearch.ComparePhoneNumber(pPhone, pOperatorName, pPerson.PersonID, pPerson.CountryID);
                            #endregion

                            //Gets response from Topup Service
                            topupResult = topupClient.AttemptSendTopup(pPhone, pAmount, pPackCode, topupData.OperatorID, topupData.CategoryID);

                            GatsTransactionEN transaction = new GatsTransactionEN();
                            transaction.Request = topupResult.RequestURL;
                            transaction.Response = topupResult.Response;
                            transaction.TransactionID = topupResult.ServiceTransactionID;
                            transaction.ProviderTransactionID = topupResult.ServiceTransactionID;
                            transaction.Amount = pAmount;
                            transaction.CountryID = pPerson.CountryID;
                            transaction.PhoneNumber = long.Parse(pPhone.ToString());
                            transaction.RegDate = DateTime.Now;
                            transaction.ResponseCode = topupResult.Code;
                            transaction.Paid = true;

                            if (!String.Equals(topupResult.Code, "Success") && !String.Equals(topupResult.Code, "02"))
                            {
                                if (product.PersonDiscount > 0)
                                {
                                    //Refunds user balance from updated values
                                    var updatedBags = topupDAL.GetUserOperatorBags(pPerson.PersonID);
                                    UpdateUserBagsRefund(product, updatedBags, pPerson.PersonID);
                                }
                            }
                            else
                            {
                                topupResult.Message = topupData.Result;
                            }

                            //Updates users bags history
                            //RegisterFinalUserBagState(states, pPerson.PersonID);

                            topupDAL.LogTopupTransaction(transaction, pPerson.PersonID, pPerson.CountryID, topupData.Operator, product.InventoryDiscount, product.PersonDiscount);
                            EventViewerLogger.LogInformation("SendTopup: " + transaction.Response);
                        }
                    }
                    catch (Exception ex)
                    {
                        topupResult.Message = ex.Message;
                        topupResult.Code = "Error";

                        if (product.PersonDiscount > 0)
                        {
                            //Gets user bags with updated values
                            operatorUserBags = topupDAL.GetUserOperatorBags(pPerson.PersonID);

                            //Refunds user bag if any exception ocurred
                            UpdateUserBagsRefund(product, operatorUserBags, pPerson.PersonID);
                            EventViewerLogger.LogError(String.Format("SendTopup - Inner exception: Refund user bags. PersonID {0}. Amount {1}. Message: {2}",
                                Convert.ToString(pPerson.PersonID), product.PersonDiscount, ex.Message));

                            //Updates users bags history
                            //RegisterFinalUserBagState(states, pPerson.PersonID);
                        }
                    }
                }
                else
                {
                    //Sets the results according to topup data validation
                    topupResult.Message = topupData.Result;
                    topupResult.Code = topupData.Result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError("SendTopup: " + ex.Message);
            }

            return topupResult;
        }

        /// <summary>
        /// Validates topup requested data is correct. Checks the product selected is available for the mobile operator. 
        /// Validates amount requested over user's bags
        /// </summary>
        /// <param name="pPerson">Vendor</param>
        /// <param name="pTopupData">Topup info (phone, amount, operator, etc)</param>
        /// <returns>Topup data result</returns>
        private TopupEN ValidateTopupData(PersonEN pPerson, TopupEN pTopupData, ref ProductEN oProduct)
        {
            ProductEN productAvailable = new ProductEN();
            TopupEN validTopup = new TopupEN();
            validTopup.IsValid = false;
            validTopup.PersonID = pPerson.PersonID;

            try
            {
                //Checks available topup product
                productAvailable = topupDAL.GetAvailableTopupProduct(pTopupData.Operator, pPerson.CountryID, pTopupData.Amount, pTopupData.PackageCode);

                if (productAvailable != null)
                {
                    //Product selected for transaction
                    oProduct = productAvailable;

                    //Validates if user balance is required for transaction
                    if (productAvailable.PersonDiscount > 0)
                    {
                        //Checks user's bags balance
                        Decimal balance = topupDAL.GetPersonAvailableAmount(pPerson.PersonID, pPerson.CountryID, pTopupData.Operator);
                        if (balance > 0)
                        {
                            if (balance >= productAvailable.PersonDiscount)
                            {
                                validTopup.Operator = productAvailable.OperatorName;
                                validTopup.Product = productAvailable;
                                validTopup.IsValid = true;
                                validTopup.Result = Values.Ok;
                                validTopup.ResultCode = Values.OkCode;
                                validTopup.OperatorID = productAvailable.OperatorID;
                                validTopup.CategoryID = productAvailable.CategoryID;
                            }
                            else
                            {
                                validTopup.Operator = productAvailable.OperatorName;
                                validTopup.IsValid = false;
                                validTopup.Result = Values.NoCreditLeft;
                                validTopup.ResultCode = Values.NoCreditLeftCode;
                            }
                        }
                        else
                        {
                            validTopup.Operator = productAvailable.OperatorName;
                            validTopup.IsValid = false;
                            validTopup.Result = Values.NoCreditLeft;
                            validTopup.ResultCode = Values.NoCreditLeftCode;
                        }
                    }
                    else
                    {
                        //Balance on user bag is not required
                        validTopup.Operator = productAvailable.OperatorName;
                        validTopup.IsValid = true;
                        validTopup.Result = Values.Ok;
                        validTopup.ResultCode = Values.PackageTopupCode;
                    }
                }
                else
                {
                    validTopup.Operator = pTopupData.Operator;
                    validTopup.IsValid = false;
                    validTopup.Result = Values.InvalidProduct;
                    validTopup.ResultCode = Values.InvalidProductCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError("ValidateTopupData: " + ex.Message);
            }

            return validTopup;
        }

        #endregion


        #region UserBags

        /// <summary>
        /// Gets vendor bags. Used to create an Http response to SendTopup operation
        /// </summary>
        /// <param name="pPersonID">PersonID: Vendor ID as in database</param>
        /// <returns></returns>
        public List<PersonBagOperatorEN> GetUserBags(int pPersonID)
        {
            List<PersonBagOperatorEN> operators = new List<PersonBagOperatorEN>();
            try
            {
                operators = topupDAL.GetUserOperatorBags(pPersonID);
                return operators;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetUserBags: " + ex.InnerException);
            }

            return operators;
        }


        //Private method
        /// <summary>
        /// Updates vendor bags according to Topup transaction amount.
        /// </summary>
        /// <param name="pProduct">Product data selected for operation</param>
        /// <param name="pPersonBags">All vendor bags used to discount the transaction amount from bags</param>
        /// <param name="pPersonID">Vendor ID as in database</param>
        /// <param name="pIsRefund">Toggles discount operation (adds if failed or deducts if succeed)</param>
        /// <returns></returns>
        private bool UpdateUserBags(ProductEN pProduct, List<PersonBagOperatorEN> pPersonBags, int pPersonID)
        {
            bool finished = false;
            int counter = 0;
            var Operator = "";
            try
            {
                var empty = pPersonBags.SingleOrDefault(e => e.MobileOperatorID <= 0);
                pPersonBags.Remove(empty);

                counter = pPersonBags.Count;
                var contador = 0;
                foreach (var item in pPersonBags)
                {
                    if (contador == 0)
                    {
                        Operator = item.MobileOperatorID.ToString();
                    }
                    else
                    {
                        Operator = Operator + "," + item.MobileOperatorID.ToString();
                    }
                    contador++;

                    #region Logs
                    EventViewerLogger.LogInformation(String.Format("UpdateUserBags: Current User Bag: {0}, Operator: {1}, PersonID: {2}", item.UserBalance, item.MobileOperatorName, Convert.ToString(pPersonID)));
                    #endregion
                }


                // Decimal newAmount = (!pIsRefund) ? item.UserBalanceAmount - pProduct.PersonDiscount : item.UserBalanceAmount + pProduct.PersonDiscount;
                int result = topupDAL.UpdateUserBag(pProduct.PersonDiscount, pPersonID, Operator, counter,pProduct.ProductID);

                if (result > 1)
                {
                    finished = true;
                }
                else
                {
                    finished = false;
                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError("UpdateUserBags: " + ex.Message);
            }

            return finished;
        }

        private bool UpdateUserBagsRefund(ProductEN pProduct, List<PersonBagOperatorEN> pPersonBags, int pPersonID)
        {
            bool finished = false;
            int counter = 0;
            var Operator = "";
            try
            {
                var empty = pPersonBags.SingleOrDefault(e => e.MobileOperatorID <= 0);
                pPersonBags.Remove(empty);

                counter = pPersonBags.Count;
                var contador = 0;
                foreach (var item in pPersonBags)
                {
                    if (contador == 0)
                    {
                        Operator = item.MobileOperatorID.ToString();
                    }
                    else
                    {
                        Operator = Operator + "," + item.MobileOperatorID.ToString();
                    }
                    contador++;

                    #region Logs
                    EventViewerLogger.LogInformation(String.Format("UpdateUserBags: Current User Bag: {0}, Operator: {1}, PersonID: {2}", item.UserBalance, item.MobileOperatorName, Convert.ToString(pPersonID)));
                    #endregion
                }


                // Decimal newAmount = (!pIsRefund) ? item.UserBalanceAmount - pProduct.PersonDiscount : item.UserBalanceAmount + pProduct.PersonDiscount;
                int result = topupDAL.UpdateUserBagRefund(pProduct.PersonDiscount, pPersonID, Operator, counter);

                if (result > 1)
                {
                    finished = true;
                }
                else
                {
                    finished = false;
                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError("UpdateUserBags: " + ex.Message);
            }

            return finished;
        }

        private List<UserBagHistoryEN> RegisterInitialUserBagState(List<PersonBagOperatorEN> pUserBags, int pProductID)
        {
            List<UserBagHistoryEN> userbagStates = new List<UserBagHistoryEN>();

            try
            {
                foreach (var bag in pUserBags)
                {
                    UserBagHistoryEN userbagState = new UserBagHistoryEN();
                    userbagState.BagID = bag.BagID;
                    userbagState.ProductID = pProductID;
                    userbagState.InitialBagValue = bag.UserBalanceAmount;
                    userbagState.InsertedAt = DateTime.Now;
                    userbagState.UpdatedAt = DateTime.Now;

                    int statusID = topupDAL.InsertUserBagHistory(userbagState);

                    if (statusID > 0)
                    {
                        userbagState.ID = statusID;
                        userbagStates.Add(userbagState);
                    }
                }
            }
            catch (Exception ex)
            {
                userbagStates = null;
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("RegisterInitialUserBagState: " + ex.Message);
            }

            return userbagStates;
        }

        private void RegisterFinalUserBagState(List<UserBagHistoryEN> pInsertedStates, int pPersonID)
        {
            try
            {
                var actualUserBags = topupDAL.GetUserOperatorBags(pPersonID);

                foreach (var state in pInsertedStates)
                {
                    state.FinalBagValue = actualUserBags.Where(b => b.BagID == state.BagID).FirstOrDefault().UserBalanceAmount;
                    state.UpdatedAt = DateTime.Now;

                    //Updates history item with new Balance
                    topupDAL.UpdateUserBagHistory(state);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                EventViewerLogger.LogError("RegisterFinalUserBagState: " + ex.Message);
            }
        }

        #endregion


        #region TopupRequest

        /// <summary>
        /// Returns all topup requests made to a specific vendor (by vendor code) 
        /// </summary>
        /// <param name="pVendorCode">Vendor code as in database</param>
        /// <returns>List: All topup requests</returns>
        public List<TopupRequestEN> GetTopupRequestsByVendor(int pVendorCode)
        {
            List<TopupRequestEN> requests = new List<TopupRequestEN>();

            try
            {
                requests = topupDAL.GetTopupRequestsByVendorCode(pVendorCode);
            }
            catch (Exception ex)
            {
                requests = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError(ex.Message);
            }

            return requests;
        }

        /// <summary>
        /// Inserts topup request into TopupRequests of a specific vendor and pushes notification to vendor
        /// </summary>
        /// <param name="pConsumerID">YCR user (consumer) ID as in database</param>
        /// <param name="pTargetPhone">Phone number where topup must be made</param>
        /// <param name="pVendorCode">Vendor code as in database</param>
        /// <param name="pAmount">Topup transaction amount</param>
        /// <param name="pOperatorID">Operator ID as in database</param>
        /// <returns>Boolean: Success or not</returns>
        public async Task<bool> RequestTopup(int pConsumerID, string pNickname, string pTargetPhone, int pVendorCode, decimal pAmount, int pOperatorID, int pCategoryID)
        {
            bool requested = false;

            try
            {
                PersonEN vendor = topupDAL.GetPersonByVendorCode(pVendorCode);

                if (vendor != null)
                {
                    //Completes topup request data
                    TopupRequestEN request = new TopupRequestEN();
                    request.ConsumerID = pConsumerID;
                    request.ConsumerNickname = pNickname;
                    request.TargetPhone = pTargetPhone;
                    request.VendorCode = vendor.VendorCode;
                    request.OperatorID = pOperatorID;
                    request.Amount = pAmount;
                    request.StatusCode = 0;
                    request.RequestDate = DateTime.Now;
                    request.ModDate = DateTime.Now;
                    request.CategoryID = pCategoryID;

                    //Inserts topup request
                    int insertedRequest = topupDAL.InsertTopupRequest(request);

                    if (insertedRequest > 0)
                    {
                        string phone = pTargetPhone.Insert(7, "-");
                        phone = phone.Substring(3);
                        string nickname = pNickname;

                        string amount = pAmount.ToString("C", new CultureInfo("en-US"));

                        //Sends notification to Vendor
                        var notificationResults = await campaignBL.SendTopupRequest(vendor.Email, nickname, phone, amount, vendor.Firstname);

                        requested = notificationResults.Where(n => n.Result).FirstOrDefault().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError("RequestTopup: " + ex.Message);
            }

            return requested;
        }

        /// <summary>
        /// Answers to a specific topup request by a consumer. If accepted, sends a topup and adds a coin to consumer's game progress.
        /// </summary>
        /// <param name="pPerson">Vendor data who makes the topup</param>
        /// <param name="pRequestID">Topup request ID as in database</param>
        /// <param name="pAccept">Answer: true if 'accepted' and false if 'denied'</param>
        /// <returns>Boolean: True if succeed and false if something went wrong</returns>
        public bool AnswerTopupRequest(PersonEN pPerson, int pRequestID, bool pAccept)
        {
            bool result = false;

            try
            {
                //Gets Topup request data
                var requestData = topupDAL.GetTopupRequestData(pRequestID);
                requestData.ModDate = DateTime.Now;

                if (pAccept)
                {
                    requestData.StatusCode = 1;

                    //Updates request to 'Status = 1' (accepted)
                    if (topupDAL.UpdateTopupRequest(requestData) > 0)
                    {
                        //Validates and fires topup
                        var topupTransaction = SendTopup(pPerson, requestData.OperatorName, requestData.Amount, requestData.TargetPhone, requestData.PackageCode);

                        if (topupTransaction != null)
                        {
                            if (String.Equals(topupTransaction.Message, Values.Ok) && requestData.OperatorName == "Claro")
                            {

                                bool AddCoinByTopUp = gameBL.AddCoinByTopup(requestData.ConsumerID);

                                if (AddCoinByTopUp == true)
                                {
                                    var content = CreateEarnedCoinContent();
                                    EventViewerLogger.LogInformation("AnswerTopupRequest: Consumer earned coin by accepted Topup request");

                                    if (content != null)
                                    {
                                        string title = (content.ContainsKey("title")) ? content["title"] : "";
                                        string message = (content.ContainsKey("message")) ? content["message"] : "";
                                        string pImageURL = "";

                                        //Send Notification
                                        var SendNotification = campaignBL.SendEarnedCoinNotification(title, message, pImageURL, requestData.ConsumerPhone);

                                        if (SendNotification != null)
                                        {
                                            result = SendNotification.Where(n => n.Result).FirstOrDefault().Result;
                                        }
                                    }
                                }
                                else
                                {
                                    EventViewerLogger.LogError("AnswerTopupRequest: Coin earned was not added to player tracking");
                                }


                                result = true;
                            }
                        }
                    }
                }
                else
                {
                    requestData.StatusCode = 2;

                    //Updates request to 'Status = 2' (denied)
                    if (topupDAL.UpdateTopupRequest(requestData) > 0)
                    {
                        //Creates notification content
                        var content = CreateTopupDeniedContent(requestData);

                        if (content != null)
                        {
                            string title = (content.ContainsKey("title")) ? content["title"] : "";
                            string message = (content.ContainsKey("message")) ? content["message"] : "";
                            string pImageURL = "";

                            //Fires notificiation
                            var notifications = campaignBL.SendTopupDeniedNotification(title, message, pImageURL, requestData.ConsumerPhone);

                            if (notifications != null)
                            {
                                result = notifications.Where(n => n.Result).FirstOrDefault().Result;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError("AnswerTopupRequest: " + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Generates 'Denied Topup Request' notification content to be pushed to consumer
        /// </summary>
        /// <param name="pRequestData">Topup request data</param>
        /// <returns>Dictionary with notification content</returns>
        private Dictionary<String, String> CreateTopupDeniedContent(TopupRequestEN pRequestData)
        {
            Dictionary<string, string> notification = new Dictionary<string, string>();

            try
            {
                string amount = Convert.ToString(pRequestData.Amount);
                string vendorCode = Convert.ToString(pRequestData.VendorCode);

                string Title = "¡Recarga denegada!";
                string Message = String.Format(@"¡Hola! Lamentamos comunicarte que tu recarga de ${0} de {1} ha sido denegada por el vendedor con código {2}. ¡No dejes de solicitar recargas con YoComproRecarga!",
                                                amount,
                                                pRequestData.OperatorName.ToUpper(),
                                                vendorCode);

                notification.Add("title", Title);
                notification.Add("message", Message);
            }
            catch (Exception ex)
            {
                notification = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError("CreateTopupDeniedContent: " + ex.Message);
            }

            return notification;
        }

        private Dictionary<String, String> CreateEarnedCoinContent()
        {
            Dictionary<string, string> notification = new Dictionary<string, string>();

            try
            {
                string Title = "¡Has ganado una moneda!";
                string Message = "Gracias a tu recarga solicitada a través de RecarGO! has ganado una moneda. ¡Sigue adelante en la aventura!";

                notification.Add("title", Title);
                notification.Add("message", Message);
            }
            catch (Exception ex)
            {
                notification = null;
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError("CreateEarnedCoinContent: " + ex.Message);
            }

            return notification;
        }

        #endregion




    }
}