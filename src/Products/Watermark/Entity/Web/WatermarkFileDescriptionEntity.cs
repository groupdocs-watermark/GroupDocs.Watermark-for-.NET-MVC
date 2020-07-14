using GroupDocs.Watermark.MVC.Products.Common.Entity.Web;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Entity.Web
{
    /// <summary>
    /// WatermarkFileDescriptionEntity.
    /// </summary>
    public class WatermarkFileDescriptionEntity : FileDescriptionEntity
    {
        public string image { get; set; }

        public string text { get; set; }

        public string fontColor { get; set; }
    }
}