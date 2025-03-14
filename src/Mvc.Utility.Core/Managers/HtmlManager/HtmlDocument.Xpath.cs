﻿#if !METRO

using System.Xml.XPath;

namespace Mvc.Utility.Core.Managers.HtmlManager
{
    public partial class HtmlDocument : IXPathNavigable
    {
        /// <summary>
        ///     Creates a new XPathNavigator object for navigating this HTML document.
        /// </summary>
        /// <returns>An XPathNavigator object. The XPathNavigator is positioned on the root of the document.</returns>
        public XPathNavigator CreateNavigator()
        {
            return new HtmlNodeNavigator(this, DocumentNode);
        }
    }
}

#endif