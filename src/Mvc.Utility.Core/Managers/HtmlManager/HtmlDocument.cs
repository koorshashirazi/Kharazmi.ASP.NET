using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Mvc.Utility.Core.Managers.HtmlManager
{
    /// <summary>
    ///     Represents a complete HTML document.
    /// </summary>
    public partial class HtmlDocument
    {
        #region Constructors

        /// <summary>
        ///     Creates an instance of an HTML document.
        /// </summary>
        public HtmlDocument()
        {
            DefaultBuilder?.Invoke(this);

            DocumentNode = CreateNode(HtmlNodeType.Document, 0);
            OptionDefaultStreamEncoding = Encoding.Default;
        }

        #endregion Constructors

        #region Nested type: ParseState

        private enum ParseState
        {
            Text,
            WhichTag,
            Tag,
            BetweenAttributes,
            EmptyTag,
            AttributeName,
            AttributeBeforeEquals,
            AttributeAfterEquals,
            AttributeValue,
            Comment,
            QuotedAttributeValue,
            ServerSideCode,
            PcData
        }

        #endregion Nested type: ParseState

        #region Manager

        internal static bool _disableBehaviorTagP = true;

        /// <summary>True to disable, false to enable the behavior tag p.</summary>
        public static bool DisableBehaviorTagP
        {
            get => _disableBehaviorTagP;
            set
            {
                if (value)
                {
                    if (HtmlNode.ElementsFlags.ContainsKey("p")) HtmlNode.ElementsFlags.Remove("p");
                }
                else
                {
                    if (!HtmlNode.ElementsFlags.ContainsKey("p"))
                        HtmlNode.ElementsFlags.Add("p", HtmlElementFlag.Empty | HtmlElementFlag.Closed);
                }

                _disableBehaviorTagP = value;
            }
        }

        /// <summary>Default builder to use in the HtmlDocument constructor</summary>
        public static Action<HtmlDocument> DefaultBuilder { get; set; }

        /// <summary>Action to execute before the Parse is executed</summary>
        public Action<HtmlDocument> ParseExecuting { get; set; }

        #endregion Manager

        #region Fields

        /// <summary>
        ///     Defines the max level we would go deep into the html document
        /// </summary>
        private static int _maxDepthLevel = int.MaxValue;

        private int _c;
        private Crc32 _crc32;
        private HtmlAttribute _currentattribute;
        private HtmlNode _currentnode;
        private bool _fullcomment;
        private int _index;
        internal Dictionary<string, HtmlNode> Lastnodes = new Dictionary<string, HtmlNode>();
        private HtmlNode _lastparentnode;
        private int _line;
        private int _lineposition, _maxlineposition;
        internal Dictionary<string, HtmlNode> Nodesid;
        private ParseState _oldstate;
        private bool _onlyDetectEncoding;
        internal Dictionary<int, HtmlNode> Openednodes;
        private List<HtmlParseError> _parseerrors = new List<HtmlParseError>();
        private ParseState _state;
        private bool _useHtmlEncodingForStream;

        /// <summary>The HtmlDocument Text. Careful if you modify it.</summary>
        public string Text;

        /// <summary>
        ///     True to stay backward compatible with previous version of HAP. This option does not guarantee 100%
        ///     compatibility.
        /// </summary>
        public bool BackwardCompatibility = true;

        /// <summary>
        ///     Adds Debugging attributes to node. Default is false.
        /// </summary>
        public bool OptionAddDebuggingAttributes;

        /// <summary>
        ///     Defines if closing for non closed nodes must be done at the end or directly in the document.
        ///     Setting this to true can actually change how browsers render the page. Default is false.
        /// </summary>
        public bool OptionAutoCloseOnEnd; // close errors at the end

        /// <summary>
        ///     Defines if non closed nodes will be checked at the end of parsing. Default is true.
        /// </summary>
        public bool OptionCheckSyntax = true;

        /// <summary>
        ///     Defines if a checksum must be computed for the document while parsing. Default is false.
        /// </summary>
        public bool OptionComputeChecksum;

        /// <summary>
        ///     Defines if SelectNodes method will return null or empty collection when no node matched the XPath expression.
        ///     Setting this to true will return empty collection and false will return null. Default is false.
        /// </summary>
        public bool OptionEmptyCollection = false;

        /// <summary>True to disable, false to enable the server side code.</summary>
        public bool DisableServerSideCode = false;

        /// <summary>
        ///     Defines the default stream encoding to use. Default is System.Text.Encoding.Default.
        /// </summary>
        public Encoding OptionDefaultStreamEncoding;

        /// <summary>
        ///     Defines if source text must be extracted while parsing errors.
        ///     If the document has a lot of errors, or cascading errors, parsing performance can be dramatically affected if set
        ///     to true.
        ///     Default is false.
        /// </summary>
        public bool OptionExtractErrorSourceText;

        // turning this on can dramatically slow performance if a lot of errors are detected

        /// <summary>
        ///     Defines the maximum length of source text or parse errors. Default is 100.
        /// </summary>
        public int OptionExtractErrorSourceTextMaxLength = 100;

        /// <summary>
        ///     Defines if LI, TR, TH, TD tags must be partially fixed when nesting errors are detected. Default is false.
        /// </summary>
        public bool OptionFixNestedTags; // fix li, tr, th, td tags

        /// <summary>
        ///     Defines if output must conform to XML, instead of HTML. Default is false.
        /// </summary>
        public bool OptionOutputAsXml;

        /// <summary>
        ///     If used together with <see cref="OptionOutputAsXml" /> and enabled, Xml namespaces in element names are preserved.
        ///     Default is false.
        /// </summary>
        public bool OptionPreserveXmlNamespaces;

        /// <summary>
        ///     Defines if attribute value output must be optimized (not bound with double quotes if it is possible). Default is
        ///     false.
        /// </summary>
        public bool OptionOutputOptimizeAttributeValues;

        /// <summary>
        ///     Defines if name must be output with it's original case. Useful for asp.net tags and attributes. Default is false.
        /// </summary>
        public bool OptionOutputOriginalCase;

        /// <summary>
        ///     Defines if name must be output in uppercase. Default is false.
        /// </summary>
        public bool OptionOutputUpperCase;

        /// <summary>
        ///     Defines if declared encoding must be read from the document.
        ///     Declared encoding is determined using the meta http-equiv="content-type" content="text/html;charset=XXXXX" html
        ///     node.
        ///     Default is true.
        /// </summary>
        public bool OptionReadEncoding = true;

        /// <summary>
        ///     Defines the name of a node that will throw the StopperNodeException when found as an end node. Default is null.
        /// </summary>
        public string OptionStopperNodeName;

        /// <summary>
        ///     Defines if the 'id' attribute must be specifically used. Default is true.
        /// </summary>
        public bool OptionUseIdAttribute = true;

        /// <summary>
        ///     Defines if empty nodes must be written as closed during output. Default is false.
        /// </summary>
        public bool OptionWriteEmptyNodes;

        /// <summary>
        ///     The max number of nested child nodes.
        ///     Added to prevent stackoverflow problem when a page has tens of thousands of opening html tags with no closing tags
        /// </summary>
        public int OptionMaxNestedChildNodes = 0;

        #endregion Fields

        #region Static Members

        internal static readonly string HtmlExceptionRefNotChild = "Reference node must be a child of this node";

        internal static readonly string HtmlExceptionUseIdAttributeFalse =
            "You need to set UseIdAttribute property to true to enable this feature";

        internal static readonly string HtmlExceptionClassDoesNotExist = "Class name doesn't exist";

        internal static readonly string HtmlExceptionClassExists = "Class name already exists";

        internal static readonly Dictionary<string, string[]> HtmlResetters = new Dictionary<string, string[]>
        {
            {"li", new[] {"ul", "ol"}}, {"tr", new[] {"table"}}, {"th", new[] {"tr", "table"}},
            {"td", new[] {"tr", "table"}}
        };

        #endregion Static Members

        #region Properties

        /// <summary>Gets the parsed text.</summary>
        /// <value>The parsed text.</value>
        public string ParsedText => Text;

        /// <summary>
        ///     Defines the max level we would go deep into the html document. If this depth level is exceeded, and exception is
        ///     thrown.
        /// </summary>
        public static int MaxDepthLevel
        {
            get => _maxDepthLevel;
            set => _maxDepthLevel = value;
        }

        /// <summary>
        ///     Gets the document CRC32 checksum if OptionComputeChecksum was set to true before parsing, 0 otherwise.
        /// </summary>
        public int CheckSum => _crc32 == null ? 0 : (int) _crc32.CheckSum;

        /// <summary>
        ///     Gets the document's declared encoding.
        ///     Declared encoding is determined using the meta http-equiv="content-type" content="text/html;charset=XXXXX" html
        ///     node (pre-HTML5) or the meta charset="XXXXX" html node (HTML5).
        /// </summary>
        public Encoding DeclaredEncoding { get; private set; }

        /// <summary>
        ///     Gets the root node of the document.
        /// </summary>
        public HtmlNode DocumentNode { get; private set; }

        /// <summary>
        ///     Gets the document's output encoding.
        /// </summary>
        public Encoding Encoding => GetOutEncoding();

        /// <summary>
        ///     Gets a list of parse errors found in the document.
        /// </summary>
        public IEnumerable<HtmlParseError> ParseErrors => _parseerrors;

        /// <summary>
        ///     Gets the remaining text.
        ///     Will always be null if OptionStopperNodeName is null.
        /// </summary>
        public string Remainder { get; private set; }

        /// <summary>
        ///     Gets the offset of Remainder in the original Html text.
        ///     If OptionStopperNodeName is null, this will return the length of the original Html text.
        /// </summary>
        public int RemainderOffset { get; private set; }

        /// <summary>
        ///     Gets the document's stream encoding.
        /// </summary>
        public Encoding StreamEncoding { get; private set; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        ///     Gets a valid XML name.
        /// </summary>
        /// <param name="name">Any text.</param>
        /// <returns>A string that is a valid XML name.</returns>
        public static string GetXmlName(string name)
        {
            return GetXmlName(name, false, false);
        }

        public void UseAttributeOriginalName(string tagName)
        {
            foreach (var nod in DocumentNode.SelectNodes("//" + tagName))
            foreach (var attribut in nod.Attributes)
                attribut.UseOriginalName = true;
        }

        public static string GetXmlName(string name, bool isAttribute, bool preserveXmlNamespaces)
        {
            var xmlname = string.Empty;
            var nameisok = true;
            foreach (var t in name)
                // names are lcase
                // note: we are very limited here, too much?
                if (t >= 'a' && t <= 'z' ||
                    t >= 'A' && t <= 'Z' ||
                    t >= '0' && t <= '9' ||
                    (isAttribute || preserveXmlNamespaces) && t == ':' ||
                    // (name[i]==':') || (name[i]=='_') || (name[i]=='-') || (name[i]=='.')) // these are bads in fact
                    t == '_' ||
                    t == '-' ||
                    t == '.')
                {
                    xmlname += t;
                }
                else
                {
                    nameisok = false;
                    var bytes = Encoding.UTF8.GetBytes(new[] {t});
                    foreach (var t1 in bytes) xmlname += t1.ToString("x2");

                    xmlname += "_";
                }

            if (nameisok) return xmlname;

            return "_" + xmlname;
        }

        /// <summary>
        ///     Applies HTML encoding to a specified string.
        /// </summary>
        /// <param name="html">The input string to encode. May not be null.</param>
        /// <returns>The encoded string.</returns>
        public static string HtmlEncode(string html)
        {
            return HtmlEncodeWithCompatibility(html);
        }

        internal static string HtmlEncodeWithCompatibility(string html, bool backwardCompatibility = true)
        {
            if (html == null) throw new ArgumentNullException(nameof(html));

            // replace & by &amp; but only once!

            var rx = backwardCompatibility
                ? new Regex("&(?!(amp;)|(lt;)|(gt;)|(quot;))", RegexOptions.IgnoreCase)
                : new Regex("&(?!(amp;)|(lt;)|(gt;)|(quot;)|(nbsp;)|(reg;))", RegexOptions.IgnoreCase);
            return rx.Replace(html, "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;");
        }

        /// <summary>
        ///     Determines if the specified character is considered as a whitespace character.
        /// </summary>
        /// <param name="c">The character to check.</param>
        /// <returns>true if if the specified character is considered as a whitespace character.</returns>
        public static bool IsWhiteSpace(int c)
        {
            if (c == 10 || c == 13 || c == 32 || c == 9) return true;

            return false;
        }

        /// <summary>
        ///     Creates an HTML attribute with the specified name.
        /// </summary>
        /// <param name="name">The name of the attribute. May not be null.</param>
        /// <returns>The new HTML attribute.</returns>
        public HtmlAttribute CreateAttribute(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var att = CreateAttribute();
            att.Name = name;
            return att;
        }

        /// <summary>
        ///     Creates an HTML attribute with the specified name.
        /// </summary>
        /// <param name="name">The name of the attribute. May not be null.</param>
        /// <param name="value">The value of the attribute.</param>
        /// <returns>The new HTML attribute.</returns>
        public HtmlAttribute CreateAttribute(string name, string value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var att = CreateAttribute(name);
            att.Value = value;
            return att;
        }

        /// <summary>
        ///     Creates an HTML comment node.
        /// </summary>
        /// <returns>The new HTML comment node.</returns>
        public HtmlCommentNode CreateComment()
        {
            return (HtmlCommentNode) CreateNode(HtmlNodeType.Comment);
        }

        /// <summary>
        ///     Creates an HTML comment node with the specified comment text.
        /// </summary>
        /// <param name="comment">The comment text. May not be null.</param>
        /// <returns>The new HTML comment node.</returns>
        public HtmlCommentNode CreateComment(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment)) throw new ArgumentNullException(nameof(comment));

            var c = CreateComment();
            c.Comment = comment;
            return c;
        }

        /// <summary>
        ///     Creates an HTML element node with the specified name.
        /// </summary>
        /// <param name="name">The qualified name of the element. May not be null.</param>
        /// <returns>The new HTML node.</returns>
        public HtmlNode CreateElement(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));

            var node = CreateNode(HtmlNodeType.Element);
            node.Name = name;
            return node;
        }

        /// <summary>
        ///     Creates an HTML text node.
        /// </summary>
        /// <returns>The new HTML text node.</returns>
        public HtmlTextNode CreateTextNode()
        {
            return (HtmlTextNode) CreateNode(HtmlNodeType.Text);
        }

        /// <summary>
        ///     Creates an HTML text node with the specified text.
        /// </summary>
        /// <param name="text">The text of the node. May not be null.</param>
        /// <returns>The new HTML text node.</returns>
        public HtmlTextNode CreateTextNode(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new ArgumentNullException(nameof(text));

            var t = CreateTextNode();
            t.Text = text;
            return t;
        }

        /// <summary>
        ///     Detects the encoding of an HTML stream.
        /// </summary>
        /// <param name="stream">The input stream. May not be null.</param>
        /// <returns>The detected encoding.</returns>
        public Encoding DetectEncoding(Stream stream)
        {
            return DetectEncoding(stream, false);
        }

        /// <summary>
        ///     Detects the encoding of an HTML stream.
        /// </summary>
        /// <param name="stream">The input stream. May not be null.</param>
        /// <param name="checkHtml">The html is checked.</param>
        /// <returns>The detected encoding.</returns>
        public Encoding DetectEncoding(Stream stream, bool checkHtml)
        {
            _useHtmlEncodingForStream = checkHtml;

            if (stream == null) throw new ArgumentNullException(nameof(stream));

            return DetectEncoding(new StreamReader(stream));
        }

        /// <summary>
        ///     Detects the encoding of an HTML text provided on a TextReader.
        /// </summary>
        /// <param name="reader">The TextReader used to feed the HTML. May not be null.</param>
        /// <returns>The detected encoding.</returns>
        public Encoding DetectEncoding(TextReader reader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            _onlyDetectEncoding = true;
            Openednodes = OptionCheckSyntax ? new Dictionary<int, HtmlNode>() : null;

            Nodesid = OptionUseIdAttribute ? new Dictionary<string, HtmlNode>(StringComparer.OrdinalIgnoreCase) : null;

            if (reader is StreamReader sr && !_useHtmlEncodingForStream)
            {
                Text = sr.ReadToEnd();
                StreamEncoding = sr.CurrentEncoding;
                return StreamEncoding;
            }

            StreamEncoding = null;
            DeclaredEncoding = null;

            Text = reader.ReadToEnd();
            DocumentNode = CreateNode(HtmlNodeType.Document, 0);

            // this is almost a hack, but it allows us not to muck with the original parsing code
            try
            {
                Parse();
            }
            catch (EncodingFoundException ex)
            {
                return ex.Encoding;
            }

            return StreamEncoding;
        }

        /// <summary>
        ///     Detects the encoding of an HTML text.
        /// </summary>
        /// <param name="html">The input html text. May not be null.</param>
        /// <returns>The detected encoding.</returns>
        public Encoding DetectEncodingHtml(string html)
        {
            if (html == null) throw new ArgumentNullException(nameof(html));

            using (var sr = new StringReader(html))
            {
                var encoding = DetectEncoding(sr);
                return encoding;
            }
        }

        /// <summary>
        ///     Gets the HTML node with the specified 'id' attribute value.
        /// </summary>
        /// <param name="id">The attribute id to match. May not be null.</param>
        /// <returns>The HTML node with the matching id or null if not found.</returns>
        public HtmlNode GetElementById(string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            if (Nodesid == null) throw new Exception(HtmlExceptionUseIdAttributeFalse);

            return Nodesid.ContainsKey(id) ? Nodesid[id] : null;
        }

        /// <summary>
        ///     Loads an HTML document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        public void Load(Stream stream)
        {
            Load(new StreamReader(stream, OptionDefaultStreamEncoding));
        }

        /// <summary>
        ///     Loads an HTML document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="detectEncodingFromByteOrderMarks">
        ///     Indicates whether to look for byte order marks at the beginning of the
        ///     stream.
        /// </param>
        public void Load(Stream stream, bool detectEncodingFromByteOrderMarks)
        {
            Load(new StreamReader(stream, detectEncodingFromByteOrderMarks));
        }

        /// <summary>
        ///     Loads an HTML document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="encoding">The character encoding to use.</param>
        public void Load(Stream stream, Encoding encoding)
        {
            Load(new StreamReader(stream, encoding));
        }

        /// <summary>
        ///     Loads an HTML document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">
        ///     Indicates whether to look for byte order marks at the beginning of the
        ///     stream.
        /// </param>
        public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
        {
            Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks));
        }

        /// <summary>
        ///     Loads an HTML document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">
        ///     Indicates whether to look for byte order marks at the beginning of the
        ///     stream.
        /// </param>
        /// <param name="buffersize">The minimum buffer size.</param>
        public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
        {
            Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks, buffersize));
        }

        /// <summary>
        ///     Loads the HTML document from the specified TextReader.
        /// </summary>
        /// <param name="reader">The TextReader used to feed the HTML data into the document. May not be null.</param>
        public void Load(TextReader reader)
        {
            // all Load methods pass down to this one
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            _onlyDetectEncoding = false;

            Openednodes = OptionCheckSyntax ? new Dictionary<int, HtmlNode>() : null;

            Nodesid = OptionUseIdAttribute ? new Dictionary<string, HtmlNode>(StringComparer.OrdinalIgnoreCase) : null;

            var sr = reader as StreamReader;
            if (sr != null)
            {
                try
                {
                    // trigger bom read if needed
                    sr.Peek();
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch (Exception)
                    // ReSharper restore EmptyGeneralCatchClause
                {
                    // void on purpose
                }

                StreamEncoding = sr.CurrentEncoding;
            }
            else
            {
                StreamEncoding = null;
            }

            DeclaredEncoding = null;

            Text = reader.ReadToEnd();
            DocumentNode = CreateNode(HtmlNodeType.Document, 0);
            Parse();

            if (!OptionCheckSyntax || Openednodes == null) return;

            foreach (var node in Openednodes.Values)
            {
                if (!node._starttag) // already reported
                    continue;

                string html;
                if (OptionExtractErrorSourceText)
                {
                    html = node.OuterHtml;
                    if (html.Length > OptionExtractErrorSourceTextMaxLength)
                        html = html.Substring(0, OptionExtractErrorSourceTextMaxLength);
                }
                else
                {
                    html = string.Empty;
                }

                AddError(HtmlParseErrorCode.TagNotClosed, node._line, node._lineposition, node._streamposition, html,
                    "End tag </" + node.Name + "> was not found");
            }

            // we don't need this anymore
            Openednodes.Clear();
        }

        /// <summary>
        ///     Loads the HTML document from the specified string.
        /// </summary>
        /// <param name="html">String containing the HTML document to load. May not be null.</param>
        public void LoadHtml(string html)
        {
            if (html == null) throw new ArgumentNullException("html");

            using (var sr = new StringReader(html))
            {
                Load(sr);
            }
        }

        /// <summary>
        ///     Saves the HTML document to the specified stream.
        /// </summary>
        /// <param name="outStream">The stream to which you want to save.</param>
        public void Save(Stream outStream)
        {
            var sw = new StreamWriter(outStream, GetOutEncoding());
            Save(sw);
        }

        /// <summary>
        ///     Saves the HTML document to the specified stream.
        /// </summary>
        /// <param name="outStream">The stream to which you want to save. May not be null.</param>
        /// <param name="encoding">The character encoding to use. May not be null.</param>
        public void Save(Stream outStream, Encoding encoding)
        {
            if (outStream == null) throw new ArgumentNullException("outStream");

            if (encoding == null) throw new ArgumentNullException("encoding");

            var sw = new StreamWriter(outStream, encoding);
            Save(sw);
        }

        /// <summary>
        ///     Saves the HTML document to the specified StreamWriter.
        /// </summary>
        /// <param name="writer">The StreamWriter to which you want to save.</param>
        public void Save(StreamWriter writer)
        {
            Save((TextWriter) writer);
        }

        /// <summary>
        ///     Saves the HTML document to the specified TextWriter.
        /// </summary>
        /// <param name="writer">The TextWriter to which you want to save. May not be null.</param>
        public void Save(TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException("writer");

            DocumentNode.WriteTo(writer);
            writer.Flush();
        }

        /// <summary>
        ///     Saves the HTML document to the specified XmlWriter.
        /// </summary>
        /// <param name="writer">The XmlWriter to which you want to save.</param>
        public void Save(XmlWriter writer)
        {
            DocumentNode.WriteTo(writer);
            writer.Flush();
        }

        #endregion Public Methods

        #region Internal Methods

        internal HtmlAttribute CreateAttribute()
        {
            return new HtmlAttribute(this);
        }

        internal HtmlNode CreateNode(HtmlNodeType type)
        {
            return CreateNode(type, -1);
        }

        internal HtmlNode CreateNode(HtmlNodeType type, int index)
        {
            switch (type)
            {
                case HtmlNodeType.Comment:
                    return new HtmlCommentNode(this, index);

                case HtmlNodeType.Text:
                    return new HtmlTextNode(this, index);

                default:
                    return new HtmlNode(type, this, index);
            }
        }

        internal Encoding GetOutEncoding()
        {
            return DeclaredEncoding ?? (StreamEncoding ?? OptionDefaultStreamEncoding);
        }

        internal HtmlNode GetXmlDeclaration()
        {
            if (!DocumentNode.HasChildNodes) return null;

            foreach (var node in DocumentNode._childnodes)
                if (node.Name == "?xml") // it's ok, names are case sensitive
                    return node;

            return null;
        }

        internal void SetIdForNode(HtmlNode node, string id)
        {
            if (!OptionUseIdAttribute) return;

            if (Nodesid == null || id == null) return;

            if (node == null)
                Nodesid.Remove(id);
            else
                Nodesid[id] = node;
        }

        internal void UpdateLastParentNode()
        {
            do
            {
                if (_lastparentnode.Closed) _lastparentnode = _lastparentnode.ParentNode;
            } while (_lastparentnode != null && _lastparentnode.Closed);

            if (_lastparentnode == null) _lastparentnode = DocumentNode;
        }

        #endregion Internal Methods

        #region Private Methods

        private void AddError(HtmlParseErrorCode code, int line, int linePosition, int streamPosition, string sourceText
            , string reason)
        {
            var err = new HtmlParseError(code, line, linePosition, streamPosition, sourceText, reason);
            _parseerrors.Add(err);
        }

        private void CloseCurrentNode()
        {
            if (_currentnode.Closed) // text or document are by def closed
                return;

            var error = false;
            var prev = Utilities.GetDictionaryValueOrDefault(Lastnodes, _currentnode.Name);

            // find last node of this kind
            if (prev == null)
            {
                if (HtmlNode.IsClosedElement(_currentnode.Name))
                {
                    // </br> will be seen as <br>
                    _currentnode.CloseNode(_currentnode);

                    // add to parent node
                    if (_lastparentnode != null)
                    {
                        HtmlNode foundNode = null;
                        var futureChild = new Stack<HtmlNode>();
                        for (var node = _lastparentnode.LastChild; node != null; node = node.PreviousSibling)
                        {
                            if (node.Name == _currentnode.Name && !node.HasChildNodes)
                            {
                                foundNode = node;
                                break;
                            }

                            futureChild.Push(node);
                        }

                        if (foundNode != null)
                            while (futureChild.Count != 0)
                            {
                                var node = futureChild.Pop();
                                _lastparentnode.RemoveChild(node);
                                foundNode.AppendChild(node);
                            }
                        else
                            _lastparentnode.AppendChild(_currentnode);
                    }
                }
                else
                {
                    // node has no parent
                    // node is not a closed node

                    if (HtmlNode.CanOverlapElement(_currentnode.Name))
                    {
                        // this is a hack: add it as a text node
                        var closenode = CreateNode(HtmlNodeType.Text, _currentnode._outerstartindex);
                        closenode._outerlength = _currentnode._outerlength;
                        ((HtmlTextNode) closenode).Text = ((HtmlTextNode) closenode).Text.ToLowerInvariant();
                        if (_lastparentnode != null) _lastparentnode.AppendChild(closenode);
                    }
                    else
                    {
                        if (HtmlNode.IsEmptyElement(_currentnode.Name))
                        {
                            AddError(HtmlParseErrorCode.EndTagNotRequired, _currentnode._line,
                                _currentnode._lineposition, _currentnode._streamposition, _currentnode.OuterHtml,
                                "End tag </" + _currentnode.Name + "> is not required");
                        }
                        else
                        {
                            // node cannot overlap, node is not empty
                            AddError(HtmlParseErrorCode.TagNotOpened, _currentnode._line, _currentnode._lineposition,
                                _currentnode._streamposition, _currentnode.OuterHtml,
                                "Start tag <" + _currentnode.Name + "> was not found");
                            error = true;
                        }
                    }
                }
            }
            else
            {
                if (OptionFixNestedTags)
                    if (FindResetterNodes(prev, GetResetters(_currentnode.Name)))
                    {
                        AddError(HtmlParseErrorCode.EndTagInvalidHere, _currentnode._line, _currentnode._lineposition,
                            _currentnode._streamposition, _currentnode.OuterHtml,
                            "End tag </" + _currentnode.Name + "> invalid here");
                        error = true;
                    }

                if (!error)
                {
                    Lastnodes[_currentnode.Name] = prev._prevwithsamename;
                    prev.CloseNode(_currentnode);
                }
            }

            // we close this node, get grandparent
            if (!error)
                if (_lastparentnode != null && (!HtmlNode.IsClosedElement(_currentnode.Name) || _currentnode._starttag))
                    UpdateLastParentNode();
        }

        private string CurrentNodeName()
        {
            return Text.Substring(_currentnode._namestartindex, _currentnode._namelength);
        }

        private void DecrementPosition()
        {
            _index--;
            if (_lineposition == 0)
            {
                _lineposition = _maxlineposition;
                _line--;
            }
            else
            {
                _lineposition--;
            }
        }

        private HtmlNode FindResetterNode(HtmlNode node, string name)
        {
            var resetter = Utilities.GetDictionaryValueOrDefault(Lastnodes, name);
            if (resetter == null) return null;

            if (resetter.Closed) return null;

            if (resetter._streamposition < node._streamposition) return null;

            return resetter;
        }

        private bool FindResetterNodes(HtmlNode node, string[] names)
        {
            if (names == null) return false;

            for (var i = 0; i < names.Length; i++)
                if (FindResetterNode(node, names[i]) != null)
                    return true;

            return false;
        }

        private void FixNestedTag(string name, string[] resetters)
        {
            if (resetters == null) return;

            var prev = Utilities.GetDictionaryValueOrDefault(Lastnodes, _currentnode.Name);
            // if we find a previous unclosed same name node, without a resetter node between, we must close it
            if (prev == null || Lastnodes[name].Closed) return;

            // try to find a resetter node, if found, we do nothing
            if (FindResetterNodes(prev, resetters)) return;

            // ok we need to close the prev now
            // create a fake closer node
            var close = new HtmlNode(prev.NodeType, this, -1);
            close._endnode = close;
            prev.CloseNode(close);
        }

        private void FixNestedTags()
        {
            // we are only interested by start tags, not closing tags
            if (!_currentnode._starttag) return;

            var name = CurrentNodeName();
            FixNestedTag(name, GetResetters(name));
        }

        private string[] GetResetters(string name)
        {
            if (!HtmlResetters.TryGetValue(name, out var resetters)) return null;

            return resetters;
        }

        private void IncrementPosition()
        {
            if (_crc32 != null)
                // REVIEW: should we add some checksum code in DecrementPosition too?
                _crc32.AddToCrc32(_c);

            _index++;
            _maxlineposition = _lineposition;
            if (_c == 10)
            {
                _lineposition = 0;
                _line++;
            }
            else
            {
                _lineposition++;
            }
        }

        private bool IsValidTag()
        {
            var isValidTag = _c == '<' && _index < Text.Length &&
                             (char.IsLetter(Text[_index]) || Text[_index] == '/' || Text[_index] == '!' ||
                              Text[_index] == '%');
            return isValidTag;
        }

        private bool NewCheck()
        {
            if (_c != '<' || !IsValidTag()) return false;

            if (_index < Text.Length)
                if (Text[_index] == '%')
                {
                    if (DisableServerSideCode) return false;

                    switch (_state)
                    {
                        case ParseState.AttributeAfterEquals:
                            PushAttributeValueStart(_index - 1);
                            break;

                        case ParseState.BetweenAttributes:
                            PushAttributeNameStart(_index - 1, _lineposition - 1);
                            break;

                        case ParseState.WhichTag:
                            PushNodeNameStart(true, _index - 1);
                            _state = ParseState.Tag;
                            break;
                    }

                    _oldstate = _state;
                    _state = ParseState.ServerSideCode;
                    return true;
                }

            if (!PushNodeEnd(_index - 1, true))
            {
                // stop parsing
                _index = Text.Length;
                return true;
            }

            _state = ParseState.WhichTag;
            if (_index - 1 <= Text.Length - 2)
                if (Text[_index] == '!')
                {
                    PushNodeStart(HtmlNodeType.Comment, _index - 1, _lineposition - 1);
                    PushNodeNameStart(true, _index);
                    PushNodeNameEnd(_index + 1);
                    _state = ParseState.Comment;
                    if (_index < Text.Length - 2)
                    {
                        if (Text[_index + 1] == '-' && Text[_index + 2] == '-')
                            _fullcomment = true;
                        else
                            _fullcomment = false;
                    }

                    return true;
                }

            PushNodeStart(HtmlNodeType.Element, _index - 1, _lineposition - 1);
            return true;
        }

        private void Parse()
        {
            ParseExecuting?.Invoke(this);

            var lastquote = 0;
            if (OptionComputeChecksum) _crc32 = new Crc32();

            Lastnodes = new Dictionary<string, HtmlNode>();
            _c = 0;
            _fullcomment = false;
            _parseerrors = new List<HtmlParseError>();
            _line = 1;
            _lineposition = 0;
            _maxlineposition = 0;

            _state = ParseState.Text;
            _oldstate = _state;
            DocumentNode._innerlength = Text.Length;
            DocumentNode._outerlength = Text.Length;
            RemainderOffset = Text.Length;

            _lastparentnode = DocumentNode;
            _currentnode = CreateNode(HtmlNodeType.Text, 0);
            _currentattribute = null;

            _index = 0;
            PushNodeStart(HtmlNodeType.Text, 0, _lineposition);
            while (_index < Text.Length)
            {
                _c = Text[_index];
                IncrementPosition();

                switch (_state)
                {
                    case ParseState.Text:
                        if (NewCheck())
                        {
                        }

                        break;

                    case ParseState.WhichTag:
                        if (NewCheck()) continue;

                        if (_c == '/')
                        {
                            PushNodeNameStart(false, _index);
                        }
                        else
                        {
                            PushNodeNameStart(true, _index - 1);
                            DecrementPosition();
                        }

                        _state = ParseState.Tag;
                        break;

                    case ParseState.Tag:
                        if (NewCheck()) continue;

                        if (IsWhiteSpace(_c))
                        {
                            CloseParentImplicitExplicitNode();

                            PushNodeNameEnd(_index - 1);
                            if (_state != ParseState.Tag) continue;

                            _state = ParseState.BetweenAttributes;
                            continue;
                        }

                        if (_c == '/')
                        {
                            CloseParentImplicitExplicitNode();

                            PushNodeNameEnd(_index - 1);
                            if (_state != ParseState.Tag) continue;

                            _state = ParseState.EmptyTag;
                            continue;
                        }

                        if (_c == '>')
                        {
                            CloseParentImplicitExplicitNode();

                            //// CHECK if parent is compatible with end tag
                            //if (IsParentIncompatibleEndTag())
                            //{
                            //    _state = ParseState.Text;
                            //    PushNodeStart(HtmlNodeType.Text, _index);
                            //    break;
                            //}

                            PushNodeNameEnd(_index - 1);
                            if (_state != ParseState.Tag) continue;

                            if (!PushNodeEnd(_index, false))
                            {
                                // stop parsing
                                _index = Text.Length;
                                break;
                            }

                            if (_state != ParseState.Tag) continue;

                            _state = ParseState.Text;
                            PushNodeStart(HtmlNodeType.Text, _index, _lineposition);
                        }

                        break;

                    case ParseState.BetweenAttributes:
                        if (NewCheck()) continue;

                        if (IsWhiteSpace(_c)) continue;

                        if (_c == '/' || _c == '?')
                        {
                            _state = ParseState.EmptyTag;
                            continue;
                        }

                        if (_c == '>')
                        {
                            if (!PushNodeEnd(_index, false))
                            {
                                // stop parsing
                                _index = Text.Length;
                                break;
                            }

                            if (_state != ParseState.BetweenAttributes) continue;

                            _state = ParseState.Text;
                            PushNodeStart(HtmlNodeType.Text, _index, _lineposition);
                            continue;
                        }

                        PushAttributeNameStart(_index - 1, _lineposition - 1);
                        _state = ParseState.AttributeName;
                        break;

                    case ParseState.EmptyTag:
                        if (NewCheck()) continue;

                        if (_c == '>')
                        {
                            if (!PushNodeEnd(_index, true))
                            {
                                // stop parsing
                                _index = Text.Length;
                                break;
                            }

                            if (_state != ParseState.EmptyTag) continue;

                            _state = ParseState.Text;
                            PushNodeStart(HtmlNodeType.Text, _index, _lineposition);
                            continue;
                        }

                        // we may end up in this state if attributes are incorrectly seperated
                        // by a /-character. If so, start parsing attribute-name immediately.
                        if (!IsWhiteSpace(_c))
                        {
                            // Just do nothing and push to next one!
                            DecrementPosition();
                            _state = ParseState.BetweenAttributes;
                        }
                        else
                        {
                            _state = ParseState.BetweenAttributes;
                        }

                        break;

                    case ParseState.AttributeName:
                        if (NewCheck()) continue;

                        if (IsWhiteSpace(_c))
                        {
                            PushAttributeNameEnd(_index - 1);
                            _state = ParseState.AttributeBeforeEquals;
                            continue;
                        }

                        if (_c == '=')
                        {
                            PushAttributeNameEnd(_index - 1);
                            _state = ParseState.AttributeAfterEquals;
                            continue;
                        }

                        if (_c == '>')
                        {
                            PushAttributeNameEnd(_index - 1);
                            if (!PushNodeEnd(_index, false))
                            {
                                // stop parsing
                                _index = Text.Length;
                                break;
                            }

                            if (_state != ParseState.AttributeName) continue;

                            _state = ParseState.Text;
                            PushNodeStart(HtmlNodeType.Text, _index, _lineposition);
                        }

                        break;

                    case ParseState.AttributeBeforeEquals:
                        if (NewCheck()) continue;

                        if (IsWhiteSpace(_c)) continue;

                        if (_c == '>')
                        {
                            if (!PushNodeEnd(_index, false))
                            {
                                // stop parsing
                                _index = Text.Length;
                                break;
                            }

                            if (_state != ParseState.AttributeBeforeEquals) continue;

                            _state = ParseState.Text;
                            PushNodeStart(HtmlNodeType.Text, _index, _lineposition);
                            continue;
                        }

                        if (_c == '=')
                        {
                            _state = ParseState.AttributeAfterEquals;
                            continue;
                        }

                        // no equals, no whitespace, it's a new attrribute starting
                        _state = ParseState.BetweenAttributes;
                        DecrementPosition();
                        break;

                    case ParseState.AttributeAfterEquals:
                        if (NewCheck()) continue;

                        if (IsWhiteSpace(_c)) continue;

                        if (_c == '\'' || _c == '"')
                        {
                            _state = ParseState.QuotedAttributeValue;
                            PushAttributeValueStart(_index, _c);
                            lastquote = _c;
                            continue;
                        }

                        if (_c == '>')
                        {
                            if (!PushNodeEnd(_index, false))
                            {
                                // stop parsing
                                _index = Text.Length;
                                break;
                            }

                            if (_state != ParseState.AttributeAfterEquals) continue;

                            _state = ParseState.Text;
                            PushNodeStart(HtmlNodeType.Text, _index, _lineposition);
                            continue;
                        }

                        PushAttributeValueStart(_index - 1);
                        _state = ParseState.AttributeValue;
                        break;

                    case ParseState.AttributeValue:
                        if (NewCheck()) continue;

                        if (IsWhiteSpace(_c))
                        {
                            PushAttributeValueEnd(_index - 1);
                            _state = ParseState.BetweenAttributes;
                            continue;
                        }

                        if (_c == '>')
                        {
                            PushAttributeValueEnd(_index - 1);
                            if (!PushNodeEnd(_index, false))
                            {
                                // stop parsing
                                _index = Text.Length;
                                break;
                            }

                            if (_state != ParseState.AttributeValue) continue;

                            _state = ParseState.Text;
                            PushNodeStart(HtmlNodeType.Text, _index, _lineposition);
                        }

                        break;

                    case ParseState.QuotedAttributeValue:
                        if (_c == lastquote)
                        {
                            PushAttributeValueEnd(_index - 1);
                            _state = ParseState.BetweenAttributes;
                            continue;
                        }

                        if (_c == '<')
                            if (_index < Text.Length)
                                if (Text[_index] == '%')
                                {
                                    _oldstate = _state;
                                    _state = ParseState.ServerSideCode;
                                }

                        break;

                    case ParseState.Comment:
                        if (_c == '>')
                        {
                            if (_fullcomment)
                                if ((Text[_index - 2] != '-' || Text[_index - 3] != '-') &&
                                    (Text[_index - 2] != '!' || Text[_index - 3] != '-' || Text[_index - 4] != '-'))
                                    continue;

                            if (!PushNodeEnd(_index, false))
                            {
                                // stop parsing
                                _index = Text.Length;
                                break;
                            }

                            _state = ParseState.Text;
                            PushNodeStart(HtmlNodeType.Text, _index, _lineposition);
                        }

                        break;

                    case ParseState.ServerSideCode:
                        if (_c == '%')
                        {
                            if (_index < Text.Length)
                                if (Text[_index] == '>')
                                {
                                    switch (_oldstate)
                                    {
                                        case ParseState.AttributeAfterEquals:
                                            _state = ParseState.AttributeValue;
                                            break;

                                        case ParseState.BetweenAttributes:
                                            PushAttributeNameEnd(_index + 1);
                                            _state = ParseState.BetweenAttributes;
                                            break;

                                        default:
                                            _state = _oldstate;
                                            break;
                                    }

                                    IncrementPosition();
                                }
                        }
                        else if (_oldstate == ParseState.QuotedAttributeValue && _c == lastquote)
                        {
                            _state = _oldstate;
                            DecrementPosition();
                        }

                        break;

                    case ParseState.PcData:
                        // look for </tag + 1 char

                        // check buffer end
                        if (_currentnode._namelength + 3 <= Text.Length - (_index - 1))
                            if (string.Compare(Text.Substring(_index - 1, _currentnode._namelength + 2),
                                    "</" + _currentnode.Name, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                int c = Text[_index - 1 + 2 + _currentnode.Name.Length];
                                if (c == '>' || IsWhiteSpace(c))
                                {
                                    // add the script as a text node
                                    var script = CreateNode(HtmlNodeType.Text,
                                        _currentnode._outerstartindex + _currentnode._outerlength);
                                    script._outerlength = _index - 1 - script._outerstartindex;
                                    script._streamposition = script._outerstartindex;
                                    script._line = _currentnode.Line;
                                    script._lineposition = _currentnode.LinePosition + _currentnode._namelength + 2;
                                    _currentnode.AppendChild(script);

                                    PushNodeStart(HtmlNodeType.Element, _index - 1, _lineposition - 1);
                                    PushNodeNameStart(false, _index - 1 + 2);
                                    _state = ParseState.Tag;
                                    IncrementPosition();
                                }
                            }

                        break;
                }
            }

            // TODO: Add implicit end here?

            // finish the current work
            if (_currentnode._namestartindex > 0) PushNodeNameEnd(_index);

            PushNodeEnd(_index, false);

            // we don't need this anymore
            Lastnodes.Clear();
        }

        private void PushAttributeNameEnd(int index)
        {
            _currentattribute._namelength = index - _currentattribute._namestartindex;
            _currentnode.Attributes.Append(_currentattribute);
        }

        private void PushAttributeNameStart(int index, int lineposition)
        {
            _currentattribute = CreateAttribute();
            _currentattribute._namestartindex = index;
            _currentattribute.Line = _line;
            _currentattribute._lineposition = lineposition;
            _currentattribute._streamposition = index;
        }

        private void PushAttributeValueEnd(int index)
        {
            _currentattribute._valuelength = index - _currentattribute._valuestartindex;
        }

        private void PushAttributeValueStart(int index)
        {
            PushAttributeValueStart(index, 0);
        }

        private void CloseParentImplicitExplicitNode()
        {
            var hasNodeToClose = true;

            while (hasNodeToClose && !_lastparentnode.Closed)
            {
                hasNodeToClose = false;

                var forceExplicitEnd = false;

                // CHECK if parent must be implicitely closed
                if (IsParentImplicitEnd())
                {
                    if (OptionOutputAsXml)
                    {
                        forceExplicitEnd = true;
                    }
                    else
                    {
                        CloseParentImplicitEnd();
                        hasNodeToClose = true;
                    }
                }

                // CHECK if parent must be explicitely closed
                if (forceExplicitEnd || IsParentExplicitEnd())
                {
                    CloseParentExplicitEnd();
                    hasNodeToClose = true;
                }
            }
        }

        private bool IsParentImplicitEnd()
        {
            // MUST be a start tag
            if (!_currentnode._starttag) return false;

            var isImplicitEnd = false;

            var parent = _lastparentnode.Name;
            var nodeName = Text.Substring(_currentnode._namestartindex, _index - _currentnode._namestartindex - 1)
                .ToLowerInvariant();

            switch (parent)
            {
                case "a":
                    isImplicitEnd = nodeName == "a";
                    break;

                case "dd":
                    isImplicitEnd = nodeName == "dt" || nodeName == "dd";
                    break;

                case "dt":
                    isImplicitEnd = nodeName == "dt" || nodeName == "dd";
                    break;

                case "li":
                    isImplicitEnd = nodeName == "li";
                    break;

                case "p":
                    if (DisableBehaviorTagP)
                        isImplicitEnd = nodeName == "address" || nodeName == "article" || nodeName == "aside" ||
                                        nodeName == "blockquote" || nodeName == "dir" || nodeName == "div" ||
                                        nodeName == "dl" || nodeName == "fieldset" || nodeName == "footer" ||
                                        nodeName == "form" || nodeName == "h1" || nodeName == "h2" ||
                                        nodeName == "h3" || nodeName == "h4" || nodeName == "h5" || nodeName == "h6" ||
                                        nodeName == "header" || nodeName == "hr" || nodeName == "menu" ||
                                        nodeName == "nav" || nodeName == "ol" || nodeName == "p" || nodeName == "pre" ||
                                        nodeName == "section" || nodeName == "table" || nodeName == "ul";
                    else
                        isImplicitEnd = nodeName == "p";

                    break;

                case "option":
                    isImplicitEnd = nodeName == "option";
                    break;
            }

            return isImplicitEnd;
        }

        private bool IsParentExplicitEnd()
        {
            // MUST be a start tag
            if (!_currentnode._starttag) return false;

            var isExplicitEnd = false;

            var parent = _lastparentnode.Name;
            var nodeName = Text.Substring(_currentnode._namestartindex, _index - _currentnode._namestartindex - 1)
                .ToLowerInvariant();

            switch (parent)
            {
                case "title":
                    isExplicitEnd = nodeName == "title";
                    break;

                case "p":
                    isExplicitEnd = nodeName == "div";
                    break;

                case "table":
                    isExplicitEnd = nodeName == "table";
                    break;

                case "tr":
                    isExplicitEnd = nodeName == "tr";
                    break;

                case "td":
                    isExplicitEnd = nodeName == "td" || nodeName == "th" || nodeName == "tr";
                    break;

                case "th":
                    isExplicitEnd = nodeName == "td" || nodeName == "th" || nodeName == "tr";
                    break;

                case "h1":
                    isExplicitEnd = nodeName == "h2" || nodeName == "h3" || nodeName == "h4" || nodeName == "h5";
                    break;

                case "h2":
                    isExplicitEnd = nodeName == "h1" || nodeName == "h3" || nodeName == "h4" || nodeName == "h5";
                    break;

                case "h3":
                    isExplicitEnd = nodeName == "h1" || nodeName == "h2" || nodeName == "h4" || nodeName == "h5";
                    break;

                case "h4":
                    isExplicitEnd = nodeName == "h1" || nodeName == "h2" || nodeName == "h3" || nodeName == "h5";
                    break;

                case "h5":
                    isExplicitEnd = nodeName == "h1" || nodeName == "h2" || nodeName == "h3" || nodeName == "h4";
                    break;
            }

            return isExplicitEnd;
        }

        //private bool IsParentIncompatibleEndTag()
        //{
        //    // MUST be a end tag
        //    if (_currentnode._starttag) return false;

        //    bool isIncompatible = false;

        //    var parent = _lastparentnode.Name;
        //    var nodeName = Text.Substring(_currentnode._namestartindex, _index - _currentnode._namestartindex - 1);

        //    switch (parent)
        //    {
        //        case "h1":
        //            isIncompatible = nodeName == "h2" || nodeName == "h3" || nodeName == "h4" || nodeName == "h5";
        //            break;
        //        case "h2":
        //            isIncompatible = nodeName == "h1" || nodeName == "h3" || nodeName == "h4" || nodeName == "h5";
        //            break;
        //        case "h3":
        //            isIncompatible = nodeName == "h1" || nodeName == "h2" || nodeName == "h4" || nodeName == "h5";
        //            break;
        //        case "h4":
        //            isIncompatible = nodeName == "h1" || nodeName == "h2" || nodeName == "h3" || nodeName == "h5";
        //            break;
        //        case "h5":
        //            isIncompatible = nodeName == "h1" || nodeName == "h2" || nodeName == "h3" || nodeName == "h4";
        //            break;
        //    }

        //    return isIncompatible;
        //}

        private void CloseParentImplicitEnd()
        {
            var close = new HtmlNode(_lastparentnode.NodeType, this, -1);
            close._endnode = close;
            close._isImplicitEnd = true;
            _lastparentnode._isImplicitEnd = true;
            _lastparentnode.CloseNode(close);
        }

        private void CloseParentExplicitEnd()
        {
            var close = new HtmlNode(_lastparentnode.NodeType, this, -1);
            close._endnode = close;
            _lastparentnode.CloseNode(close);
        }

        private void PushAttributeValueStart(int index, int quote)
        {
            _currentattribute._valuestartindex = index;
            if (quote == '\'') _currentattribute.QuoteType = AttributeValueQuote.SingleQuote;
        }

        private bool PushNodeEnd(int index, bool close)
        {
            _currentnode._outerlength = index - _currentnode._outerstartindex;

            if (_currentnode._nodetype == HtmlNodeType.Text || _currentnode._nodetype == HtmlNodeType.Comment)
            {
                // forget about void nodes
                if (_currentnode._outerlength > 0)
                {
                    _currentnode._innerlength = _currentnode._outerlength;
                    _currentnode._innerstartindex = _currentnode._outerstartindex;
                    if (_lastparentnode != null) _lastparentnode.AppendChild(_currentnode);
                }
            }
            else
            {
                if (_currentnode._starttag && _lastparentnode != _currentnode)
                {
                    // add to parent node
                    if (_lastparentnode != null) _lastparentnode.AppendChild(_currentnode);

                    ReadDocumentEncoding(_currentnode);

                    // remember last node of this kind
                    var prev = Utilities.GetDictionaryValueOrDefault(Lastnodes, _currentnode.Name);

                    _currentnode._prevwithsamename = prev;
                    Lastnodes[_currentnode.Name] = _currentnode;

                    // change parent?
                    if (_currentnode.NodeType == HtmlNodeType.Document || _currentnode.NodeType == HtmlNodeType.Element)
                        _lastparentnode = _currentnode;

                    if (HtmlNode.IsCDataElement(CurrentNodeName()))
                    {
                        _state = ParseState.PcData;
                        return true;
                    }

                    if (HtmlNode.IsClosedElement(_currentnode.Name) || HtmlNode.IsEmptyElement(_currentnode.Name))
                        close = true;
                }
            }

            if (close || !_currentnode._starttag)
            {
                if (OptionStopperNodeName != null && Remainder == null && string.Compare(_currentnode.Name,
                        OptionStopperNodeName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    RemainderOffset = index;
                    Remainder = Text.Substring(RemainderOffset);
                    CloseCurrentNode();
                    return false; // stop parsing
                }

                CloseCurrentNode();
            }

            return true;
        }

        private void PushNodeNameEnd(int index)
        {
            _currentnode._namelength = index - _currentnode._namestartindex;
            if (OptionFixNestedTags) FixNestedTags();
        }

        private void PushNodeNameStart(bool starttag, int index)
        {
            _currentnode._starttag = starttag;
            _currentnode._namestartindex = index;
        }

        private void PushNodeStart(HtmlNodeType type, int index, int lineposition)
        {
            _currentnode = CreateNode(type, index);
            _currentnode._line = _line;
            _currentnode._lineposition = lineposition;
            _currentnode._streamposition = index;
        }

        private void ReadDocumentEncoding(HtmlNode node)
        {
            if (!OptionReadEncoding) return;
            // format is
            // <meta http-equiv="content-type" content="text/html;charset=iso-8859-1" />

            // when we append a child, we are in node end, so attributes are already populated
            if (node._namelength != 4) // quick check, avoids string alloc
                return;

            if (node.Name != "meta") // all nodes names are lowercase
                return;

            string charset = null;
            var att = node.Attributes["http-equiv"];
            if (att != null)
            {
                if (string.Compare(att.Value, "content-type", StringComparison.OrdinalIgnoreCase) != 0) return;

                var content = node.Attributes["content"];
                if (content != null) charset = NameValuePairList.GetNameValuePairsValue(content.Value, "charset");
            }
            else
            {
                att = node.Attributes["charset"];
                if (att != null) charset = att.Value;
            }

            if (!string.IsNullOrEmpty(charset))
            {
                // The following check fixes the the bug described at: http://htmlagilitypack.codeplex.com/WorkItem/View.aspx?WorkItemId=25273
                if (string.Equals(charset, "utf8", StringComparison.OrdinalIgnoreCase)) charset = "utf-8";

                try
                {
                    DeclaredEncoding = Encoding.GetEncoding(charset);
                }
                catch (ArgumentException)
                {
                    DeclaredEncoding = null;
                }

                if (_onlyDetectEncoding) throw new EncodingFoundException(DeclaredEncoding);

                if (StreamEncoding != null)
                {
#if SILVERLIGHT || PocketPC || METRO || NETSTANDARD1_3 || NETSTANDARD1_6
                    if (_declaredencoding.WebName != _streamencoding.WebName)
#else
                    if (DeclaredEncoding != null)
                        if (DeclaredEncoding.CodePage != StreamEncoding.CodePage)
#endif
                            AddError(HtmlParseErrorCode.CharsetMismatch, _line, _lineposition, _index, node.OuterHtml,
                                "Encoding mismatch between StreamEncoding: " + StreamEncoding.WebName +
                                " and DeclaredEncoding: " + DeclaredEncoding.WebName);
                }
            }
        }

        #endregion Private Methods
    }
}