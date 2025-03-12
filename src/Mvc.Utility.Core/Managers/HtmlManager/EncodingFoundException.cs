using System;
using System.Text;

namespace Mvc.Utility.Core.Managers.HtmlManager
{
    internal class EncodingFoundException : Exception
    {
        #region Constructors

        internal EncodingFoundException(Encoding encoding)
        {
            Encoding = encoding;
        }

        #endregion Constructors

        #region Properties

        internal Encoding Encoding { get; }

        #endregion Properties
    }
}