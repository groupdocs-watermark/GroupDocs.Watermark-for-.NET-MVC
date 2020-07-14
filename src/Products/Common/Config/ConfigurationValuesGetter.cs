using System;

namespace GroupDocs.Watermark.MVC.Products.Common.Config
{
    /// <summary>
    /// ConfigurationValuesGetter.
    /// </summary>
    public class ConfigurationValuesGetter
    {
        private readonly dynamic configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationValuesGetter"/> class.
        /// </summary>
        /// <param name="configuration">dynamic.</param>
        public ConfigurationValuesGetter(dynamic configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// GetStringPropertyValue.
        /// </summary>
        /// <param name="propertyName">string.</param>
        /// <returns>StringPropertyValue.</returns>
        public string GetStringPropertyValue(string propertyName)
        {
            return (this.configuration != null && this.configuration[propertyName] != null && !string.IsNullOrEmpty(this.configuration[propertyName].ToString())) ?
                this.configuration[propertyName].ToString() :
                null;
        }

        /// <summary>
        /// GetStringPropertyValue.
        /// </summary>
        /// <param name="propertyName">propertyName string.</param>
        /// <param name="defaultValue">defaultValue string.</param>
        /// <returns>StringPropertyValue.</returns>
        public string GetStringPropertyValue(string propertyName, string defaultValue)
        {
            return (this.configuration != null && this.configuration[propertyName] != null && !string.IsNullOrEmpty(this.configuration[propertyName].ToString())) ?
                this.configuration[propertyName].ToString() :
                defaultValue;
        }

        /// <summary>
        /// GetIntegerPropertyValue.
        /// </summary>
        /// <param name="propertyName">string.</param>
        /// <param name="defaultValue">int.</param>
        /// <returns>IntegerPropertyValue.</returns>
        public int GetIntegerPropertyValue(string propertyName, int defaultValue)
        {
            int value;
            value = (this.configuration != null && this.configuration[propertyName] != null && !string.IsNullOrEmpty(this.configuration[propertyName].ToString())) ?
                Convert.ToInt32(this.configuration[propertyName]) :
                defaultValue;
            return value;
        }

        /// <summary>
        /// GetIntegerPropertyValue.
        /// </summary>
        /// <param name="propertyName">string.</param>
        /// <param name="defaultValue">int.</param>
        /// <param name="innerPropertyName">innerPropertyName.</param>
        /// <returns>IntegerPropertyValue.</returns>
        public int GetIntegerPropertyValue(string propertyName, int defaultValue, string innerPropertyName)
        {
            int value;
            if (!string.IsNullOrEmpty(innerPropertyName))
            {
                value = (this.configuration != null && this.configuration[propertyName] != null && !string.IsNullOrEmpty(this.configuration[propertyName][innerPropertyName].ToString())) ?
                    Convert.ToInt32(this.configuration[propertyName][innerPropertyName]) :
                    defaultValue;
            }
            else
            {
                value = (this.configuration != null && this.configuration[propertyName] != null && !string.IsNullOrEmpty(this.configuration[propertyName].ToString())) ?
                    Convert.ToInt32(this.configuration[propertyName]) :
                    defaultValue;
            }

            return value;
        }

        /// <summary>
        /// GetBooleanPropertyValue.
        /// </summary>
        /// <param name="propertyName">string.</param>
        /// <param name="defaultValue">bool.</param>
        /// <returns>BooleanPropertyValue.</returns>
        public bool GetBooleanPropertyValue(string propertyName, bool defaultValue)
        {
            return (this.configuration != null && this.configuration[propertyName] != null && !string.IsNullOrEmpty(this.configuration[propertyName].ToString())) ? Convert.ToBoolean(this.configuration[propertyName]) : defaultValue;
        }
    }
}