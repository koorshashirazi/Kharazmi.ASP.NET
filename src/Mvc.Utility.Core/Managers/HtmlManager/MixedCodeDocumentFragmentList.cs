using System;
using System.Collections.Generic;
#if !METRO
using System.Collections;

namespace Mvc.Utility.Core.Managers.HtmlManager
{
    /// <summary>
    ///     Represents a list of mixed code fragments.
    /// </summary>
    public class MixedCodeDocumentFragmentList : IEnumerable
    {
        #region Fields

        private readonly IList<MixedCodeDocumentFragment> _items = new List<MixedCodeDocumentFragment>();

        #endregion Fields

        #region Constructors

        internal MixedCodeDocumentFragmentList(MixedCodeDocument doc)
        {
            Doc = doc;
        }

        #endregion Constructors

        #region IEnumerable Members

        /// <summary>
        ///     Gets an enumerator that can iterate through the fragment list.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion IEnumerable Members

        #region Nested type: MixedCodeDocumentFragmentEnumerator

        /// <summary>
        ///     Represents a fragment enumerator.
        /// </summary>
        public class MixedCodeDocumentFragmentEnumerator : IEnumerator
        {
            #region Constructors

            internal MixedCodeDocumentFragmentEnumerator(IList<MixedCodeDocumentFragment> items)
            {
                _items = items;
                _index = -1;
            }

            #endregion Constructors

            #region Properties

            /// <summary>
            ///     Gets the current element in the collection.
            /// </summary>
            public MixedCodeDocumentFragment Current => _items[_index];

            #endregion Properties

            #region Fields

            private int _index;
            private readonly IList<MixedCodeDocumentFragment> _items;

            #endregion Fields

            #region IEnumerator Members

            /// <summary>
            ///     Gets the current element in the collection.
            /// </summary>
            object IEnumerator.Current => Current;

            /// <summary>
            ///     Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            ///     true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
            ///     end of the collection.
            /// </returns>
            public bool MoveNext()
            {
                _index++;
                return _index < _items.Count;
            }

            /// <summary>
            ///     Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                _index = -1;
            }

            #endregion IEnumerator Members
        }

        #endregion Nested type: MixedCodeDocumentFragmentEnumerator

        #region Properties

        /// <summary>
        ///     Gets the Document
        /// </summary>
        public MixedCodeDocument Doc { get; }

        /// <summary>
        ///     Gets the number of fragments contained in the list.
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        ///     Gets a fragment from the list using its index.
        /// </summary>
        public MixedCodeDocumentFragment this[int index] => _items[index];

        #endregion Properties

        #region Public Methods

        /// <summary>
        ///     Appends a fragment to the list of fragments.
        /// </summary>
        /// <param name="newFragment">The fragment to append. May not be null.</param>
        public void Append(MixedCodeDocumentFragment newFragment)
        {
            if (newFragment == null) throw new ArgumentNullException("newFragment");

            _items.Add(newFragment);
        }

        /// <summary>
        ///     Gets an enumerator that can iterate through the fragment list.
        /// </summary>
        public MixedCodeDocumentFragmentEnumerator GetEnumerator()
        {
            return new MixedCodeDocumentFragmentEnumerator(_items);
        }

        /// <summary>
        ///     Prepends a fragment to the list of fragments.
        /// </summary>
        /// <param name="newFragment">The fragment to append. May not be null.</param>
        public void Prepend(MixedCodeDocumentFragment newFragment)
        {
            if (newFragment == null) throw new ArgumentNullException("newFragment");

            _items.Insert(0, newFragment);
        }

        /// <summary>
        ///     Remove a fragment from the list of fragments. If this fragment was not in the list, an exception will be raised.
        /// </summary>
        /// <param name="fragment">The fragment to remove. May not be null.</param>
        public void Remove(MixedCodeDocumentFragment fragment)
        {
            if (fragment == null) throw new ArgumentNullException("fragment");

            var index = GetFragmentIndex(fragment);
            if (index == -1) throw new IndexOutOfRangeException();

            RemoveAt(index);
        }

        /// <summary>
        ///     Remove all fragments from the list.
        /// </summary>
        public void RemoveAll()
        {
            _items.Clear();
        }

        /// <summary>
        ///     Remove a fragment from the list of fragments, using its index in the list.
        /// </summary>
        /// <param name="index">The index of the fragment to remove.</param>
        public void RemoveAt(int index)
        {
            //MixedCodeDocumentFragment frag = (MixedCodeDocumentFragment) _items[index];
            _items.RemoveAt(index);
        }

        #endregion Public Methods

        #region Internal Methods

        internal void Clear()
        {
            _items.Clear();
        }

        internal int GetFragmentIndex(MixedCodeDocumentFragment fragment)
        {
            if (fragment == null) throw new ArgumentNullException("fragment");

            for (var i = 0; i < _items.Count; i++)
                if (_items[i] == fragment)
                    return i;

            return -1;
        }

        #endregion Internal Methods
    }
}

#endif