namespace Mvc.Utility.Core.Managers.HtmlManager
{
    /// <summary>
    ///     Represents an HTML text node.
    /// </summary>
    public class HtmlTextNode : HtmlNode
    {
        #region Fields

        private string _text;

        #endregion Fields

        #region Constructors

        internal HtmlTextNode(HtmlDocument ownerdocument, int index)
            :
            base(HtmlNodeType.Text, ownerdocument, index)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///     Gets or Sets the HTML between the start and end tags of the object. In the case of a text node, it is equals to
        ///     OuterHtml.
        /// </summary>
        public override string InnerHtml
        {
            get => OuterHtml;
            set => _text = value;
        }

        /// <summary>
        ///     Gets or Sets the object and its content in HTML.
        /// </summary>
        public override string OuterHtml
        {
            get
            {
                if (_text == null) return base.OuterHtml;

                return _text;
            }
        }

        /// <summary>
        ///     Gets or Sets the text of the node.
        /// </summary>
        public string Text
        {
            get
            {
                if (_text == null) return base.OuterHtml;

                return _text;
            }
            set
            {
                _text = value;
                SetChanged();
            }
        }

        #endregion Properties
    }
}