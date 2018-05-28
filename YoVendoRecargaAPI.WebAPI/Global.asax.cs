using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using YoVendoRecargaAPI.BL;
using YoVendoRecargaAPI.WebAPI.Interactors;

namespace YoVendoRecargaAPI.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Timer aTimer = new Timer();
            int time = int.Parse(ConfigurationManager.AppSettings["timerLeaderBoards"].ToString());
            aTimer.Interval = time * 1000;
            aTimer.Elapsed += new ElapsedEventHandler(aTimer_Tick);
            aTimer.Start();
        }

        void aTimer_Tick(object sender, EventArgs e)
        {
            var api = ConfigurationManager.AppSettings["Api"];
            if (api == "RecarGO")
            {
                EventViewerLoggerBL.LogError("Inicia Timer...");
                LeaderBoardsInteractor interactor = new LeaderBoardsInteractor();
                var responseSuccess = interactor.createResponseSaveJson();
            }
        }
    }
}
