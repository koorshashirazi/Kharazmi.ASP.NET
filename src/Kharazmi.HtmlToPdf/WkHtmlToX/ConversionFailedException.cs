using System;

namespace Kharazmi.HtmlToPdf.WkHtmlToX
{
    sealed class ConversionFailedException : Exception
    {
        public ConversionFailedException(string errorText)
            : base(errorText)
        {
        }
    }
}