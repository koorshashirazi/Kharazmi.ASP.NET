using System;
using System.Collections.Generic;

namespace Mvc.Utility.Core.Managers.HtmlManager
{
    internal class NameValuePairList
    {
        #region Private Methods

        private void Parse(string text)
        {
            _allPairs.Clear();
            _pairsWithName.Clear();
            if (text == null) return;

            var p = text.Split(';');
            foreach (var pv in p)
            {
                if (pv.Length == 0) continue;

                var onep = pv.Split(new[] {'='}, 2);
                if (onep.Length == 0) continue;

                var nvp = new KeyValuePair<string, string>(onep[0].Trim().ToLowerInvariant(),
                    onep.Length < 2 ? "" : onep[1]);

                _allPairs.Add(nvp);

                // index by name
                List<KeyValuePair<string, string>> al;
                if (!_pairsWithName.TryGetValue(nvp.Key, out al))
                {
                    al = new List<KeyValuePair<string, string>>();
                    _pairsWithName.Add(nvp.Key, al);
                }

                al.Add(nvp);
            }
        }

        #endregion Private Methods

        #region Fields

        internal readonly string Text;
        private readonly List<KeyValuePair<string, string>> _allPairs;
        private readonly Dictionary<string, List<KeyValuePair<string, string>>> _pairsWithName;

        #endregion Fields

        #region Constructors

        internal NameValuePairList() :
            this(null)
        {
        }

        internal NameValuePairList(string text)
        {
            Text = text;
            _allPairs = new List<KeyValuePair<string, string>>();
            _pairsWithName = new Dictionary<string, List<KeyValuePair<string, string>>>();

            Parse(text);
        }

        #endregion Constructors

        #region Internal Methods

        internal static string GetNameValuePairsValue(string text, string name)
        {
            var l = new NameValuePairList(text);
            return l.GetNameValuePairValue(name);
        }

        internal List<KeyValuePair<string, string>> GetNameValuePairs(string name)
        {
            if (name == null) return _allPairs;

            return _pairsWithName.ContainsKey(name) ? _pairsWithName[name] : new List<KeyValuePair<string, string>>();
        }

        internal string GetNameValuePairValue(string name)
        {
            if (name == null) throw new ArgumentNullException();

            var al = GetNameValuePairs(name);
            if (al.Count == 0) return string.Empty;

            // return first item
            return al[0].Value.Trim();
        }

        #endregion Internal Methods
    }
}