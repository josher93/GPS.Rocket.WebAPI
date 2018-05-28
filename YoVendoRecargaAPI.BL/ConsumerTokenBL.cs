using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.DAL;
using YoVendoRecargaAPI.SVC;
using YoVendoRecargaAPI.Entities;
using System.Configuration;

namespace YoVendoRecargaAPI.BL
{
    public class ConsumerTokenBL
    {
        ConsumerTokenDAL tokenDAL = new ConsumerTokenDAL();
        ConsumerDAL consumerDAL = new ConsumerDAL();
        TwilioApiClient twilioClient = new TwilioApiClient();
        
        public TokenResultEN RegisterPhoneConsumer(ConsumerEN pConsumer, string pPhone, string pDeviceID, int pCountryID)
        {
            TokenResultEN tokenResult = new TokenResultEN();

            tokenResult.registeredAndSent = false;

            try
            {
                pConsumer.Phone = pPhone;
                pConsumer.CountryID = pCountryID;
                pConsumer.DeviceID = pDeviceID;
                pConsumer.Active = false;
                pConsumer.ModificationDate = DateTime.Now;

                if (consumerDAL.UpdateConsumer(pConsumer) > 0)
                {
                   

                    var consumer = consumerDAL.GetConsumerByID(pConsumer.ConsumerID);
                    var lastToken = tokenDAL.GetLastConsumerSmsToken(pConsumer.ConsumerID);
                    //This condition validates if a consumer has already registered his phone number
                    if (consumer.Phone == null)
                    {

                        string token = tokenDAL.GenerateConsumerToken(pConsumer.ConsumerID);

                        if (!String.IsNullOrEmpty(token))
                        {
                            string targetPhone = pPhone;
                            string messageTemplate = ConfigurationManager.AppSettings["SmsValidationCode"].ToString();

                            string message = String.Concat(messageTemplate, token);
                            string proveedor = ConfigurationManager.AppSettings["proveedorsms"].ToString();
                         
                            
                        }
                    }
                    else
                    {
                        if (lastToken != null)
                        {
                            DateTime AfterMinute = lastToken.CreationDate.AddMinutes(1.5);
                            if (pConsumer.ModificationDate >= AfterMinute)
                            {
                                string token = tokenDAL.GenerateConsumerToken(pConsumer.ConsumerID);

                                if (!String.IsNullOrEmpty(token))
                                {
                                    string targetPhone =  pPhone;
                                    string messageTemplate = ConfigurationManager.AppSettings["SmsValidationCode"].ToString();

                                    string message = String.Concat(messageTemplate, token);
                                    string proveedor = ConfigurationManager.AppSettings["proveedorsms"].ToString();
                                   
                                   
                                }
                                else
                                {
                                    tokenResult.registeredAndSent = false;
                                }
                            }
                            else
                            {
                                var TimeRemaining = AfterMinute - DateTime.Now;

                                double SecondsRemaining = TimeRemaining.TotalSeconds;
                                SecondsRemaining = Math.Round(SecondsRemaining);

                                tokenResult.TimeRemaining = "Espera " + SecondsRemaining + " segundos para enviar otro mensaje.";
                                tokenResult.registeredAndSent = false;

                                return tokenResult;
                            }
                        }
                        else
                        {
                            string token = tokenDAL.GenerateConsumerToken(pConsumer.ConsumerID);

                            if (!String.IsNullOrEmpty(token))
                            {
                                string targetPhone = "+" + pPhone;
                                string messageTemplate = ConfigurationManager.AppSettings["SmsValidationCode"].ToString();

                                string message = String.Concat(messageTemplate, token);
                                
                            }
                            else
                            {
                                tokenResult.registeredAndSent = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                tokenResult.registeredAndSent = false;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("RegisterPhoneConsumer: " + ex.Message);
            }

            return tokenResult;
        }

        public bool VerifyConsumerToken(string pToken, int pConsumerID)
        {
            bool verified = false;
            try
            {
                string masterToken = ConfigurationManager.AppSettings["yocomprorecarga_master_token"].ToString();

                var consumer = consumerDAL.GetConsumerByID(pConsumerID);

                if (consumer != null)
                {
                    if (!String.Equals(pToken, masterToken))
                    {
                        var token = tokenDAL.GetConsumerSmsToken(pConsumerID, pToken, DateTime.Now);

                        if (token != null)
                        {
                            token.Status = true;
                            int updateToken = tokenDAL.UpdateToken(token);

                            if (updateToken > 0)
                            {
                                consumer.Active = true;
                                int updateConsumer = consumerDAL.UpdateConsumer(consumer);

                                verified = (updateConsumer > 0) ? true : false;
                            }
                        }
                    }
                    else
                    {
                        consumer.Active = true;
                        int updateConsumer = consumerDAL.UpdateConsumer(consumer);

                        verified = (updateConsumer > 0) ? true : false;
                    }
                }
            }
            catch (Exception ex)
            {
                verified = false;
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("VerifyConsumerToken: " + ex.Message);
            }

            return verified;
        }

    }
}
