using GroupDocs.Watermark.MVC.Products.Watermark.Config;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Util.Directory
{
    /// <summary>
    /// FilesDirectoryUtils.
    /// </summary>
    public class FilesDirectoryUtils : IDirectoryUtils
    {
        private WatermarkConfiguration watermarkConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilesDirectoryUtils"/> class.
        /// </summary>
        /// <param name="watermarkConfiguration">WatermarkConfiguration.</param>
        public FilesDirectoryUtils(WatermarkConfiguration watermarkConfiguration)
        {
            this.watermarkConfiguration = watermarkConfiguration;
        }

        /// <summary>
        /// Get path.
        /// </summary>
        /// <returns>string.</returns>
        public string GetPath()
        {
            return this.watermarkConfiguration.GetFilesDirectory();
        }
    }
}