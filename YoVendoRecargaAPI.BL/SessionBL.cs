using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.DAL;

namespace YoVendoRecargaAPI.BL
{
    public class SessionBL
    {
        SessionDAL sessionDAL = new SessionDAL();

        public int SignoutUser(int pSessionID)
        {
            int updated = default(int);

            try
            {
                var session = sessionDAL.GetSessionByID(pSessionID);

                if (session != null)
                {
                    session.ActiveSession = false;
                    updated = sessionDAL.UpdateSession(session);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("SignoutUser: " + ex.Message);
            }

            return updated;
        }
    }
}
