using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.Topup.Models;

namespace YoVendoRecargaAPI.Topup.Utils
{
    public class AzureSearchManager
    {
        TopupDAL topupDAL = new TopupDAL();

        public void InsertEntry(int pPersonID, string pOperatorName, string pPhone, int pCountryID)
        {
            try
            {
                CountryEN personCountry = topupDAL.GetCountryByID(pCountryID);
                string operatorBrand = pOperatorName + " " + personCountry.Name;

                OperatorEN operatorFound = topupDAL.GetOperatorByBrand(pCountryID, operatorBrand);

                if (operatorFound != null)
                {
                    RangeAzureSearch item = new RangeAzureSearch();
                    item.country_code = (operatorFound.ISO2Code != String.Empty) ? operatorFound.ISO2Code : "";
                    item.mnc = (operatorFound.Mnc != String.Empty) ? operatorFound.Mnc : "";
                    item.mcc = "706";
                    item.mno_id = Convert.ToString(operatorFound.OperatorID);
                    item.term_end = pPhone;
                    item.term_init = pPhone;
                    item.term_id = Guid.NewGuid().ToString();

                    AzureSearchValue azureValue = new AzureSearchValue();
                    List<RangeAzureSearch> values = new List<RangeAzureSearch>();
                    values.Add(item);
                    azureValue.value = values;

                    InsertValueIntoAzureSearch(azureValue);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError("AzureSearch: " + ex.Message);
            }
        }

        public bool CheckExistingEntry(string msisdn)
        {
            bool exists = false;

            string url = "https://gpstermsearch.search.windows.net/indexes/gpsterms/docs?search=*&$filter= term_init  eq '" + msisdn + "' &api-version=2015-02-28";
            string jsonResult = null;

            HttpWebResponse response = null;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("api-key", Values.AzureSearchApiKey);

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                var streamReader = new StreamReader(response.GetResponseStream());
                jsonResult = streamReader.ReadToEnd();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var resultObjects = AllChildren(JObject.Parse(jsonResult)).First(c => c.Type == JTokenType.Array && c.Path.Contains("value")).Children<JObject>();

                    int resultCount = resultObjects.Count();

                    if (resultCount > 0)
                    {
                        exists = true;
                    }
                    else
                    {
                        exists = false;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError("AzureSearch: " + ex.Message);
            }

            return exists;
        }

        public bool ComparePhoneNumber(string pMsisdn, string pOperatorName, int pPersonID, int pPersonCountryID)
        {
            bool isTheSame = false;
            string countryOperatorName = string.Empty;

            string url = "https://gpstermsearch.search.windows.net/indexes/gpsterms/docs?search=*&$filter= term_init  eq '" + pMsisdn + "' &api-version=2015-02-28";

            HttpWebResponse response = null;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Headers.Add("api-key", Values.AzureSearchApiKey);

            string jsonResult;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (var sr = new StreamReader(response.GetResponseStream()))
                {
                    jsonResult = sr.ReadToEnd();
                }

                AzureSearchValue searchValue = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<AzureSearchValue>(jsonResult);
                string currentMnoID = searchValue.value[0].mno_id;


                CountryEN personCountry = topupDAL.GetCountryByID(pPersonCountryID);
                countryOperatorName = pOperatorName + " " + personCountry;


                OperatorEN operatorFound = topupDAL.GetOperatorByBrand(pPersonCountryID, countryOperatorName);

                if (operatorFound != null)
                { 
                    isTheSame = (String.Equals(currentMnoID, operatorFound.OperatorID)) ? true : false ;

                    if (!isTheSame)
                        UpdateCurrentPhone(searchValue.value[0].term_id, Convert.ToString(operatorFound.OperatorID));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError("AzureSearch: " + ex.Message);
            }

            return isTheSame;
        }

        private void UpdateCurrentPhone(string pTermID, string pNewPhone)
        {
            string requestBody = "{\"value\":[{\"@search.action\":\"merge\",\"term_id\":\"" + pTermID + "\",\"mno_id\":\"" + pNewPhone + "\"}]}";

            string updateUrl = "https://gpstermsearch.search.windows.net/indexes/gpsterms/docs/index?api-version=2015-02-28";
            HttpWebRequest postRequest = (HttpWebRequest)HttpWebRequest.Create(updateUrl);
            postRequest.Method = "POST";
            postRequest.ContentType = "application/json";
            postRequest.Headers.Add("api-key", Values.AzureSearchApiKey);

            try
            {
                using (var streamWriter = new StreamWriter(postRequest.GetRequestStream()))
                {
                    streamWriter.Write(requestBody);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                WebResponse response = postRequest.GetResponse();
                var streamReader = new StreamReader(response.GetResponseStream());
                var result = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError("AzureSearch: " + ex.Message);
            }
        }

        private void InsertValueIntoAzureSearch(AzureSearchValue pItem)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("https://gpstermsearch.search.windows.net/indexes/gpsterms/docs/index?api-version=2015-02-28");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("api-key", Values.AzureSearchApiKey);

            try
            {
                var serializer = new JavaScriptSerializer();
                var serializedResult = serializer.Serialize(pItem);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(serializedResult);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                WebResponse response = request.GetResponse();
                var streamReader = new StreamReader(response.GetResponseStream());
                var result = streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLogger.LogError("AzureSearch: " + ex.Message);
            }
        }

        private IEnumerable<JToken> AllChildren(JToken json)
        {
            foreach (var c in json.Children())
            {
                yield return c;
                foreach (var cc in AllChildren(c))
                {
                    yield return cc;
                }
            }
        }
    }
}
