using GroupDocs.Watermark.MVC.Products.Watermark.Config;
using GroupDocs.Watermark.MVC.Products.Watermark.Entity.Directory;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Util.Directory
{
    /// <summary>
    /// DataDirectoryUtils.
    /// </summary>
    public class DataDirectoryUtils : IDirectoryUtils
    {
        private readonly string DATA_FOLDER = "/WatermarkData";
        private WatermarkConfiguration watermarkConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataDirectoryUtils"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="watermarkConfig">WatermarkConfiguration.</param>
        public DataDirectoryUtils(WatermarkConfiguration watermarkConfig)
        {
            this.watermarkConfiguration = watermarkConfig;

            if (string.IsNullOrEmpty(watermarkConfig.dataDirectory))
            {
                watermarkConfig.dataDirectory = watermarkConfig.filesDirectory + this.DATA_FOLDER;
            }

            // create directory objects
            this.ImageDirectory = new ImageDataDirectoryEntity(this.watermarkConfiguration);
            this.UploadedImageDirectory = new UploadedImageDataDirectoryEntity(this.watermarkConfiguration);
            this.TextDirectory = new TextDataDirectoryEntity(this.watermarkConfiguration);

            // create directories
            System.IO.Directory.CreateDirectory(this.ImageDirectory.Path);
            System.IO.Directory.CreateDirectory(this.TextDirectory.XmlPath);

            string uploadedImagePath = this.ImageDirectory.Path + this.ImageDirectory.GetUploadedImageFolder();
            System.IO.Directory.CreateDirectory(uploadedImagePath);
        }

        public ImageDataDirectoryEntity ImageDirectory { get; set; }

        public UploadedImageDataDirectoryEntity UploadedImageDirectory { get; set; }

        public TextDataDirectoryEntity TextDirectory { get; set; }

        /// <summary>
        /// GetPath.
        /// </summary>
        /// <returns>string.</returns>
        public string GetPath()
        {
            return this.watermarkConfiguration.dataDirectory;
        }
    }
}