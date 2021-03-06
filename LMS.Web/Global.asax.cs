using log4net;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LMS.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            ILog Log = LogManager.GetLogger(typeof(MvcApplication));
            Log.Fatal(exception.Message, exception);
            Server.ClearError();
            Response.Redirect("/Authentication/Error");
        }
    }
}
