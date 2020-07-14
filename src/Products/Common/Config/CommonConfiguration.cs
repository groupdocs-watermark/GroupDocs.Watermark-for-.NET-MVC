using GroupDocs.Watermark.MVC.Products.Common.Util.Parser;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace GroupDocs.Watermark.MVC.Products.Common.Config
{
    /// <summary>
    /// CommonConfiguration.
    /// </summary>
    public class CommonConfiguration : ConfigurationSection
    {
        [JsonProperty]
        public bool pageSelector { get; set; }

        [JsonProperty]
        public bool download { get; set; }

        [JsonProperty]
        public bool upload { get; set; }

        [JsonProperty]
        public bool print { get; set; }

        [JsonProperty]
        public bool browse { get; set; }

        [JsonProperty]
        public bool rewrite { get; set; }

        public bool enableRightClick { get; set; }

        private NameValueCollection commonConfiguration = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection("commonConfiguration");

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonConfiguration"/> class.
        /// Constructor.
        /// </summary>
        public CommonConfiguration()
        {
            YamlParser parser = new YamlParser();
            dynamic configuration = parser.GetConfiguration("common");
            ConfigurationValuesGetter valuesGetter = new ConfigurationValuesGetter(configuration);
            this.pageSelector = valuesGetter.GetBooleanPropertyValue("pageSelector", Convert.ToBoolean(this.commonConfiguration["isPageSelector"]));
            this.download = valuesGetter.GetBooleanPropertyValue("download", Convert.ToBoolean(this.commonConfiguration["isDownload"]));
            this.upload = valuesGetter.GetBooleanPropertyValue("upload", Convert.ToBoolean(this.commonConfiguration["isUpload"]));
            this.print = valuesGetter.GetBooleanPropertyValue("print", Convert.ToBoolean(this.commonConfiguration["isPrint"]));
            this.browse = valuesGetter.GetBooleanPropertyValue("browse", Convert.ToBoolean(this.commonConfiguration["isBrowse"]));
            this.rewrite = valuesGetter.GetBooleanPropertyValue("rewrite", Convert.ToBoolean(this.commonConfiguration["isRewrite"]));
            this.enableRightClick = valuesGetter.GetBooleanPropertyValue("enableRightClick", Convert.ToBoolean(this.commonConfiguration["enableRightClick"]));
        }
    }
}