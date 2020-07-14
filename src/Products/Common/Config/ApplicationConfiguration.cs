﻿using GroupDocs.Watermark.MVC.Products.Common.Util.Parser;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GroupDocs.Watermark.MVC.Products.Common.Config
{
    /// <summary>
    /// Application configuration.
    /// </summary>
    public class ApplicationConfiguration
    {
        public string LicensePath = "Licenses";

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationConfiguration"/> class.
        /// Get license path from the application configuration section of the web.config.
        /// </summary>
        public ApplicationConfiguration()
        {
            YamlParser parser = new YamlParser();
            dynamic configuration = parser.GetConfiguration("application");
            ConfigurationValuesGetter valuesGetter = new ConfigurationValuesGetter(configuration);
            string license = valuesGetter.GetStringPropertyValue("licensePath");
            if (string.IsNullOrEmpty(license))
            {
                string[] files = System.IO.Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.LicensePath), "*.lic");
                this.LicensePath = Path.Combine(this.LicensePath, files[0]);
            }
            else
            {
                if (!IsFullPath(license))
                {
                    license = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, license);
                    if (!Directory.Exists(Path.GetDirectoryName(license)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(license));
                    }
                }

                this.LicensePath = license;
                if (!File.Exists(this.LicensePath))
                {
                    Debug.WriteLine("License file path is incorrect, launched in trial mode");
                    this.LicensePath = string.Empty;
                }
            }
        }

        private static bool IsFullPath(string path)
        {
            return !string.IsNullOrWhiteSpace(path)
                && path.IndexOfAny(System.IO.Path.GetInvalidPathChars().ToArray()) == -1
                && Path.IsPathRooted(path)
                && !Path.GetPathRoot(path).Equals(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal);
        }
    }
}