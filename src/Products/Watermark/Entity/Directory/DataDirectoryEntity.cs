using GroupDocs.Watermark.MVC.Products.Watermark.Config;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Entity.Directory
{
    /// <summary>
    /// DataDirectoryEntity.
    /// </summary>
    public abstract class DataDirectoryEntity
    {
        public string Path { get; set; }

        public string PreviewPath { get; set; }

        public string XmlPath { get; set; }

        public static readonly string DATA_PREVIEW_FOLDER = "/Preview";
        public static readonly string DATA_XML_FOLDER = "/XML";
        protected WatermarkConfiguration watermarkConfiguration;
        protected string currentDirectoryPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataDirectoryEntity"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="watermarkConfiguration">WatermarkConfiguration.</param>
        /// <param name="currentDirectoryPath">string.</param>
        public DataDirectoryEntity(WatermarkConfiguration watermarkConfiguration, string currentDirectoryPath)
        {
            this.watermarkConfiguration = watermarkConfiguration;
            this.currentDirectoryPath = currentDirectoryPath;
            this.Path = watermarkConfiguration.dataDirectory + currentDirectoryPath;
            this.PreviewPath = watermarkConfiguration.dataDirectory + currentDirectoryPath + DATA_PREVIEW_FOLDER;
            this.XmlPath = watermarkConfiguration.dataDirectory + currentDirectoryPath + DATA_XML_FOLDER;
        }
    }
}