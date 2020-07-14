using GroupDocs.Watermark.MVC.Products.Watermark.Config;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Util.Directory
{
    /// <summary>
    /// TempDirectoryUtils.
    /// </summary>
    public class TempDirectoryUtils : IDirectoryUtils
    {
        private WatermarkConfiguration watermarkConfiguration;

        /// <summary>
        /// Get path.
        /// </summary>
        /// <returns>string.</returns>
        public string GetPath()
        {
            return this.watermarkConfiguration.GetTempFilesDirectory();
        }
    }
}