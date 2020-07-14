using GroupDocs.Watermark.MVC.Products.Common.Entity.Web;
using GroupDocs.Watermark.MVC.Products.Watermark.Entity.XML;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Entity.Web
{
    /// <summary>
    /// WatermarkLoadedPageEntity.
    /// </summary>
    public class WatermarkLoadedPageEntity : PageDescriptionEntity
    {
        public XmlEntity props { get; set; }
    }
}