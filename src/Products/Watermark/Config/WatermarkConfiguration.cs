using GroupDocs.Watermark.MVC.Products.Common.Config;
using GroupDocs.Watermark.MVC.Products.Common.Util.Parser;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Config
{
    /// <summary>
    /// WatermarkConfiguration.
    /// </summary>
    public class WatermarkConfiguration : CommonConfiguration
    {
        [JsonProperty]
        internal string filesDirectory = "DocumentSamples/Watermark";

        [JsonProperty]
        private string defaultDocument = string.Empty;

        [JsonProperty]
        internal string dataDirectory = string.Empty;

        [JsonProperty]
        private string tempFilesDirectory = string.Empty;

        [JsonProperty]
        private int preloadPageCount;

        [JsonProperty]
        private bool zoom = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkConfiguration"/> class.
        /// Get watermark configuration section from the Web.config.
        /// </summary>
        public WatermarkConfiguration()
        {
            YamlParser parser = new YamlParser();
            dynamic configuration = parser.GetConfiguration("watermark");
            ConfigurationValuesGetter valuesGetter = new ConfigurationValuesGetter(configuration);

            // get Watermark configuration section from the web.config
            this.filesDirectory = valuesGetter.GetStringPropertyValue("filesDirectory", this.filesDirectory);
            if (!IsFullPath(this.filesDirectory))
            {
                this.filesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.filesDirectory);
                if (!Directory.Exists(this.filesDirectory))
                {
                    Directory.CreateDirectory(this.filesDirectory);
                }
            }

            this.defaultDocument = valuesGetter.GetStringPropertyValue("defaultDocument", this.defaultDocument).Replace(@"\", "/");
            this.preloadPageCount = valuesGetter.GetIntegerPropertyValue("preloadPageCount", this.preloadPageCount);
            this.zoom = valuesGetter.GetBooleanPropertyValue("zoom", this.zoom);
        }

        private static bool IsFullPath(string path)
        {
            return !string.IsNullOrWhiteSpace(path)
                && path.IndexOfAny(System.IO.Path.GetInvalidPathChars().ToArray()) == -1
                && Path.IsPathRooted(path)
                && !Path.GetPathRoot(path).Equals(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
        }

        public void SetFilesDirectory(string filesDirectory)
        {
            this.filesDirectory = filesDirectory;
        }

        public string GetFilesDirectory()
        {
            return this.filesDirectory;
        }

        public void SetDefaultDocument(string defaultDocument)
        {
            this.defaultDocument = defaultDocument;
        }

        public string GetDefaultDocument()
        {
            return this.defaultDocument;
        }

        public void SetPreloadPageCount(int preloadPageCount)
        {
            this.preloadPageCount = preloadPageCount;
        }

        public void SetTempFilesDirectory(string tempFilesDirectory)
        {
            this.tempFilesDirectory = tempFilesDirectory;
        }

        public string GetTempFilesDirectory()
        {
            return this.tempFilesDirectory;
        }

        public int GetPreloadPageCount()
        {
            return this.preloadPageCount;
        }
    }
}