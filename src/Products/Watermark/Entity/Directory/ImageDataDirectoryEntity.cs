using GroupDocs.Watermark.MVC.Products.Watermark.Config;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Entity.Directory
{
    /// <summary>
    /// ImageDataDirectoryEntity.
    /// </summary>
    public class ImageDataDirectoryEntity : DataDirectoryEntity
    {
        private string UPLOADED_IMAGE = "/Uploaded";

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageDataDirectoryEntity"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="watermarkConfiguration">WatermarkConfiguration.</param>
        public ImageDataDirectoryEntity(WatermarkConfiguration watermarkConfiguration)
            : base(watermarkConfiguration, "/Image")
        {
        }

        public string GetUploadedImageFolder()
        {
            return this.UPLOADED_IMAGE;
        }

        public void SetUploadedImageFolder(string path)
        {
            this.UPLOADED_IMAGE = path;
        }
    }
}