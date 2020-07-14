using GroupDocs.Watermark.MVC.Products.Watermark.Config;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Entity.Directory
{
    /// <summary>
    /// BarcodeDataDirectoryEntity.
    /// </summary>
    public class UploadedImageDataDirectoryEntity : DataDirectoryEntity
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadedImageDataDirectoryEntity"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="watermarkConfiguration">WatermarkConfiguration.</param>
        public UploadedImageDataDirectoryEntity(WatermarkConfiguration watermarkConfiguration)
            : base(watermarkConfiguration, "/Image/Uploaded")
        {
        }
    }
}