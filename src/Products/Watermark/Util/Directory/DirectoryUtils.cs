using GroupDocs.Watermark.MVC.Products.Watermark.Config;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Util.Directory
{
    /// <summary>
    /// DirectoryUtils.
    /// </summary>
    public class DirectoryUtils
    {
        internal FilesDirectoryUtils FilesDirectory;
        internal DataDirectoryUtils DataDirectory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryUtils"/> class.
        /// </summary>
        /// <param name="watermarkConfiguration">WatermarkConfiguration.</param>
        public DirectoryUtils(WatermarkConfiguration watermarkConfiguration)
        {
            this.FilesDirectory = new FilesDirectoryUtils(watermarkConfiguration);
            this.DataDirectory = new DataDirectoryUtils(watermarkConfiguration);
        }
    }
}