using GroupDocs.Watermark.MVC.Products.Watermark.Config;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Entity.Directory
{
    /// <summary>
    /// TextDataDirectoryEntity.
    /// </summary>
    public class TextDataDirectoryEntity : DataDirectoryEntity
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TextDataDirectoryEntity"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="watermarkConfiguration">WatermarkConfiguration.</param>
        public TextDataDirectoryEntity(WatermarkConfiguration watermarkConfiguration)
                : base(watermarkConfiguration, "/Text")
        {
        }
    }
}