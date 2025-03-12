#if !METRO

namespace Mvc.Utility.Core.Managers.HtmlManager
{
    /// <summary>
    ///     Represents a fragment of text in a mixed code document.
    /// </summary>
    public class MixedCodeDocumentTextFragment : MixedCodeDocumentFragment
    {
        #region Constructors

        internal MixedCodeDocumentTextFragment(MixedCodeDocument doc)
            :
            base(doc, MixedCodeDocumentFragmentType.Text)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///     Gets the fragment text.
        /// </summary>
        public string Text
        {
            get => FragmentText;
            set => FragmentText = value;
        }

        #endregion Properties
    }
}

#endif