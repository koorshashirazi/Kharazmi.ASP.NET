using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Mvc.Utility.Core.Managers.HtmlManager
{
    /// <summary>
    ///     Represents an HTML node.
    /// </summary>
    [DebuggerDisplay("Name: {OriginalName}")]
    public partial class HtmlNode
    {
        #region Consts

        internal const string DepthLevelExceptionMessage = "The document is too complex to parse";

        #endregion Consts

        #region Fields

        internal HtmlAttributeCollection _attributes;
        internal HtmlNodeCollection _childnodes;
        internal HtmlNode _endnode;

        private bool _changed;
        internal string _innerhtml;
        internal int _innerlength;
        internal int _innerstartindex;
        internal int _line;
        internal int _lineposition;
        internal int _namelength;
        internal int _namestartindex;
        internal HtmlNode _nextnode;
        internal HtmlNodeType _nodetype;
        internal string _outerhtml;
        internal int _outerlength;
        internal int _outerstartindex;
        private string _optimizedName;
        internal HtmlDocument _ownerdocument;
        internal HtmlNode _parentnode;
        internal HtmlNode _prevnode;
        internal HtmlNode _prevwithsamename;
        internal bool _starttag;
        internal int _streamposition;
        internal bool _isImplicitEnd;

        #endregion Fields

        #region Static Members

        /// <summary>
        ///     Gets the name of a comment node. It is actually defined as '#comment'.
        /// </summary>
        public static readonly string HtmlNodeTypeNameComment = "#comment";

        /// <summary>
        ///     Gets the name of the document node. It is actually defined as '#document'.
        /// </summary>
        public static readonly string HtmlNodeTypeNameDocument = "#document";

        /// <summary>
        ///     Gets the name of a text node. It is actually defined as '#text'.
        /// </summary>
        public static readonly string HtmlNodeTypeNameText = "#text";

        /// <summary>
        ///     Gets a collection of flags that define specific behaviors for specific element nodes.
        ///     The table contains a DictionaryEntry list with the lowercase tag name as the Key, and a combination of
        ///     HtmlElementFlags as the Value.
        /// </summary>
        public static Dictionary<string, HtmlElementFlag> ElementsFlags;

        #endregion Static Members

        #region Constructors

        /// <summary>
        ///     Initialize HtmlNode. Builds a list of all tags that have special allowances
        /// </summary>
        static HtmlNode()
        {
            // tags whose content may be anything
            ElementsFlags = new Dictionary<string, HtmlElementFlag>(StringComparer.OrdinalIgnoreCase)
            {
                {"script", HtmlElementFlag.CData}, {"style", HtmlElementFlag.CData}, {"noxhtml", HtmlElementFlag.CData},
                {"textarea", HtmlElementFlag.CData},

                // tags that can not contain other tags
                {"base", HtmlElementFlag.Empty}, {"link", HtmlElementFlag.Empty}, {"meta", HtmlElementFlag.Empty},
                {"isindex", HtmlElementFlag.Empty}, {"hr", HtmlElementFlag.Empty}, {"col", HtmlElementFlag.Empty},
                {"img", HtmlElementFlag.Empty}, {"param", HtmlElementFlag.Empty}, {"embed", HtmlElementFlag.Empty},
                {"frame", HtmlElementFlag.Empty}, {"wbr", HtmlElementFlag.Empty}, {"bgsound", HtmlElementFlag.Empty},
                {"spacer", HtmlElementFlag.Empty}, {"keygen", HtmlElementFlag.Empty}, {"area", HtmlElementFlag.Empty},
                {"input", HtmlElementFlag.Empty}, {"basefont", HtmlElementFlag.Empty},
                {"source", HtmlElementFlag.Empty}, {"form", HtmlElementFlag.CanOverlap},

                //// they sometimes contain, and sometimes they don 't...
                //ElementsFlags.Add("option", HtmlElementFlag.Empty);

                // tag whose closing tag is equivalent to open tag:
                // <p>bla</p>bla will be transformed into <p>bla</p>bla
                // <p>bla<p>bla will be transformed into <p>bla<p>bla and not <p>bla></p><p>bla</p> or <p>bla<p>bla</p></p>
                //<br> see above
                {"br", HtmlElementFlag.Empty | HtmlElementFlag.Closed}
            };

            if (!HtmlDocument.DisableBehaviorTagP)
                ElementsFlags.Add("p", HtmlElementFlag.Empty | HtmlElementFlag.Closed);
        }

        /// <summary>
        ///     Initializes HtmlNode, providing type, owner and where it exists in a collection
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ownerdocument"></param>
        /// <param name="index"></param>
        public HtmlNode(HtmlNodeType type, HtmlDocument ownerdocument, int index)
        {
            _nodetype = type;
            _ownerdocument = ownerdocument;
            _outerstartindex = index;

            switch (type)
            {
                case HtmlNodeType.Comment:
                    Name = HtmlNodeTypeNameComment;
                    _endnode = this;
                    break;

                case HtmlNodeType.Document:
                    Name = HtmlNodeTypeNameDocument;
                    _endnode = this;
                    break;

                case HtmlNodeType.Text:
                    Name = HtmlNodeTypeNameText;
                    _endnode = this;
                    break;
            }

            if (_ownerdocument.Openednodes != null)
                if (!Closed)
                    // we use the index as the key

                    // -1 means the node comes from public
                    if (-1 != index)
                        _ownerdocument.Openednodes.Add(index, this);

            if (-1 != index || type == HtmlNodeType.Comment || type == HtmlNodeType.Text) return;

            // innerhtml and outerhtml must be calculated
            SetChanged();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///     Gets the collection of HTML attributes for this node. May not be null.
        /// </summary>
        public HtmlAttributeCollection Attributes
        {
            get
            {
                if (!HasAttributes) _attributes = new HtmlAttributeCollection(this);

                return _attributes;
            }
            internal set => _attributes = value;
        }

        /// <summary>
        ///     Gets all the children of the node.
        /// </summary>
        public HtmlNodeCollection ChildNodes
        {
            get => _childnodes ?? (_childnodes = new HtmlNodeCollection(this));
            internal set => _childnodes = value;
        }

        /// <summary>
        ///     Gets a value indicating if this node has been closed or not.
        /// </summary>
        public bool Closed => _endnode != null;

        /// <summary>
        ///     Gets the collection of HTML attributes for the closing tag. May not be null.
        /// </summary>
        public HtmlAttributeCollection ClosingAttributes =>
            !HasClosingAttributes ? new HtmlAttributeCollection(this) : _endnode.Attributes;

        /// <summary>
        ///     Gets the closing tag of the node, null if the node is self-closing.
        /// </summary>
        public HtmlNode EndNode => _endnode;

        /// <summary>
        ///     Gets the first child of the node.
        /// </summary>
        public HtmlNode FirstChild => !HasChildNodes ? null : _childnodes[0];

        /// <summary>
        ///     Gets a value indicating whether the current node has any attributes.
        /// </summary>
        public bool HasAttributes
        {
            get
            {
                if (_attributes == null) return false;

                if (_attributes.Count <= 0) return false;

                return true;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether this node has any child nodes.
        /// </summary>
        public bool HasChildNodes
        {
            get
            {
                if (_childnodes == null) return false;

                if (_childnodes.Count <= 0) return false;

                return true;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the current node has any attributes on the closing tag.
        /// </summary>
        public bool HasClosingAttributes
        {
            get
            {
                if (_endnode == null || _endnode == this) return false;

                if (_endnode._attributes == null) return false;

                if (_endnode._attributes.Count <= 0) return false;

                return true;
            }
        }

        /// <summary>
        ///     Gets or sets the value of the 'id' HTML attribute. The document must have been parsed using the
        ///     OptionUseIdAttribute set to true.
        /// </summary>
        public string Id
        {
            get
            {
                if (_ownerdocument.Nodesid == null) throw new Exception(HtmlDocument.HtmlExceptionUseIdAttributeFalse);

                return GetId();
            }
            set
            {
                if (_ownerdocument.Nodesid == null) throw new Exception(HtmlDocument.HtmlExceptionUseIdAttributeFalse);

                if (value == null) throw new ArgumentNullException("value");

                SetId(value);
            }
        }

        /// <summary>
        ///     Gets or Sets the HTML between the start and end tags of the object.
        /// </summary>
        public virtual string InnerHtml
        {
            get
            {
                if (_changed)
                {
                    UpdateHtml();
                    return _innerhtml;
                }

                if (_innerhtml != null) return _innerhtml;

                if (_innerstartindex < 0 || _innerlength < 0) return string.Empty;

                return _ownerdocument.Text.Substring(_innerstartindex, _innerlength);
            }
            set
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(value);

                RemoveAllChildren();
                AppendChildren(doc.DocumentNode.ChildNodes);
            }
        }

        /// <summary>
        ///     Gets or Sets the text between the start and end tags of the object.
        /// </summary>
        public virtual string InnerText
        {
            get
            {
                if (!_ownerdocument.BackwardCompatibility)
                {
                    if (HasChildNodes)
                    {
                        var sb = new StringBuilder();
                        AppendInnerText(sb);
                        return sb.ToString();
                    }

                    return GetCurrentNodeText();
                }

                switch (_nodetype)
                {
                    case HtmlNodeType.Text: return ((HtmlTextNode) this).Text;
                    // Don't display comment or comment child nodes
                    case HtmlNodeType.Comment: return "";
                }

                // note: right now, this method is *slow*, because we recompute everything.
                // it could be optimized like innerhtml
                if (!HasChildNodes) return string.Empty;

                string s = null;
                foreach (var node in ChildNodes) s += node.InnerText;

                return s;
            }
        }

        /// <summary>Gets direct inner text.</summary>
        /// <returns>The direct inner text.</returns>
        public virtual string GetDirectInnerText()
        {
            if (!_ownerdocument.BackwardCompatibility)
            {
                if (!HasChildNodes) return GetCurrentNodeText();

                var sb = new StringBuilder();
                AppendDirectInnerText(sb);
                return sb.ToString();
            }

            switch (_nodetype)
            {
                case HtmlNodeType.Text: return ((HtmlTextNode) this).Text;
                // Don't display comment or comment child nodes
                case HtmlNodeType.Comment: return "";
            }

            if (!HasChildNodes) return string.Empty;

            string s = null;
            foreach (var node in ChildNodes)
                if (node._nodetype == HtmlNodeType.Text)
                    s += ((HtmlTextNode) node).Text;

            return s;
        }

        internal string GetCurrentNodeText()
        {
            if (_nodetype != HtmlNodeType.Text) return "";

            var s = ((HtmlTextNode) this).Text;

            if (ParentNode.Name != "pre")
                // Make some test...
                s = s.Replace("\n", "")
                    .Replace("\r", "")
                    .Replace("\t", "");

            return s;
        }

        internal void AppendDirectInnerText(StringBuilder sb)
        {
            if (_nodetype == HtmlNodeType.Text) sb.Append(GetCurrentNodeText());

            if (!HasChildNodes) return;

            foreach (var node in ChildNodes) sb.Append(node.GetCurrentNodeText());
        }

        internal void AppendInnerText(StringBuilder sb)
        {
            if (_nodetype == HtmlNodeType.Text) sb.Append(GetCurrentNodeText());

            if (!HasChildNodes) return;

            foreach (var node in ChildNodes) node.AppendInnerText(sb);
        }

        /// <summary>
        ///     Gets the last child of the node.
        /// </summary>
        public HtmlNode LastChild => !HasChildNodes ? null : _childnodes[_childnodes.Count - 1];

        /// <summary>
        ///     Gets the line number of this node in the document.
        /// </summary>
        public int Line
        {
            get => _line;
            internal set => _line = value;
        }

        /// <summary>
        ///     Gets the column number of this node in the document.
        /// </summary>
        public int LinePosition
        {
            get => _lineposition;
            internal set => _lineposition = value;
        }

        /// <summary>
        ///     Gets the stream position of the area between the opening and closing tag of the node, relative to the start of the
        ///     document.
        /// </summary>
        public int InnerStartIndex => _innerstartindex;

        /// <summary>
        ///     Gets the length of the area between the opening and closing tag of the node.
        /// </summary>
        public int InnerLength => InnerHtml.Length;

        /// <summary>
        ///     Gets the length of the entire node, opening and closing tag included.
        /// </summary>
        public int OuterLength => OuterHtml.Length;

        /// <summary>
        ///     Gets or sets this node's name.
        /// </summary>
        public string Name
        {
            get
            {
                if (_optimizedName != null) return _optimizedName;

                if (OriginalName == null) Name = _ownerdocument.Text.Substring(_namestartindex, _namelength);

                _optimizedName = OriginalName == null ? string.Empty : OriginalName.ToLowerInvariant();

                return _optimizedName;
            }
            set
            {
                OriginalName = value;
                _optimizedName = null;
            }
        }

        /// <summary>
        ///     Gets the HTML node immediately following this element.
        /// </summary>
        public HtmlNode NextSibling
        {
            get => _nextnode;
            internal set => _nextnode = value;
        }

        /// <summary>
        ///     Gets the type of this node.
        /// </summary>
        public HtmlNodeType NodeType
        {
            get => _nodetype;
            internal set => _nodetype = value;
        }

        /// <summary>
        ///     The original unaltered name of the tag
        /// </summary>
        public string OriginalName { get; private set; }

        /// <summary>
        ///     Gets or Sets the object and its content in HTML.
        /// </summary>
        public virtual string OuterHtml
        {
            get
            {
                if (_changed)
                {
                    UpdateHtml();
                    return _outerhtml;
                }

                if (_outerhtml != null) return _outerhtml;

                if (_outerstartindex < 0 || _outerlength < 0) return string.Empty;

                return _ownerdocument.Text.Substring(_outerstartindex, _outerlength);
            }
        }

        /// <summary>
        ///     Gets the <see cref="HtmlDocument" /> to which this node belongs.
        /// </summary>
        public HtmlDocument OwnerDocument
        {
            get => _ownerdocument;
            internal set => _ownerdocument = value;
        }

        /// <summary>
        ///     Gets the parent of this node (for nodes that can have parents).
        /// </summary>
        public HtmlNode ParentNode
        {
            get => _parentnode;
            internal set => _parentnode = value;
        }

        /// <summary>
        ///     Gets the node immediately preceding this node.
        /// </summary>
        public HtmlNode PreviousSibling
        {
            get => _prevnode;
            internal set => _prevnode = value;
        }

        /// <summary>
        ///     Gets the stream position of this node in the document, relative to the start of the document.
        /// </summary>
        public int StreamPosition => _streamposition;

        /// <summary>
        ///     Gets a valid XPath string that points to this node
        /// </summary>
        public string XPath
        {
            get
            {
                var basePath = ParentNode == null || ParentNode.NodeType == HtmlNodeType.Document
                    ? "/"
                    : ParentNode.XPath + "/";
                return basePath + GetRelativeXpath();
            }
        }

        /// <summary>
        ///     The depth of the node relative to the opening root html element. This value is used to determine if a document has
        ///     to many nested html nodes which can cause stack overflows
        /// </summary>
        public int Depth { get; set; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        ///     Determines if an element node can be kept overlapped.
        /// </summary>
        /// <param name="name">The name of the element node to check. May not be <c>null</c>.</param>
        /// <returns>true if the name is the name of an element node that can be kept overlapped, <c>false</c> otherwise.</returns>
        public static bool CanOverlapElement(string name)
        {
            if (name == null) throw new ArgumentNullException("name");

            if (!ElementsFlags.TryGetValue(name, out var flag)) return false;

            return (flag & HtmlElementFlag.CanOverlap) != 0;
        }

        /// <summary>
        ///     Creates an HTML node from a string representing literal HTML.
        /// </summary>
        /// <param name="html">The HTML text.</param>
        /// <returns>The newly created node instance.</returns>
        public static HtmlNode CreateNode(string html)
        {
            // REVIEW: this is *not* optimum...
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            if (!doc.DocumentNode.IsSingleElementNode()) throw new Exception("Multiple node elments can't be created.");

            var element = doc.DocumentNode.FirstChild;

            while (element != null)
            {
                if (element.NodeType == HtmlNodeType.Element && element.OuterHtml != "\r\n") return element;

                element = element.NextSibling;
            }

            return doc.DocumentNode.FirstChild;
        }

        /// <summary>
        ///     Determines if an element node is a CDATA element node.
        /// </summary>
        /// <param name="name">The name of the element node to check. May not be null.</param>
        /// <returns>true if the name is the name of a CDATA element node, false otherwise.</returns>
        public static bool IsCDataElement(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            if (!ElementsFlags.TryGetValue(name, out var flag)) return false;

            return (flag & HtmlElementFlag.CData) != 0;
        }

        /// <summary>
        ///     Determines if an element node is closed.
        /// </summary>
        /// <param name="name">The name of the element node to check. May not be null.</param>
        /// <returns>true if the name is the name of a closed element node, false otherwise.</returns>
        public static bool IsClosedElement(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            if (!ElementsFlags.TryGetValue(name, out var flag)) return false;

            return (flag & HtmlElementFlag.Closed) != 0;
        }

        /// <summary>
        ///     Determines if an element node is defined as empty.
        /// </summary>
        /// <param name="name">The name of the element node to check. May not be null.</param>
        /// <returns>true if the name is the name of an empty element node, false otherwise.</returns>
        public static bool IsEmptyElement(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            if (name.Length == 0) return true;

            // <!DOCTYPE ...
            if ('!' == name[0]) return true;

            // <?xml ...
            if ('?' == name[0]) return true;

            if (!ElementsFlags.TryGetValue(name, out var flag)) return false;

            return (flag & HtmlElementFlag.Empty) != 0;
        }

        /// <summary>
        ///     Determines if a text corresponds to the closing tag of an node that can be kept overlapped.
        /// </summary>
        /// <param name="text">The text to check. May not be null.</param>
        /// <returns>true or false.</returns>
        public static bool IsOverlappedClosingElement(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            // min is </x>: 4
            if (text.Length <= 4) return false;

            if (text[0] != '<' || text[text.Length - 1] != '>' || text[1] != '/') return false;

            var name = text.Substring(2, text.Length - 3);
            return CanOverlapElement(name);
        }

        /// <summary>
        ///     Returns a collection of all ancestor nodes of this element.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HtmlNode> Ancestors()
        {
            var node = ParentNode;
            if (node == null) yield break;

            yield return node; //return the immediate parent node

            //now look at it's parent and walk up the tree of parents
            while (node.ParentNode != null)
            {
                yield return node.ParentNode;
                node = node.ParentNode;
            }
        }

        /// <summary>
        ///     Get Ancestors with matching name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<HtmlNode> Ancestors(string name)
        {
            for (var n = ParentNode; n != null; n = n.ParentNode)
                if (n.Name == name)
                    yield return n;
        }

        /// <summary>
        ///     Returns a collection of all ancestor nodes of this element.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HtmlNode> AncestorsAndSelf()
        {
            for (var n = this; n != null; n = n.ParentNode) yield return n;
        }

        /// <summary>
        ///     Gets all anscestor nodes and the current node
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<HtmlNode> AncestorsAndSelf(string name)
        {
            for (var n = this; n != null; n = n.ParentNode)
                if (n.Name == name)
                    yield return n;
        }

        /// <summary>
        ///     Adds the specified node to the end of the list of children of this node.
        /// </summary>
        /// <param name="newChild">The node to add. May not be null.</param>
        /// <returns>The node added.</returns>
        public HtmlNode AppendChild(HtmlNode newChild)
        {
            if (newChild == null) throw new ArgumentNullException(nameof(newChild));

            ChildNodes.Append(newChild);
            _ownerdocument.SetIdForNode(newChild, newChild.GetId());
            SetChildNodesId(newChild);

            SetChanged();
            return newChild;
        }

        /// <summary>Sets child nodes identifier.</summary>
        /// <param name="chilNode">The chil node.</param>
        public void SetChildNodesId(HtmlNode chilNode)
        {
            foreach (var child in chilNode.ChildNodes)
            {
                _ownerdocument.SetIdForNode(child, child.GetId());
                SetChildNodesId(child);
            }
        }

        /// <summary>
        ///     Adds the specified node to the end of the list of children of this node.
        /// </summary>
        /// <param name="newChildren">The node list to add. May not be null.</param>
        public void AppendChildren(HtmlNodeCollection newChildren)
        {
            if (newChildren == null) throw new ArgumentNullException("newChildren");

            foreach (var newChild in newChildren) AppendChild(newChild);
        }

        /// <summary>
        ///     Gets all Attributes with name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<HtmlAttribute> ChildAttributes(string name)
        {
            return Attributes.AttributesWithName(name);
        }

        /// <summary>
        ///     Creates a duplicate of the node
        /// </summary>
        /// <returns></returns>
        public HtmlNode Clone()
        {
            return CloneNode(true);
        }

        /// <summary>
        ///     Creates a duplicate of the node and changes its name at the same time.
        /// </summary>
        /// <param name="newName">The new name of the cloned node. May not be <c>null</c>.</param>
        /// <returns>The cloned node.</returns>
        public HtmlNode CloneNode(string newName)
        {
            return CloneNode(newName, true);
        }

        /// <summary>
        ///     Creates a duplicate of the node and changes its name at the same time.
        /// </summary>
        /// <param name="newName">The new name of the cloned node. May not be null.</param>
        /// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself.</param>
        /// <returns>The cloned node.</returns>
        public HtmlNode CloneNode(string newName, bool deep)
        {
            if (newName == null) throw new ArgumentNullException("newName");

            var node = CloneNode(deep);
            node.Name = newName;
            return node;
        }

        /// <summary>
        ///     Creates a duplicate of the node.
        /// </summary>
        /// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself.</param>
        /// <returns>The cloned node.</returns>
        public HtmlNode CloneNode(bool deep)
        {
            var node = _ownerdocument.CreateNode(_nodetype);
            node.Name = OriginalName;

            switch (_nodetype)
            {
                case HtmlNodeType.Comment:
                    ((HtmlCommentNode) node).Comment = ((HtmlCommentNode) this).Comment;
                    return node;

                case HtmlNodeType.Text:
                    ((HtmlTextNode) node).Text = ((HtmlTextNode) this).Text;
                    return node;
            }

            // attributes
            if (HasAttributes)
                foreach (var att in _attributes)
                {
                    var newatt = att.Clone();
                    node.Attributes.Append(newatt);
                }

            // closing attributes
            if (HasClosingAttributes)
            {
                node._endnode = _endnode.CloneNode(false);
                foreach (var att in _endnode._attributes)
                {
                    var newatt = att.Clone();
                    node._endnode._attributes.Append(newatt);
                }
            }

            if (!deep) return node;

            if (!HasChildNodes) return node;

            // child nodes
            foreach (var child in _childnodes)
            {
                var newchild = child.CloneNode(deep);
                node.AppendChild(newchild);
            }

            return node;
        }

        /// <summary>
        ///     Creates a duplicate of the node and the subtree under it.
        /// </summary>
        /// <param name="node">The node to duplicate. May not be <c>null</c>.</param>
        public void CopyFrom(HtmlNode node)
        {
            CopyFrom(node, true);
        }

        /// <summary>
        ///     Creates a duplicate of the node.
        /// </summary>
        /// <param name="node">The node to duplicate. May not be <c>null</c>.</param>
        /// <param name="deep">true to recursively clone the subtree under the specified node, false to clone only the node itself.</param>
        public void CopyFrom(HtmlNode node, bool deep)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            Attributes.RemoveAll();
            if (node.HasAttributes)
                foreach (var att in node.Attributes)
                {
                    var newatt = att.Clone();
                    Attributes.Append(newatt);
                }

            if (!deep) return;

            RemoveAllChildren();
            if (!node.HasChildNodes) return;

            foreach (var child in node.ChildNodes) AppendChild(child.CloneNode(true));
        }

        /// <summary>
        ///     Gets all Descendant nodes for this node and each of child nodes
        /// </summary>
        /// <param name="level">The depth level of the node to parse in the html tree</param>
        /// <returns>the current element as an HtmlNode</returns>
        [Obsolete("Use Descendants() instead, the results of this function will change in a future version")]
        public IEnumerable<HtmlNode> DescendantNodes(int level = 0)
        {
            if (level > HtmlDocument.MaxDepthLevel) throw new ArgumentException(DepthLevelExceptionMessage);

            foreach (var node in ChildNodes)
            {
                yield return node;

                foreach (var descendant in node.DescendantNodes(level + 1)) yield return descendant;
            }
        }

        /// <summary>
        ///     Returns a collection of all descendant nodes of this element, in document order
        /// </summary>
        /// <returns></returns>
        [Obsolete(
            "Use DescendantsAndSelf() instead, the results of this function will change in a future version")]
        public IEnumerable<HtmlNode> DescendantNodesAndSelf()
        {
            return DescendantsAndSelf();
        }

        /// <summary>
        ///     Gets all Descendant nodes in enumerated list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HtmlNode> Descendants()
        {
            return Descendants(0);
        }

        /// <summary>
        ///     Gets all Descendant nodes in enumerated list
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HtmlNode> Descendants(int level)
        {
            if (level > HtmlDocument.MaxDepthLevel) throw new ArgumentException(DepthLevelExceptionMessage);

            foreach (var node in ChildNodes)
            {
                yield return node;

                foreach (var descendant in node.Descendants(level + 1)) yield return descendant;
            }
        }

        /// <summary>
        ///     Get all descendant nodes with matching name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<HtmlNode> Descendants(string name)
        {
            foreach (var node in Descendants())
                if (string.Equals(node.Name, name, StringComparison.OrdinalIgnoreCase))
                    yield return node;
        }

        /// <summary>
        ///     Returns a collection of all descendant nodes of this element, in document order
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HtmlNode> DescendantsAndSelf()
        {
            yield return this;

            foreach (var n in Descendants())
            {
                var el = n;
                if (el != null) yield return el;
            }
        }

        /// <summary>
        ///     Gets all descendant nodes including this node
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<HtmlNode> DescendantsAndSelf(string name)
        {
            yield return this;

            foreach (var node in Descendants())
                if (node.Name == name)
                    yield return node;
        }

        /// <summary>
        ///     Gets first generation child node matching name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public HtmlNode Element(string name)
        {
            foreach (var node in ChildNodes)
                if (node.Name == name)
                    return node;

            return null;
        }

        /// <summary>
        ///     Gets matching first generation child nodes matching name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<HtmlNode> Elements(string name)
        {
            foreach (var node in ChildNodes)
                if (node.Name == name)
                    yield return node;
        }

        /// <summary>
        ///     Helper method to get the value of an attribute of this node. If the attribute is not found, the default value will
        ///     be returned.
        /// </summary>
        /// <param name="name">The name of the attribute to get. May not be <c>null</c>.</param>
        /// <param name="def">The default value to return if not found.</param>
        /// <returns>The value of the attribute if found, the default value if not found.</returns>
        public string GetAttributeValue(string name, string def)
        {
            if (name == null) throw new ArgumentNullException("name");

            if (!HasAttributes) return def;

            var att = Attributes[name];
            if (att == null) return def;

            return att.Value;
        }

        /// <summary>
        ///     Helper method to get the value of an attribute of this node. If the attribute is not found, the default value will
        ///     be returned.
        /// </summary>
        /// <param name="name">The name of the attribute to get. May not be <c>null</c>.</param>
        /// <param name="def">The default value to return if not found.</param>
        /// <returns>The value of the attribute if found, the default value if not found.</returns>
        public int GetAttributeValue(string name, int def)
        {
            if (name == null) throw new ArgumentNullException("name");

            if (!HasAttributes) return def;

            var att = Attributes[name];
            if (att == null) return def;

            try
            {
                return Convert.ToInt32(att.Value);
            }
            catch
            {
                return def;
            }
        }

        /// <summary>
        ///     Helper method to get the value of an attribute of this node. If the attribute is not found, the default value will
        ///     be returned.
        /// </summary>
        /// <param name="name">The name of the attribute to get. May not be <c>null</c>.</param>
        /// <param name="def">The default value to return if not found.</param>
        /// <returns>The value of the attribute if found, the default value if not found.</returns>
        public bool GetAttributeValue(string name, bool def)
        {
            if (name == null) throw new ArgumentNullException("name");

            if (!HasAttributes) return def;

            var att = Attributes[name];
            if (att == null) return def;

            try
            {
                return Convert.ToBoolean(att.Value);
            }
            catch
            {
                return def;
            }
        }

        /// <summary>
        ///     Inserts the specified node immediately after the specified reference node.
        /// </summary>
        /// <param name="newChild">The node to insert. May not be <c>null</c>.</param>
        /// <param name="refChild">The node that is the reference node. The newNode is placed after the refNode.</param>
        /// <returns>The node being inserted.</returns>
        public HtmlNode InsertAfter(HtmlNode newChild, HtmlNode refChild)
        {
            if (newChild == null) throw new ArgumentNullException("newChild");

            if (refChild == null) return PrependChild(newChild);

            if (newChild == refChild) return newChild;

            var index = -1;

            if (_childnodes != null) index = _childnodes[refChild];

            if (index == -1) throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);

            if (_childnodes != null) _childnodes.Insert(index + 1, newChild);

            _ownerdocument.SetIdForNode(newChild, newChild.GetId());
            SetChildNodesId(newChild);
            SetChanged();
            return newChild;
        }

        /// <summary>
        ///     Inserts the specified node immediately before the specified reference node.
        /// </summary>
        /// <param name="newChild">The node to insert. May not be <c>null</c>.</param>
        /// <param name="refChild">The node that is the reference node. The newChild is placed before this node.</param>
        /// <returns>The node being inserted.</returns>
        public HtmlNode InsertBefore(HtmlNode newChild, HtmlNode refChild)
        {
            if (newChild == null) throw new ArgumentNullException("newChild");

            if (refChild == null) return AppendChild(newChild);

            if (newChild == refChild) return newChild;

            var index = -1;

            if (_childnodes != null) index = _childnodes[refChild];

            if (index == -1) throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);

            if (_childnodes != null) _childnodes.Insert(index, newChild);

            _ownerdocument.SetIdForNode(newChild, newChild.GetId());
            SetChildNodesId(newChild);
            SetChanged();
            return newChild;
        }

        /// <summary>
        ///     Adds the specified node to the beginning of the list of children of this node.
        /// </summary>
        /// <param name="newChild">The node to add. May not be <c>null</c>.</param>
        /// <returns>The node added.</returns>
        public HtmlNode PrependChild(HtmlNode newChild)
        {
            if (newChild == null) throw new ArgumentNullException("newChild");

            ChildNodes.Prepend(newChild);
            _ownerdocument.SetIdForNode(newChild, newChild.GetId());
            SetChildNodesId(newChild);
            SetChanged();
            return newChild;
        }

        /// <summary>
        ///     Adds the specified node list to the beginning of the list of children of this node.
        /// </summary>
        /// <param name="newChildren">The node list to add. May not be <c>null</c>.</param>
        public void PrependChildren(HtmlNodeCollection newChildren)
        {
            if (newChildren == null) throw new ArgumentNullException("newChildren");

            for (var i = newChildren.Count - 1; i >= 0; i--) PrependChild(newChildren[i]);
        }

        /// <summary>
        ///     Removes node from parent collection
        /// </summary>
        public void Remove()
        {
            if (ParentNode != null) ParentNode.ChildNodes.Remove(this);
        }

        /// <summary>
        ///     Removes all the children and/or attributes of the current node.
        /// </summary>
        public void RemoveAll()
        {
            RemoveAllChildren();

            if (HasAttributes) _attributes.Clear();

            if (_endnode != null && _endnode != this)
                if (_endnode._attributes != null)
                    _endnode._attributes.Clear();

            SetChanged();
        }

        /// <summary>
        ///     Removes all the children of the current node.
        /// </summary>
        public void RemoveAllChildren()
        {
            if (!HasChildNodes) return;

            if (_ownerdocument.OptionUseIdAttribute)
                // remove nodes from id list
                foreach (var node in _childnodes)
                {
                    _ownerdocument.SetIdForNode(null, node.GetId());
                    RemoveAllIDforNode(node);
                }

            _childnodes.Clear();
            SetChanged();
        }

        /// <summary>Removes all id for node described by node.</summary>
        /// <param name="node">The node.</param>
        public void RemoveAllIDforNode(HtmlNode node)
        {
            foreach (var nodeChildNode in node.ChildNodes)
            {
                _ownerdocument.SetIdForNode(null, nodeChildNode.GetId());
                RemoveAllIDforNode(nodeChildNode);
            }
        }

        /// <summary>
        ///     Removes the specified child node.
        /// </summary>
        /// <param name="oldChild">The node being removed. May not be <c>null</c>.</param>
        /// <returns>The node removed.</returns>
        public HtmlNode RemoveChild(HtmlNode oldChild)
        {
            if (oldChild == null) throw new ArgumentNullException("oldChild");

            var index = -1;

            if (_childnodes != null) index = _childnodes[oldChild];

            if (index == -1) throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);

            if (_childnodes != null) _childnodes.Remove(index);

            _ownerdocument.SetIdForNode(null, oldChild.GetId());
            RemoveAllIDforNode(oldChild);
            SetChanged();
            return oldChild;
        }

        /// <summary>
        ///     Removes the specified child node.
        /// </summary>
        /// <param name="oldChild">The node being removed. May not be <c>null</c>.</param>
        /// <param name="keepGrandChildren">true to keep grand children of the node, false otherwise.</param>
        /// <returns>The node removed.</returns>
        public HtmlNode RemoveChild(HtmlNode oldChild, bool keepGrandChildren)
        {
            if (oldChild == null) throw new ArgumentNullException("oldChild");

            if (oldChild._childnodes != null && keepGrandChildren)
            {
                // get prev sibling
                var prev = oldChild.PreviousSibling;

                // reroute grand children to ourselves
                foreach (var grandchild in oldChild._childnodes) prev = InsertAfter(grandchild, prev);
            }

            RemoveChild(oldChild);
            SetChanged();
            return oldChild;
        }

        /// <summary>
        ///     Replaces the child node oldChild with newChild node.
        /// </summary>
        /// <param name="newChild">The new node to put in the child list.</param>
        /// <param name="oldChild">The node being replaced in the list.</param>
        /// <returns>The node replaced.</returns>
        public HtmlNode ReplaceChild(HtmlNode newChild, HtmlNode oldChild)
        {
            if (newChild == null) return RemoveChild(oldChild);

            if (oldChild == null) return AppendChild(newChild);

            var index = -1;

            if (_childnodes != null) index = _childnodes[oldChild];

            if (index == -1) throw new ArgumentException(HtmlDocument.HtmlExceptionRefNotChild);

            if (_childnodes != null) _childnodes.Replace(index, newChild);

            _ownerdocument.SetIdForNode(null, oldChild.GetId());
            RemoveAllIDforNode(oldChild);

            _ownerdocument.SetIdForNode(newChild, newChild.GetId());
            SetChildNodesId(newChild);

            SetChanged();
            return newChild;
        }

        /// <summary>
        ///     Helper method to set the value of an attribute of this node. If the attribute is not found, it will be created
        ///     automatically.
        /// </summary>
        /// <param name="name">The name of the attribute to set. May not be null.</param>
        /// <param name="value">The value for the attribute.</param>
        /// <returns>The corresponding attribute instance.</returns>
        public HtmlAttribute SetAttributeValue(string name, string value)
        {
            if (name == null) throw new ArgumentNullException("name");

            var att = Attributes[name];
            if (att == null) return Attributes.Append(_ownerdocument.CreateAttribute(name, value));

            att.Value = value;
            return att;
        }

        /// <summary>
        ///     Saves all the children of the node to the specified TextWriter.
        /// </summary>
        /// <param name="outText">The TextWriter to which you want to save.</param>
        /// <param name="level">Identifies the level we are in starting at root with 0</param>
        public void WriteContentTo(TextWriter outText, int level = 0)
        {
            if (level > HtmlDocument.MaxDepthLevel) throw new ArgumentException(DepthLevelExceptionMessage);

            if (_childnodes == null) return;

            foreach (var node in _childnodes) node.WriteTo(outText, level + 1);
        }

        /// <summary>
        ///     Saves all the children of the node to a string.
        /// </summary>
        /// <returns>The saved string.</returns>
        public string WriteContentTo()
        {
            var sw = new StringWriter();
            WriteContentTo(sw);
            sw.Flush();
            return sw.ToString();
        }

        /// <summary>
        ///     Saves the current node to the specified TextWriter.
        /// </summary>
        /// <param name="outText">The TextWriter to which you want to save.</param>
        /// <param name="level">identifies the level we are in starting at root with 0</param>
        public virtual void WriteTo(TextWriter outText, int level = 0)
        {
            string html;
            switch (_nodetype)
            {
                case HtmlNodeType.Comment:
                    html = ((HtmlCommentNode) this).Comment;
                    if (_ownerdocument.OptionOutputAsXml)
                    {
                        var commentNode = (HtmlCommentNode) this;
                        if (!_ownerdocument.BackwardCompatibility &&
                            commentNode.Comment.ToLowerInvariant().StartsWith("<!doctype"))
                            outText.Write(commentNode.Comment);
                        else
                            outText.Write("<!--" + GetXmlComment(commentNode) + " -->");
                    }
                    else
                    {
                        outText.Write(html);
                    }

                    break;

                case HtmlNodeType.Document:
                    if (_ownerdocument.OptionOutputAsXml)
                    {
#if SILVERLIGHT || PocketPC || METRO || NETSTANDARD1_3 || NETSTANDARD1_6
						outText.Write("<?xml version=\"1.0\" encoding=\"" + _ownerdocument.GetOutEncoding().WebName + "\"?>");
#else
                        outText.Write("<?xml version=\"1.0\" encoding=\"" + _ownerdocument.GetOutEncoding().BodyName +
                                      "\"?>");
#endif
                        // check there is a root element
                        if (_ownerdocument.DocumentNode.HasChildNodes)
                        {
                            var rootnodes = _ownerdocument.DocumentNode._childnodes.Count;
                            if (rootnodes > 0)
                            {
                                var xml = _ownerdocument.GetXmlDeclaration();
                                if (xml != null) rootnodes--;

                                if (rootnodes > 1)
                                {
                                    if (!_ownerdocument.BackwardCompatibility)
                                    {
                                        WriteContentTo(outText, level);
                                    }
                                    else
                                    {
                                        if (_ownerdocument.OptionOutputUpperCase)
                                        {
                                            outText.Write("<SPAN>");
                                            WriteContentTo(outText, level);
                                            outText.Write("</SPAN>");
                                        }
                                        else
                                        {
                                            outText.Write("<span>");
                                            WriteContentTo(outText, level);
                                            outText.Write("</span>");
                                        }
                                    }

                                    break;
                                }
                            }
                        }
                    }

                    WriteContentTo(outText, level);
                    break;

                case HtmlNodeType.Text:
                    html = ((HtmlTextNode) this).Text;
                    outText.Write(_ownerdocument.OptionOutputAsXml
                        ? HtmlDocument.HtmlEncodeWithCompatibility(html, _ownerdocument.BackwardCompatibility)
                        : html);
                    break;

                case HtmlNodeType.Element:
                    var name = _ownerdocument.OptionOutputUpperCase ? Name.ToUpperInvariant() : Name;

                    if (_ownerdocument.OptionOutputOriginalCase) name = OriginalName;

                    if (_ownerdocument.OptionOutputAsXml)
                    {
                        if (name.Length > 0)
                        {
                            if (name[0] == '?')
                                // forget this one, it's been done at the document level
                                break;

                            if (name.Trim().Length == 0) break;

                            name = HtmlDocument.GetXmlName(name, false, _ownerdocument.OptionPreserveXmlNamespaces);
                        }
                        else
                        {
                            break;
                        }
                    }

                    outText.Write("<" + name);
                    WriteAttributes(outText, false);

                    if (HasChildNodes)
                    {
                        outText.Write(">");
                        var cdata = false;
                        if (_ownerdocument.OptionOutputAsXml && IsCDataElement(Name))
                        {
                            // this code and the following tries to output things as nicely as possible for old browsers.
                            cdata = true;
                            outText.Write("\r\n//<![CDATA[\r\n");
                        }

                        if (cdata)
                        {
                            if (HasChildNodes)
                                // child must be a text
                                ChildNodes[0].WriteTo(outText, level);

                            outText.Write("\r\n//]]>//\r\n");
                        }
                        else
                        {
                            WriteContentTo(outText, level);
                        }

                        if (_ownerdocument.OptionOutputAsXml || !_isImplicitEnd)
                        {
                            outText.Write("</" + name);
                            if (!_ownerdocument.OptionOutputAsXml) WriteAttributes(outText, true);

                            outText.Write(">");
                        }
                    }
                    else
                    {
                        if (IsEmptyElement(Name))
                        {
                            if (_ownerdocument.OptionWriteEmptyNodes || _ownerdocument.OptionOutputAsXml)
                            {
                                outText.Write(" />");
                            }
                            else
                            {
                                if (Name.Length > 0 && Name[0] == '?') outText.Write("?");

                                outText.Write(">");
                            }
                        }
                        else
                        {
                            if (!_isImplicitEnd)
                                outText.Write("></" + name + ">");
                            else
                                outText.Write(">");
                        }
                    }

                    break;
            }
        }

        /// <summary>
        ///     Saves the current node to the specified XmlWriter.
        /// </summary>
        /// <param name="writer">The XmlWriter to which you want to save.</param>
        public void WriteTo(XmlWriter writer)
        {
            switch (_nodetype)
            {
                case HtmlNodeType.Comment:
                    writer.WriteComment(GetXmlComment((HtmlCommentNode) this));
                    break;

                case HtmlNodeType.Document:
#if SILVERLIGHT || PocketPC || METRO || NETSTANDARD1_3 || NETSTANDARD1_6
					writer.WriteProcessingInstruction("xml",
													  "version=\"1.0\" encoding=\"" +
													  _ownerdocument.GetOutEncoding().WebName + "\"");
#else
                    writer.WriteProcessingInstruction("xml",
                        "version=\"1.0\" encoding=\"" + _ownerdocument.GetOutEncoding().BodyName + "\"");
#endif

                    if (HasChildNodes)
                        foreach (var subnode in ChildNodes)
                            subnode.WriteTo(writer);

                    break;

                case HtmlNodeType.Text:
                    var html = ((HtmlTextNode) this).Text;
                    writer.WriteString(html);
                    break;

                case HtmlNodeType.Element:
                    var name = _ownerdocument.OptionOutputUpperCase ? Name.ToUpperInvariant() : Name;

                    if (_ownerdocument.OptionOutputOriginalCase) name = OriginalName;

                    writer.WriteStartElement(name);
                    WriteAttributes(writer, this);

                    if (HasChildNodes)
                        foreach (var subnode in ChildNodes)
                            subnode.WriteTo(writer);

                    writer.WriteEndElement();
                    break;
            }
        }

        /// <summary>
        ///     Saves the current node to a string.
        /// </summary>
        /// <returns>The saved string.</returns>
        public string WriteTo()
        {
            using (var sw = new StringWriter())
            {
                WriteTo(sw);
                sw.Flush();
                return sw.ToString();
            }
        }

        /// <summary>
        ///     Sets the parent Html node and properly determines the current node's depth using the parent node's depth.
        /// </summary>
        public void SetParent(HtmlNode parent)
        {
            if (parent == null) return;

            ParentNode = parent;
            if (OwnerDocument.OptionMaxNestedChildNodes > 0)
            {
                Depth = parent.Depth + 1;
                if (Depth > OwnerDocument.OptionMaxNestedChildNodes)
                    throw new Exception(
                        string.Format(
                            "Document has more than {0} nested tags. This is likely due to the page not closing tags properly.",
                            OwnerDocument.OptionMaxNestedChildNodes));
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal void SetChanged()
        {
            _changed = true;
            if (ParentNode != null) ParentNode.SetChanged();
        }

        private void UpdateHtml()
        {
            _innerhtml = WriteContentTo();
            _outerhtml = WriteTo();
            _changed = false;
        }

        internal static string GetXmlComment(HtmlCommentNode comment)
        {
            var s = comment.Comment;
            s = s.Substring(4, s.Length - 7).Replace("--", " - -");
            return s;
        }

        internal static void WriteAttributes(XmlWriter writer, HtmlNode node)
        {
            if (!node.HasAttributes) return;

            // we use Hashitems to make sure attributes are written only once
            foreach (var att in node.Attributes.Hashitems.Values) writer.WriteAttributeString(att.XmlName, att.Value);
        }

        internal void UpdateLastNode()
        {
            HtmlNode newLast = null;
            if (_prevwithsamename == null || !_prevwithsamename._starttag)
                foreach (var openNode in _ownerdocument.Openednodes)
                    if ((openNode.Key < _outerstartindex || openNode.Key > _outerstartindex + _outerlength) &&
                        openNode.Value.OriginalName == OriginalName)
                    {
                        if (newLast == null && openNode.Value._starttag)
                            newLast = openNode.Value;
                        else if (newLast != null && newLast.InnerStartIndex < openNode.Key && openNode.Value._starttag)
                            newLast = openNode.Value;
                    }
            else
                newLast = _prevwithsamename;

            if (newLast != null) _ownerdocument.Lastnodes[newLast.Name] = newLast;
        }

        internal void CloseNode(HtmlNode endnode, int level = 0)
        {
            if (level > HtmlDocument.MaxDepthLevel) throw new ArgumentException(DepthLevelExceptionMessage);

            if (!_ownerdocument.OptionAutoCloseOnEnd)
                // close all children
                if (_childnodes != null)
                    foreach (var child in _childnodes)
                    {
                        if (child.Closed) continue;

                        // create a fake closer node
                        var close = new HtmlNode(NodeType, _ownerdocument, -1);
                        close._endnode = close;
                        child.CloseNode(close, level + 1);
                    }

            if (!Closed)
            {
                _endnode = endnode;

                if (_ownerdocument.Openednodes != null) _ownerdocument.Openednodes.Remove(_outerstartindex);

                var self = Utilities.GetDictionaryValueOrDefault(_ownerdocument.Lastnodes, Name);
                if (self == this)
                {
                    _ownerdocument.Lastnodes.Remove(Name);
                    _ownerdocument.UpdateLastParentNode();

                    if (_starttag && !string.IsNullOrEmpty(Name)) UpdateLastNode();
                }

                if (endnode == this) return;

                // create an inner section
                _innerstartindex = _outerstartindex + _outerlength;
                _innerlength = endnode._outerstartindex - _innerstartindex;

                // update full length
                _outerlength = endnode._outerstartindex + endnode._outerlength - _outerstartindex;
            }
        }

        internal string GetId()
        {
            var att = Attributes["id"];
            return att == null ? string.Empty : att.Value;
        }

        internal void SetId(string id)
        {
            var att = Attributes["id"] ?? _ownerdocument.CreateAttribute("id");
            att.Value = id;
            _ownerdocument.SetIdForNode(this, att.Value);
            Attributes.Add(att);
            SetChanged();
        }

        internal void WriteAttribute(TextWriter outText, HtmlAttribute att)
        {
            if (att.Value == null)
                // null value attribute are not written
                return;

            string name;
            var quote = att.QuoteType == AttributeValueQuote.DoubleQuote ? "\"" : "'";
            if (_ownerdocument.OptionOutputAsXml)
            {
                name = _ownerdocument.OptionOutputUpperCase ? att.XmlName.ToUpperInvariant() : att.XmlName;
                if (_ownerdocument.OptionOutputOriginalCase) name = att.OriginalName;

                outText.Write(" " + name + "=" + quote +
                              HtmlDocument.HtmlEncodeWithCompatibility(att.XmlValue,
                                  _ownerdocument.BackwardCompatibility) + quote);
            }
            else
            {
                name = _ownerdocument.OptionOutputUpperCase ? att.Name.ToUpperInvariant() : att.Name;
                if (_ownerdocument.OptionOutputOriginalCase) name = att.OriginalName;

                if (att.Name.Length >= 4)
                    if (att.Name[0] == '<' && att.Name[1] == '%' && att.Name[att.Name.Length - 1] == '>' &&
                        att.Name[att.Name.Length - 2] == '%')
                    {
                        outText.Write(" " + name);
                        return;
                    }

                if (_ownerdocument.OptionOutputOptimizeAttributeValues)
                {
                    if (att.Value.IndexOfAny(new[] {(char) 10, (char) 13, (char) 9, ' '}) < 0)
                        outText.Write(" " + name + "=" + att.Value);
                    else
                        outText.Write(" " + name + "=" + quote + att.Value + quote);
                }
                else
                {
                    outText.Write(" " + name + "=" + quote + att.Value + quote);
                }
            }
        }

        internal void WriteAttributes(TextWriter outText, bool closing)
        {
            if (_ownerdocument.OptionOutputAsXml)
            {
                if (_attributes == null) return;

                // we use Hashitems to make sure attributes are written only once
                foreach (var att in _attributes.Hashitems.Values) WriteAttribute(outText, att);

                return;
            }

            if (!closing)
            {
                if (_attributes != null)
                    foreach (var att in _attributes)
                        WriteAttribute(outText, att);

                if (!_ownerdocument.OptionAddDebuggingAttributes) return;

                WriteAttribute(outText, _ownerdocument.CreateAttribute("_closed", Closed.ToString()));
                WriteAttribute(outText, _ownerdocument.CreateAttribute("_children", ChildNodes.Count.ToString()));

                var i = 0;
                foreach (var n in ChildNodes)
                {
                    WriteAttribute(outText, _ownerdocument.CreateAttribute("_child_" + i, n.Name));
                    i++;
                }
            }
            else
            {
                if (_endnode == null || _endnode._attributes == null || _endnode == this) return;

                foreach (var att in _endnode._attributes) WriteAttribute(outText, att);

                if (!_ownerdocument.OptionAddDebuggingAttributes) return;

                WriteAttribute(outText, _ownerdocument.CreateAttribute("_closed", Closed.ToString()));
                WriteAttribute(outText, _ownerdocument.CreateAttribute("_children", ChildNodes.Count.ToString()));
            }
        }

        #endregion Internal Methods

        #region Private Methods

        private string GetRelativeXpath()
        {
            if (ParentNode == null) return Name;

            if (NodeType == HtmlNodeType.Document) return string.Empty;

            var i = 1;
            foreach (var node in ParentNode.ChildNodes)
            {
                if (node.Name != Name) continue;

                if (node == this) break;

                i++;
            }

            return Name + "[" + i + "]";
        }

        private bool IsSingleElementNode()
        {
            var count = 0;
            var element = FirstChild;

            while (element != null)
            {
                if (element.NodeType == HtmlNodeType.Element && element.OuterHtml != "\r\n") count++;

                element = element.NextSibling;
            }

            return count <= 1 ? true : false;
        }

        #endregion Private Methods

        #region Class Helper

        /// <summary>
        ///     Adds one or more classes to this node.
        /// </summary>
        /// <param name="name">The node list to add. May not be null.</param>
        public void AddClass(string name)
        {
            AddClass(name, false);
        }

        /// <summary>
        ///     Adds one or more classes to this node.
        /// </summary>
        /// <param name="name">The node list to add. May not be null.</param>
        /// <param name="throwError">true to throw Error if class name exists, false otherwise.</param>
        public void AddClass(string name, bool throwError)
        {
            var classAttributes = Attributes.AttributesWithName("class");

            var htmlAttributes = classAttributes.ToList();
            if (!IsEmpty(htmlAttributes))
            {
                foreach (var att in htmlAttributes)
                    if (att.Value.Equals(name) || att.Value.Contains(name))
                    {
                        if (throwError) throw new Exception(HtmlDocument.HtmlExceptionClassExists);
                    }
                    else
                    {
                        SetAttributeValue(att.Name, att.Value + " " + name);
                    }
            }
            else
            {
                var attribute = _ownerdocument.CreateAttribute("class", name);
                Attributes.Append(attribute);
            }
        }

        /// <summary>
        ///     Removes the class attribute from the node.
        /// </summary>
        public void RemoveClass()
        {
            RemoveClass(false);
        }

        /// <summary>
        ///     Removes the class attribute from the node.
        /// </summary>
        /// <param name="throwError">true to throw Error if class name doesn't exist, false otherwise.</param>
        public void RemoveClass(bool throwError)
        {
            var classAttributes = Attributes.AttributesWithName("class");
            var htmlAttributes = classAttributes.ToList();
            if (IsEmpty(htmlAttributes) && throwError) throw new Exception(HtmlDocument.HtmlExceptionClassDoesNotExist);

            foreach (var att in htmlAttributes) Attributes.Remove(att);
        }

        /// <summary>
        ///     Removes the specified class from the node.
        /// </summary>
        /// <param name="name">The class being removed. May not be <c>null</c>.</param>
        public void RemoveClass(string name)
        {
            RemoveClass(name, false);
        }

        /// <summary>
        ///     Removes the specified class from the node.
        /// </summary>
        /// <param name="name">The class being removed. May not be <c>null</c>.</param>
        /// <param name="throwError">true to throw Error if class name doesn't exist, false otherwise.</param>
        public void RemoveClass(string name, bool throwError)
        {
            var classAttributes = Attributes.AttributesWithName("class");
            var htmlAttributes = classAttributes.ToList();
            if (IsEmpty(htmlAttributes) && throwError) throw new Exception(HtmlDocument.HtmlExceptionClassDoesNotExist);

            foreach (var att in htmlAttributes)
            {
                if (att.Value == null) continue;

                if (att.Value.Equals(name))
                {
                    Attributes.Remove(att);
                }
                else if (att.Value.Contains(name))
                {
                    var classNames = att.Value.Split(' ');

                    var newClassNames = "";

                    foreach (var item in classNames)
                        if (!item.Equals(name))
                            newClassNames += item + " ";

                    newClassNames = newClassNames.Trim();
                    SetAttributeValue(att.Name, newClassNames);
                }
                else
                {
                    if (throwError) throw new Exception(HtmlDocument.HtmlExceptionClassDoesNotExist);
                }

                if (string.IsNullOrEmpty(att.Value)) Attributes.Remove(att);
            }
        }

        /// <summary>
        ///     Replaces the class name oldClass with newClass name.
        /// </summary>
        /// <param name="newClass">The new class name.</param>
        /// <param name="oldClass">The class being replaced.</param>
        public void ReplaceClass(string newClass, string oldClass)
        {
            ReplaceClass(newClass, oldClass, false);
        }

        /// <summary>
        ///     Replaces the class name oldClass with newClass name.
        /// </summary>
        /// <param name="newClass">The new class name.</param>
        /// <param name="oldClass">The class being replaced.</param>
        /// <param name="throwError">true to throw Error if class name doesn't exist, false otherwise.</param>
        public void ReplaceClass(string newClass, string oldClass, bool throwError)
        {
            if (string.IsNullOrEmpty(newClass)) RemoveClass(oldClass);

            if (string.IsNullOrEmpty(oldClass)) AddClass(newClass);

            var classAttributes = Attributes.AttributesWithName("class");

            var htmlAttributes = classAttributes.ToList();
            if (IsEmpty(htmlAttributes) && throwError) throw new Exception(HtmlDocument.HtmlExceptionClassDoesNotExist);

            foreach (var att in htmlAttributes)
            {
                if (att.Value == null) continue;

                if (att.Value.Equals(oldClass) || att.Value.Contains(oldClass))
                {
                    var newClassNames = att.Value.Replace(oldClass, newClass);
                    SetAttributeValue(att.Name, newClassNames);
                }
                else if (throwError)
                {
                    throw new Exception(HtmlDocument.HtmlExceptionClassDoesNotExist);
                }
            }
        }

        /// <summary>Gets the CSS Class from the node.</summary>
        /// <returns>
        ///     The CSS Class from the node
        /// </returns>
        public IEnumerable<string> GetClasses()
        {
            var classAttributes = Attributes.AttributesWithName("class");

            foreach (var att in classAttributes)
            {
                var classNames = att.Value.Split(null as char[], StringSplitOptions.RemoveEmptyEntries);

                foreach (var className in classNames) yield return className;
            }
        }

        /// <summary>Check if the node class has the parameter class.</summary>
        /// <param name="className">The class.</param>
        /// <returns>True if node class has the parameter class, false if not.</returns>
        public bool HasClass(string className)
        {
            var classes = GetClasses();

            foreach (var @class in classes)
            {
                var classNames = @class.Split(null as char[], StringSplitOptions.RemoveEmptyEntries);
                foreach (var theClassName in classNames)
                    if (theClassName == className)
                        return true;
            }

            return false;
        }

        private bool IsEmpty(IEnumerable en)
        {
            foreach (var c in en) return false;

            return true;
        }

        #endregion Class Helper
    }
}