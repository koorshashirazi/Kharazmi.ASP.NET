using System;
using System.IO;
#if !METRO
using System.Text;

namespace Mvc.Utility.Core.Managers.HtmlManager
{
    /// <summary>
    ///     Represents a document with mixed code and text. ASP, ASPX, JSP, are good example of such documents.
    /// </summary>
    public class MixedCodeDocument
    {
        #region Constructors

        /// <summary>
        ///     Creates a mixed code document instance.
        /// </summary>
        public MixedCodeDocument()
        {
            _codefragments = new MixedCodeDocumentFragmentList(this);
            _textfragments = new MixedCodeDocumentFragmentList(this);
            _fragments = new MixedCodeDocumentFragmentList(this);
        }

        #endregion Constructors

        #region Nested type: ParseState

        private enum ParseState
        {
            Text,
            Code
        }

        #endregion Nested type: ParseState

        #region Fields

        private int _c;
        internal MixedCodeDocumentFragmentList _codefragments;
        private MixedCodeDocumentFragment _currentfragment;
        internal MixedCodeDocumentFragmentList _fragments;
        private int _index;
        private int _line;
        private int _lineposition;
        private ParseState _state;
        internal string _text;
        internal MixedCodeDocumentFragmentList _textfragments;

        /// <summary>
        ///     Gets or sets the token representing code end.
        /// </summary>
        public string TokenCodeEnd = "%>";

        /// <summary>
        ///     Gets or sets the token representing code start.
        /// </summary>
        public string TokenCodeStart = "<%";

        /// <summary>
        ///     Gets or sets the token representing code directive.
        /// </summary>
        public string TokenDirective = "@";

        /// <summary>
        ///     Gets or sets the token representing response write directive.
        /// </summary>
        public string TokenResponseWrite = "Response.Write ";

        private readonly string TokenTextBlock = "TextBlock({0})";

        #endregion Fields

        #region Properties

        /// <summary>
        ///     Gets the code represented by the mixed code document seen as a template.
        /// </summary>
        public string Code
        {
            get
            {
                var s = "";
                var i = 0;
                foreach (var frag in _fragments)
                    switch (frag._type)
                    {
                        case MixedCodeDocumentFragmentType.Text:
                            s += TokenResponseWrite + string.Format(TokenTextBlock, i) + "\n";
                            i++;
                            break;

                        case MixedCodeDocumentFragmentType.Code:
                            s += ((MixedCodeDocumentCodeFragment) frag).Code + "\n";
                            break;
                    }

                return s;
            }
        }

        /// <summary>
        ///     Gets the list of code fragments in the document.
        /// </summary>
        public MixedCodeDocumentFragmentList CodeFragments => _codefragments;

        /// <summary>
        ///     Gets the list of all fragments in the document.
        /// </summary>
        public MixedCodeDocumentFragmentList Fragments => _fragments;

        /// <summary>
        ///     Gets the encoding of the stream used to read the document.
        /// </summary>
        public Encoding StreamEncoding { get; private set; }

        /// <summary>
        ///     Gets the list of text fragments in the document.
        /// </summary>
        public MixedCodeDocumentFragmentList TextFragments => _textfragments;

        #endregion Properties

        #region Public Methods

        /// <summary>
        ///     Create a code fragment instances.
        /// </summary>
        /// <returns>The newly created code fragment instance.</returns>
        public MixedCodeDocumentCodeFragment CreateCodeFragment()
        {
            return (MixedCodeDocumentCodeFragment) CreateFragment(MixedCodeDocumentFragmentType.Code);
        }

        /// <summary>
        ///     Create a text fragment instances.
        /// </summary>
        /// <returns>The newly created text fragment instance.</returns>
        public MixedCodeDocumentTextFragment CreateTextFragment()
        {
            return (MixedCodeDocumentTextFragment) CreateFragment(MixedCodeDocumentFragmentType.Text);
        }

        /// <summary>
        ///     Loads a mixed code document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        public void Load(Stream stream)
        {
            Load(new StreamReader(stream));
        }

        /// <summary>
        ///     Loads a mixed code document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="detectEncodingFromByteOrderMarks">
        ///     Indicates whether to look for byte order marks at the beginning of the
        ///     file.
        /// </param>
        public void Load(Stream stream, bool detectEncodingFromByteOrderMarks)
        {
            Load(new StreamReader(stream, detectEncodingFromByteOrderMarks));
        }

