using Kharazmi.HtmlToPdf.Interop;

namespace Kharazmi.HtmlToPdf.WkHtmlToX
{
    public static class WkHtmlToPdfHelper
    {
        public static void Convert(this WkHtmlToPdfContext wkHtmlToPdfContext, string html)
        {
            StringCallback errorCallback = (converter, errorText) =>
            {
                throw new ConversionFailedException(errorText);
            };

            Kharazmi.HtmlToPdf.WkHtmlToX.WkHtmlToPdf.wkhtmltopdf_set_error_callback(wkHtmlToPdfContext.ConverterPointer, errorCallback);
            Kharazmi.HtmlToPdf.WkHtmlToX.WkHtmlToPdf.wkhtmltopdf_add_object(wkHtmlToPdfContext.ConverterPointer, wkHtmlToPdfContext.ObjectSettingsPointer, html);
            Kharazmi.HtmlToPdf.WkHtmlToX.WkHtmlToPdf.wkhtmltopdf_convert(wkHtmlToPdfContext.ConverterPointer);
        }
    }
}