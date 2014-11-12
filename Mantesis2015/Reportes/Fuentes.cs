using System;
using System.Linq;
using iTextSharp.text;

namespace Mantesis2015.Reportes
{
    public class Fuentes
    {

        #region Fuentes

        public const String ArialFont = "Arial";


        #endregion

        #region Font Colors

        public static BaseColor Black = new BaseColor(0, 0, 0);
        public static BaseColor Red = new BaseColor(255, 0, 0);

        #endregion


        public static Font NormalFont(BaseColor color, string fontName, int fontSize)
        {
            Font font = FontFactory.GetFont(fontName, fontSize, Font.NORMAL, color);

            return font;
        }

        public static Font BoldFont(BaseColor color, string fontName, int fontSize)
        {
            Font font = FontFactory.GetFont(fontName, fontSize, Font.BOLD, color);

            return font;
        }

        public static Font ItalicFont(BaseColor color, string fontName, int fontSize)
        {
            Font font = FontFactory.GetFont(fontName, fontSize, Font.ITALIC, color);

            return font;
        }

        public static Font UnderlineFont(BaseColor color, string fontName, int fontSize)
        {
            Font font = FontFactory.GetFont(fontName, fontSize, Font.UNDERLINE, color);

            return font;
        }

        /// <summary>
        /// Fuentes encabezado y Pie de Pagina
        /// </summary>

        public static Font Footer
        {
            get
            {
                // create a basecolor to use for the Footer font, if needed.
                BaseColor grey = new BaseColor(128, 128, 128);
                Font font = FontFactory.GetFont("Arial", 9, Font.NORMAL, grey);
                return font;
            }
        }

        public static Font Header
        {
            get
            {
                BaseColor grey = new BaseColor(255, 0, 0);
                Font font = FontFactory.GetFont("Arial", 16, Font.NORMAL, grey);
                return font;
            }
        }

    }
}