        /// <summary>
        ///     Loads a mixed code document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="encoding">The character encoding to use.</param>
        public void Load(Stream stream, Encoding encoding)
        {
            Load(new StreamReader(stream, encoding));
        }

        /// <summary>
        ///     Loads a mixed code document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">
        ///     Indicates whether to look for byte order marks at the beginning of the
        ///     file.
        /// </param>
        public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
        {
            Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks));
        }

        /// <summary>
        ///     Loads a mixed code document from a stream.
        /// </summary>
        /// <param name="stream">The input stream.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">
        ///     Indicates whether to look for byte order marks at the beginning of the
        ///     file.
        /// </param>
        /// <param name="buffersize">The minimum buffer size.</param>
        public void Load(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
        {
            Load(new StreamReader(stream, encoding, detectEncodingFromByteOrderMarks, buffersize));
        }

        /// <summary>
        ///     Loads a mixed code document from a file.
        /// </summary>
        /// <param name="path">The complete file path to be read.</param>
        public void Load(string path)
        {
#if NETSTANDARD1_3 || NETSTANDARD1_6
            Load(new StreamReader(File.OpenRead(path)));
#else
            Load(new StreamReader(path));
#endif
        }

        /// <summary>
        ///     Loads a mixed code document from a file.
        /// </summary>
        /// <param name="path">The complete file path to be read.</param>
        /// <param name="detectEncodingFromByteOrderMarks">
        ///     Indicates whether to look for byte order marks at the beginning of the
        ///     file.
        /// </param>
        public void Load(string path, bool detectEncodingFromByteOrderMarks)
        {
#if NETSTANDARD1_3 || NETSTANDARD1_6
            Load(new StreamReader(File.OpenRead(path), detectEncodingFromByteOrderMarks));
#else
            Load(new StreamReader(path, detectEncodingFromByteOrderMarks));
#endif
        }

        /// <summary>
        ///     Loads a mixed code document from a file.
        /// </summary>
        /// <param name="path">The complete file path to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        public void Load(string path, Encoding encoding)
        {
#if NETSTANDARD1_3 || NETSTANDARD1_6
            Load(new StreamReader(File.OpenRead(path), encoding));
#else
            Load(new StreamReader(path, encoding));
#endif
        }

        /// <summary>
        ///     Loads a mixed code document from a file.
        /// </summary>
        /// <param name="path">The complete file path to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">
        ///     Indicates whether to look for byte order marks at the beginning of the
        ///     file.
        /// </param>
        public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
        {
#if NETSTANDARD1_3 || NETSTANDARD1_6
            Load(new StreamReader(File.OpenRead(path), encoding, detectEncodingFromByteOrderMarks));
#else
            Load(new StreamReader(path, encoding, detectEncodingFromByteOrderMarks));
#endif
        }

        /// <summary>
        ///     Loads a mixed code document from a file.
        /// </summary>
        /// <param name="path">The complete file path to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">
        ///     Indicates whether to look for byte order marks at the beginning of the
        ///     file.
        /// </param>
        /// <param name="buffersize">The minimum buffer size.</param>
        public void Load(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int buffersize)
        {
#if NETSTANDARD1_3 || NETSTANDARD1_6
            Load(new StreamReader(File.OpenRead(path), encoding, detectEncodingFromByteOrderMarks, buffersize));
#else
            Load(new StreamReader(path, encoding, detectEncodingFromByteOrderMarks, buffersize));
#endif
        }

        /// <summary>
        ///     Loads the mixed code document from the specified TextReader.
        /// </summary>
        /// <param name="reader">The TextReader used to feed the HTML data into the document.</param>
        public void Load(TextReader reader)
        {
            _codefragments.Clear();
            _textfragments.Clear();

            // all pseudo constructors get down to this one
            using (var sr = reader as StreamReader)
            {
                if (sr != null) StreamEncoding = sr.CurrentEncoding;

                _text = reader.ReadToEnd();
            }

            Parse();
        }

        /// <summary>
        ///     Loads a mixed document from a text
        /// </summary>
        /// <param name="html">The text to load.</param>
        public void LoadHtml(string html)
        {
            Load(new StringReader(html));
        }

        /// <summary>
        ///     Saves the mixed document to the specified stream.
        /// </summary>
        /// <param name="outStream">The stream to which you want to save.</param>
        public void Save(Stream outStream)
        {
            var sw = new StreamWriter(outStream, GetOutEncoding());
            Save(sw);
        }

        /// <summary>
        ///     Saves the mixed document to the specified stream.
        /// </summary>
        /// <param name="outStream">The stream to which you want to save.</param>
        /// <param name="encoding">The character encoding to use.</param>
        public void Save(Stream outStream, Encoding encoding)
        {
            var sw = new StreamWriter(outStream, encoding);
            Save(sw);
        }

        /// <summary>
        ///     Saves the mixed document to the specified file.
        /// </summary>
        /// <param name="filename">The location of the file where you want to save the document.</param>
        public void Save(string filename)
        {
#if NETSTANDARD1_3 || NETSTANDARD1_6
            StreamWriter sw = new StreamWriter(File.OpenWrite(filename), GetOutEncoding());
#else
            var sw = new StreamWriter(filename, false, GetOutEncoding());
#endif
            Save(sw);
        }

        /// <summary>
        ///     Saves the mixed document to the specified file.
        /// </summary>
        /// <param name="filename">The location of the file where you want to save the document.</param>
        /// <param name="encoding">The character encoding to use.</param>
        public void Save(string filename, Encoding encoding)
        {
#if NETSTANDARD1_3 || NETSTANDARD1_6
            StreamWriter sw = new StreamWriter(File.OpenWrite(filename), encoding);
#else
            var sw = new StreamWriter(filename, false, encoding);
#endif
            Save(sw);
        }

        /// <summary>
        ///     Saves the mixed document to the specified StreamWriter.
        /// </summary>
        /// <param name="writer">The StreamWriter to which you want to save.</param>
        public void Save(StreamWriter writer)
        {
            Save((TextWriter) writer);
        }

        /// <summary>
        ///     Saves the mixed document to the specified TextWriter.
        /// </summary>
        /// <param name="writer">The TextWriter to which you want to save.</param>
        public void Save(TextWriter writer)
        {
            writer.Flush();
        }

        #endregion Public Methods

        #region Internal Methods

        internal MixedCodeDocumentFragment CreateFragment(MixedCodeDocumentFragmentType type)
        {
            switch (type)
            {
                case MixedCodeDocumentFragmentType.Text:
                    return new MixedCodeDocumentTextFragment(this);

                case MixedCodeDocumentFragmentType.Code:
                    return new MixedCodeDocumentCodeFragment(this);

                default:
                    throw new NotSupportedException();
            }
        }

        internal Encoding GetOutEncoding()
        {
            if (StreamEncoding != null) return StreamEncoding;

            return Encoding.UTF8;
        }

        #endregion Internal Methods

        #region Private Methods

        private void IncrementPosition()
        {
            _index++;
            if (_c == 10)
            {
                _lineposition = 1;
                _line++;
            }
            else
            {
                _lineposition++;
            }
        }

        private void Parse()
        {
            _state = ParseState.Text;
            _index = 0;
            _currentfragment = CreateFragment(MixedCodeDocumentFragmentType.Text);

            while (_index < _text.Length)
            {
                _c = _text[_index];
                IncrementPosition();

                switch (_state)
                {
                    case ParseState.Text:
                        if (_index + TokenCodeStart.Length < _text.Length)
                            if (_text.Substring(_index - 1, TokenCodeStart.Length) == TokenCodeStart)
                            {
                                _state = ParseState.Code;
                                _currentfragment.Length = _index - 1 - _currentfragment.Index;
                                _currentfragment = CreateFragment(MixedCodeDocumentFragmentType.Code);
                                SetPosition();
                            }

                        break;

                    case ParseState.Code:
                        if (_index + TokenCodeEnd.Length < _text.Length)
                            if (_text.Substring(_index - 1, TokenCodeEnd.Length) == TokenCodeEnd)
                            {
                                _state = ParseState.Text;
                                _currentfragment.Length = _index + TokenCodeEnd.Length - _currentfragment.Index;
                                _index += TokenCodeEnd.Length;
                                _lineposition += TokenCodeEnd.Length;
                                _currentfragment = CreateFragment(MixedCodeDocumentFragmentType.Text);
                                SetPosition();
                            }

                        break;
                }
            }

            _currentfragment.Length = _index - _currentfragment.Index;
        }

        private void SetPosition()
        {
            _currentfragment.Line = _line;
            _currentfragment._lineposition = _lineposition;
            _currentfragment.Index = _index - 1;
            _currentfragment.Length = 0;
        }

        #endregion Private Methods
    }
}

#endif