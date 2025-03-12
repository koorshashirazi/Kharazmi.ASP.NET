using System;

namespace Mvc.Utility.Core.Managers.PdfManager
{
    public sealed class PdfDocumentCreationFailedException : Exception
    {
        public PdfDocumentCreationFailedException(string error)
            : base(error)
        {
        }
    }
}