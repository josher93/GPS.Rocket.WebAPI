using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YoVendoRecargaAPI.Entities;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.WebAPI.Models;

namespace YoVendoRecargaAPI.WebAPI.Interactors
{
    public class BuildValidatorInteractor
    {
        public BuildResponse CreateBuildResponse(BuildVersionEN pBuildVersion)
        {
            BuildResponse response = new BuildResponse();

            try
            {
                response.Valid = pBuildVersion.Valid;
                response.RequiredVersion = pBuildVersion.VersionName;
                response.Platform = pBuildVersion.Platform;
                response.Message = pBuildVersion.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
                EventViewerLoggerBL.LogError("BuildValidatorInteractor: " + ex.Message);
            }

            return response;
        }
    }
}