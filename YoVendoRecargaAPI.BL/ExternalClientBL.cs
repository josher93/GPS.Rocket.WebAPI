using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.DAL;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.BL
{
    public class ExternalClientBL
    {
        JWT jwt = new JWT();
        ExternalClientDAL clientDAL = new ExternalClientDAL();

        public ExternalClientEN VerifyExternalClient(IEnumerable<String> pClientKey)
        {
            ExternalClientEN client = null;

            try
            {
                var docodedKey = jwt.decodeClientKey(pClientKey.First().ToString());
                client = clientDAL.AuthenticateClient(docodedKey.GUID, docodedKey.AssignedPassword, docodedKey.RegDate);
            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return client;

        }

        public String CreateClientKey(ExternalClientEN pClient)
        {
            String clientKey = "";

            try
            {
                clientKey = jwt.encodeClientKey(pClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return clientKey;
        }

        public ExternalClientEN RegisterExternalClient(int pCountryID, string pName, string pAlias, string pDescription, string pPassword, ref string oApiKey)
        {
            ExternalClientEN client = new ExternalClientEN();
            int externalClientID = 0;
            var now = DateTime.Now;

            try
            {
                
                client.Active = true;
                client.Alias = pAlias;
                client.AssignedPassword = pPassword;
                client.CountryID = pCountryID;
                client.Description = pDescription;
                client.GUID = Guid.NewGuid().ToString().ToUpper();
                client.Name = pName;
                client.RegDate = now.Date;

                externalClientID = clientDAL.InsertExternalClient(client);

                if (externalClientID > 0)
                {
                    oApiKey = jwt.encodeClientKey(client);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("RegisterExternalClient: " + ex.Message);
            }

            return client;
        }
    }
}
