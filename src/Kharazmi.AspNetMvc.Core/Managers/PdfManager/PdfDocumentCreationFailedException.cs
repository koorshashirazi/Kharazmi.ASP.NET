using System;

namespace Kharazmi.AspNetMvc.Core.Managers.PdfManager
{
    public sealed class PdfDocumentCreationFailedException : Exception
    {
        public PdfDocumentCreationFailedException(string error)
            : base(error)
        {
        }
    }
}