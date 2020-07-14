using GroupDocs.Watermark.MVC.Products.Watermark.Config;

namespace GroupDocs.Watermark.MVC.Products.Common.Config
{
    /// <summary>
    /// Global configuration.
    /// </summary>
    public class GlobalConfiguration
    {
        public ServerConfiguration Server;
        public ApplicationConfiguration Application;
        public CommonConfiguration Common;
        public WatermarkConfiguration Watermark;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalConfiguration"/> class.
        /// Get all configurations.
        /// </summary>
        public GlobalConfiguration()
        {
            this.Server = new ServerConfiguration();
            this.Application = new ApplicationConfiguration();
            this.Watermark = new WatermarkConfiguration();
            this.Common = new CommonConfiguration();
        }
    }
}