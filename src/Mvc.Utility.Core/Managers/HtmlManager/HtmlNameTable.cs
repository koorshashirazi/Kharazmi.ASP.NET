using System.Xml;

namespace Mvc.Utility.Core.Managers.HtmlManager
{
    internal class HtmlNameTable : XmlNameTable
    {
        #region Fields

        private readonly NameTable _nametable = new NameTable();

        #endregion Fields

        #region Internal Methods

        internal string GetOrAdd(string array)
        {
            var s = Get(array);
            if (s == null) return Add(array);

            return s;
        }

        #endregion Internal Methods

        #region Public Methods

        public override string Add(string array)
        {
            return _nametable.Add(array);
        }

        public override string Add(char[] array, int offset, int length)
        {
            return _nametable.Add(array, offset, length);
        }

        public override string Get(string array)
        {
            return _nametable.Get(array);
        }

        public override string Get(char[] array, int offset, int length)
        {
            return _nametable.Get(array, offset, length);
        }

        #endregion Public Methods
    }
}