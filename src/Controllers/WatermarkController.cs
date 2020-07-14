using System.Web.Mvc;

namespace GroupDocs.Watermark.MVC.Controllers
{
    /// <summary>
    /// Watermark Web page controller.
    /// </summary>
    public class WatermarkController : Controller
    {
        // View Watermark
        public ActionResult Index()
        {
            return this.View();
        }
    }
}