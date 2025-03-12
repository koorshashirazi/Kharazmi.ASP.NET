using System;

namespace Kharazmi.AspNetMvc.Core.Managers.PdfManager
{
    public sealed class DocumentCreationFailedException : Exception
    {
        internal DocumentCreationFailedException(string message)
            : base(message)
        {
        }
    }
}