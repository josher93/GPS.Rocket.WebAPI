using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;

namespace YoVendoRecargaAPI.DAL
{
    public class BuildVersionDAL
    {
        Connection connection = new Connection();

        public BuildVersionEN BuildVersion(string pPlatform)
        {
            BuildVersionEN buildVersion = new BuildVersionEN();

            try
            {
                connection.Cnn.Open();
                buildVersion = connection.Cnn.Query<BuildVersionEN>("SpValidateBuildVersion", new { @platformName = pPlatform },
                    commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BuildVersionDAL: " + ex.Message);
                EventViewerLoggerDAL.LogError(ex.Message);
            }
            finally
            {
                connection.Cnn.Close();
            }

            return buildVersion;
        }
    }
}
