using GroupDocs.Watermark.MVC.Products.Common.Entity.Web;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Entity.Web
{
    /// <summary>
    /// WatermarkPostedDataEntity.
    /// </summary>
    public class WatermarkPostedDataEntity : PostedDataEntity
    {
        public string watermarkType { get; set; }

        public WatermarkDataEntity[] watermarksData { get; set; }

        public string image { get; set; }

        public string documentType { get; set; }
    }
}