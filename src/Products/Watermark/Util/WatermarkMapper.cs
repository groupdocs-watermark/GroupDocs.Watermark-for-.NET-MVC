using GroupDocs.Watermark.MVC.Products.Watermark.Entity.Web;
using GroupDocs.Watermark.Search;
using GroupDocs.Watermark.Watermarks;
using System.Collections.Generic;
using System.Linq;

namespace GroupDocs.Watermark.MVC.Products.Watermark.Util
{
    /// <summary>
    /// WatermarkMapper.
    /// </summary>
    public class WatermarkMapper
    {
        private WatermarkMapper()
        {
        }

        /// <summary>
        /// Map WatermarkInfo instances into WatermarkDataEntity.
        /// </summary>
        /// <param name="watermarks">WatermarkInfo[].</param>
        /// <returns>WatermarkDataEntity[].</returns>
        public static WatermarkDataEntity[] MapForPage(PossibleWatermarkCollection watermarks)
        {
            // initiate watermarks data array
            IList<WatermarkDataEntity> pageWatermarks = new List<WatermarkDataEntity>();

            // Each watermark data - this functionality used since watermarks data returned by the
            // GroupDocs.Watermark library are obfuscated
            for (int n = 0; n < watermarks.Count; n++)
            {
                PossibleWatermark watermarkInfo = watermarks[n];
                WatermarkDataEntity watermark = MapWatermarkDataEntity(watermarkInfo);
                pageWatermarks.Add(watermark);
            }

            return pageWatermarks.ToArray();
        }

        /// <summary>
        /// Map WatermarkInfo instances into WatermarkDataEntity.
        /// </summary>
        /// <param name="watermarkInfo">WatermarkInfo.</param>
        /// <returns>WatermarkDataEntity.</returns>
        public static WatermarkDataEntity MapWatermarkDataEntity(PossibleWatermark watermarkInfo)
        {
            WatermarkDataEntity watermark = new WatermarkDataEntity();

            watermark.height = (float)watermarkInfo.Height;
            watermark.width = (float)watermarkInfo.Width;
            watermark.left = (float)watermarkInfo.X;
            watermark.top = (float)watermarkInfo.Y;
            watermark.text = watermarkInfo.Text;

            watermark.font = watermarkInfo.FormattedTextFragments.First().Font.FamilyName.ToString();
            watermark.fontSize = watermarkInfo.FormattedTextFragments.First().Font.Size;

            Color foregroundColor = watermarkInfo.FormattedTextFragments.First().ForegroundColor;
            Color colorWoAlpha = Color.FromArgb(0, foregroundColor.R, foregroundColor.G, foregroundColor.B);
            watermark.fontColor = colorWoAlpha.ToArgb();

            watermark.pageNumber = 1;
            watermark.watermarkType = "text";

            return watermark;
        }
    }
}