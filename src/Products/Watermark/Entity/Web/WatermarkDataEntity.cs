namespace GroupDocs.Watermark.MVC.Products.Watermark.Entity.Web
{
    /// <summary>
    /// WatermarkDataEntity.
    /// </summary>
    public class WatermarkDataEntity
    {
        public int id { get; set; }

        public int pageNumber { get; set; }

        public int fontColor { get; set; }

        public float fontSize { get; set; }

        public float left { get; set; }

        public float top { get; set; }

        public float width { get; set; }

        public float height { get; set; }

        public string documentType { get; set; }

        public string text { get; set; }

        public string font { get; set; }

        public string watermarkGuid { get; set; }

        public string watermarkType { get; set; }
    }
}