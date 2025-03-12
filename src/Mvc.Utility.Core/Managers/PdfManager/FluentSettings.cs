using System;
using System.Globalization;

namespace Mvc.Utility.Core.Managers.PdfManager
{
    public static class FluentSettings
    {
        public static IPdfDocument Comressed(this IPdfDocument pdfDocument)
        {
            return pdfDocument.WithGlobalSetting(Constants.USE_COMPRESSION, "true");
        }

        public static IPdfDocument WithTitle(this IPdfDocument pdfDocument, string title)
        {
            return pdfDocument.WithGlobalSetting(Constants.DOCUMENT_TITLE, title);
        }

        public static IPdfDocument WithOutline(this IPdfDocument pdfDocument)
        {
            return pdfDocument.WithGlobalSetting(Constants.OUTLINE, "true");
        }

        public static IPdfDocument WithoutOutline(this IPdfDocument pdfDocument)
        {
            return pdfDocument.WithGlobalSetting(Constants.OUTLINE, "false");
        }

        public static IPdfDocument EncodedWith(this IPdfDocument pdfDocument, string encoding)
        {
            return pdfDocument.WithObjectSetting(Constants.WEB_DEFAULT_ENCODING, encoding);
        }

        public static IPdfDocument Landscape(this IPdfDocument pdfDocument)
        {
            return pdfDocument
                .WithGlobalSetting(Constants.ORIENTATION, Constants.LANDSCAPE);
        }

        public static IPdfDocument Portrait(this IPdfDocument pdfDocument)
        {
            return pdfDocument
                .WithGlobalSetting(Constants.ORIENTATION, Constants.PORTRAIT);
        }

        public static IPdfDocument OfSize(this IPdfDocument pdfDocument, PaperSize paperSize)
        {
            return pdfDocument
                .WithGlobalSetting(Constants.SIZE_WIDTH, paperSize.Width)
                .WithGlobalSetting(Constants.SIZE_HEIGHT, paperSize.Height);
        }

        public static IPdfDocument WithMargins(this IPdfDocument pdfDocument
            , Func<PaperMargins, PaperMargins> paperMargins)
        {
            return pdfDocument.WithMargins(paperMargins(PaperMargins.None()));
        }

        public static IPdfDocument WithMargins(this IPdfDocument pdfDocument, PaperMargins margins)
        {
            return pdfDocument
                .WithGlobalSetting(Constants.MARGIN_BOTTOM, margins.BottomSetting)
                .WithGlobalSetting(Constants.MARGIN_LEFT, margins.LeftSetting)
                .WithGlobalSetting(Constants.MARGIN_RIGHT, margins.RightSetting)
                .WithGlobalSetting(Constants.MARGIN_TOP, margins.TopSetting);
        }

        public static IPdfDocument WithResolution(this IPdfDocument pdfDocument, int dpi)
        {
            return pdfDocument
                .WithGlobalSetting(Constants.DPI, dpi.ToString(CultureInfo.InvariantCulture));
        }
    }
}