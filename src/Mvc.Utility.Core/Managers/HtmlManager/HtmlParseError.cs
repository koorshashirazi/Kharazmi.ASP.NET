namespace Mvc.Utility.Core.Managers.HtmlManager
{
    /// <summary>
    ///     Represents a parsing error found during document parsing.
    /// </summary>
    public class HtmlParseError
    {
        #region Constructors

        internal HtmlParseError(HtmlParseErrorCode code,
            int line,
            int linePosition,
            int streamPosition,
            string sourceText,
            string reason)
        {
            Code = code;
            Line = line;
            LinePosition = linePosition;
            StreamPosition = streamPosition;
            SourceText = sourceText;
            Reason = reason;
        }

        #endregion Constructors


        #region Properties

        /// <summary>
        ///     Gets the type of error.
        /// </summary>
        public HtmlParseErrorCode Code { get; }

        /// <summary>
        ///     Gets the line number of this error in the document.
        /// </summary>
        public int Line { get; }

        /// <summary>
        ///     Gets the column number of this error in the document.
        /// </summary>
        public int LinePosition { get; }

        /// <summary>
        ///     Gets a description for the error.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        ///     Gets the the full text of the line containing the error.
        /// </summary>
        public string SourceText { get; }

        /// <summary>
        ///     Gets the absolute stream position of this error in the document, relative to the start of the document.
        /// </summary>
        public int StreamPosition { get; }

        #endregion Properties
    }
}