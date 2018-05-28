using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.DAL;

namespace YoVendoRecargaAPI.BL
{
    public class BuildVersionBL
    {
        BuildVersionDAL buildDAL = new BuildVersionDAL();

        public BuildVersionEN BuildVersion(string pClientVersion, string pPlatform)
        {
            BuildVersionEN version = buildDAL.BuildVersion(pPlatform);

            try
            {
                double latestVersion = Convert.ToDouble(version.VersionName);
                double userVersion = Convert.ToDouble(pClientVersion);

                if (userVersion >= latestVersion)
                {
                    version.Valid = true;
                    //version.RequiredVersion = build.VersionName;
                    version.Result = "Application up to date";
                }
                else
                {
                    if (version.Required == true)
                    {
                        version.Valid = false;
                        //version.RequiredVersion = build.VersionName;
                        version.Result = "Update to latest version required";
                    }
                    else
                    {
                        version.Valid = true;
                        //buildResult.RequiredVersion = build.VersionName;
                        version.Result = "Update available but not required";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError(ex.Message);
            }

            return version;
        }
    }
}
