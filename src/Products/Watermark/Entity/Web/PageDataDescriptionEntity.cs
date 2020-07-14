using GroupDocs.Watermark.MVC.Products.Common.Entity.Web;
using Newtonsoft.Json;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Entity.Web
{
    /// <summary>
    /// PageDataDescriptionEntity.
    /// </summary>
    public class PageDataDescriptionEntity : PageDescriptionEntity
    {
        // List of watermark data.
        [JsonProperty]
        private WatermarkDataEntity[] watermarks;

        public void SetWatermarks(WatermarkDataEntity[] watermarks)
        {
            this.watermarks = watermarks;
        }

        public WatermarkDataEntity[] GetWatermarks()
        {
            return this.watermarks;
        }
    }
}