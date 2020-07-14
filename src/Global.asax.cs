using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using GroupDocs.Watermark.MVC.AppDomainGenerator;

namespace GroupDocs.Watermark.MVC
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Fix required to use several GroupDocs products in one project.
            // Set GroupDocs products assemblies names
            string watermarkAssemblyName = "GroupDocs.Watermark.dll";

            // set GroupDocs.Watermark license
            DomainGenerator watermarkDomainGenerator = new DomainGenerator(watermarkAssemblyName, "GroupDocs.Watermark.License");
            watermarkDomainGenerator.SetWatermarkLicense();

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
