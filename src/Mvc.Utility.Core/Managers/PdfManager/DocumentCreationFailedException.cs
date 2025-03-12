using System;

namespace Mvc.Utility.Core.Managers.PdfManager
{
    public sealed class DocumentCreationFailedException : Exception
    {
        internal DocumentCreationFailedException(string message)
            : base(message)
        {
        }
    }
}