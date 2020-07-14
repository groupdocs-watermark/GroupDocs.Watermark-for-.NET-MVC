using Newtonsoft.Json;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Entity.Web
{
    /// <summary>
    /// DeleteWatermarkFileRequest.
    /// </summary>
    public class DeleteWatermarkFileRequest
    {
        [JsonProperty]
        private string guid;
        [JsonProperty]
        private string watermarkType;

        public string getGuid()
        {
            return this.guid;
        }

        public void setGuid(string guid)
        {
            this.guid = guid;
        }

        public string getWatermarkType()
        {
            return this.watermarkType;
        }

        public void setWatermarkType(string watermarkType)
        {
            this.watermarkType = watermarkType;
        }
    }
}