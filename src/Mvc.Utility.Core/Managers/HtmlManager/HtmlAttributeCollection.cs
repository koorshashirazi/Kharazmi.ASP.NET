using System;
using System.Collections;
using System.Collections.Generic;

namespace Mvc.Utility.Core.Managers.HtmlManager
{
    /// <summary>
    ///     Represents a combined list and collection of HTML nodes.
    /// </summary>
    public class HtmlAttributeCollection : IList<HtmlAttribute>
    {
        #region Constructors

        internal HtmlAttributeCollection(HtmlNode ownernode)
        {
            _ownernode = ownernode;
        }

        #endregion Constructors

        #region Fields

        internal Dictionary<string, HtmlAttribute> Hashitems =
            new Dictionary<string, HtmlAttribute>(StringComparer.OrdinalIgnoreCase);

        private readonly HtmlNode _ownernode;
        private readonly List<HtmlAttribute> _items = new List<HtmlAttribute>();

        #endregion Fields

        #region IList<HtmlAttribute> Members

        /// <summary>
        ///     Gets the number of elements actually contained in the list.
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        ///     Gets readonly status of colelction
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        ///     Gets the attribute at the specified index.
        /// </summary>
        public HtmlAttribute this[int index]
        {
            get => _items[index];
            set
            {
                var oldValue = _items[index];

                _items[index] = value;

                if (oldValue.Name != value.Name) Hashitems.Remove(oldValue.Name);

                Hashitems[value.Name] = value;

                value._ownernode = _ownernode;
                _ownernode.SetChanged();
            }
        }

        /// <summary>
        ///     Gets a given attribute from the list using its name.
        /// </summary>
        public HtmlAttribute this[string name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));

                return Hashitems.TryGetValue(name, out var value) ? value : null;
            }
            set
            {
                if (!Hashitems.TryGetValue(name, out var currentValue)) Append(value);

                this[_items.IndexOf(currentValue)] = value;
            }
        }

        /// <summary>
        ///     Adds a new attribute to the collection with the given values
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void Add(string name, string value)
        {
            Append(name, value);
        }

        /// <summary>
        ///     Adds supplied item to collection
        /// </summary>
        /// <param name="item"></param>
        public void Add(HtmlAttribute item)
        {
            Append(item);
        }

        /// <summary>Adds a range supplied items to collection.</summary>
        /// <param name="items">An IEnumerable&lt;HtmlAttribute&gt; of items to append to this.</param>
        public void AddRange(IEnumerable<HtmlAttribute> items)
        {
            foreach (var item in items) Append(item);
        }

        /// <summary>Adds a range supplied items to collection using a dictionary.</summary>
        /// <param name="items">A Dictionary&lt;string,string&gt; of items to append to this.</param>
        public void AddRange(Dictionary<string, string> items)
        {
            foreach (var item in items) Add(item.Key, item.Value);
        }

        /// <summary>
        ///     Explicit clear
        /// </summary>
        void ICollection<HtmlAttribute>.Clear()
        {
            _items.Clear();
        }

        /// <summary>
        ///     Retreives existence of supplied item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(HtmlAttribute item)
        {
            return _items.Contains(item);
        }

        /// <summary>
        ///     Copies collection to array
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(HtmlAttribute[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///     Get Explicit enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator<HtmlAttribute> IEnumerable<HtmlAttribute>.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        ///     Explicit non-generic enumerator
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        ///     Retrieves the index for the supplied item, -1 if not found
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(HtmlAttribute item)
        {
            return _items.IndexOf(item);
        }

        /// <summary>
        ///     Inserts given item into collection at supplied index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, HtmlAttribute item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            Hashitems[item.Name] = item;
            item._ownernode = _ownernode;
            _items.Insert(index, item);

            _ownernode.SetChanged();
        }

        /// <summary>
        ///     Explicit collection remove
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool ICollection<HtmlAttribute>.Remove(HtmlAttribute item)
        {
            return _items.Remove(item);
        }

        /// <summary>
        ///     Removes the attribute at the specified index.
        /// </summary>
        /// <param name="index">The index of the attribute to remove.</param>
        public void RemoveAt(int index)
        {
            var att = _items[index];
            Hashitems.Remove(att.Name);
            _items.RemoveAt(index);

            _ownernode.SetChanged();
        }

        #endregion IList<HtmlAttribute> Members

        #region Public Methods

        /// <summary>
        ///     Inserts the specified attribute as the last attribute in the collection.
        /// </summary>
        /// <param name="newAttribute">The attribute to insert. May not be null.</param>
        /// <returns>The appended attribute.</returns>
        public HtmlAttribute Append(HtmlAttribute newAttribute)
        {
            if (_ownernode.NodeType == HtmlNodeType.Text || _ownernode.NodeType == HtmlNodeType.Comment)
                throw new Exception("A Text or Comment node cannot have attributes.");

            if (newAttribute == null) throw new ArgumentNullException(nameof(newAttribute));

            Hashitems[newAttribute.Name] = newAttribute;
            newAttribute._ownernode = _ownernode;
            _items.Add(newAttribute);

            _ownernode.SetChanged();
            return newAttribute;
        }

        /// <summary>
        ///     Creates and inserts a new attribute as the last attribute in the collection.
        /// </summary>
        /// <param name="name">The name of the attribute to insert.</param>
        /// <returns>The appended attribute.</returns>
        public HtmlAttribute Append(string name)
        {
            var att = _ownernode._ownerdocument.CreateAttribute(name);
            return Append(att);
        }

        /// <summary>
        ///     Creates and inserts a new attribute as the last attribute in the collection.
        /// </summary>
        /// <param name="name">The name of the attribute to insert.</param>
        /// <param name="value">The value of the attribute to insert.</param>
        /// <returns>The appended attribute.</returns>
        public HtmlAttribute Append(string name, string value)
        {
            var att = _ownernode._ownerdocument.CreateAttribute(name, value);
            return Append(att);
        }

        /// <summary>
        ///     Checks for existance of attribute with given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name)
        {
            for (var i = 0; i < _items.Count; i++)
                if (string.Equals(_items[i].Name, name, StringComparison.OrdinalIgnoreCase))
                    return true;

            return false;
        }

        /// <summary>
        ///     Inserts the specified attribute as the first node in the collection.
        /// </summary>
        /// <param name="newAttribute">The attribute to insert. May not be null.</param>
        /// <returns>The prepended attribute.</returns>
        public HtmlAttribute Prepend(HtmlAttribute newAttribute)
        {
            Insert(0, newAttribute);
            return newAttribute;
        }

        /// <summary>
        ///     Removes a given attribute from the list.
        /// </summary>
        /// <param name="attribute">The attribute to remove. May not be null.</param>
        public void Remove(HtmlAttribute attribute)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));

            var index = GetAttributeIndex(attribute);
            if (index == -1) throw new IndexOutOfRangeException();

            RemoveAt(index);
        }

        /// <summary>
        ///     Removes an attribute from the list, using its name. If there are more than one attributes with this name, they will
        ///     all be removed.
        /// </summary>
        /// <param name="name">The attribute's name. May not be null.</param>
        public void Remove(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            for (var i = 0; i < _items.Count; i++)
            {
                var att = _items[i];
                if (string.Equals(att.Name, name, StringComparison.OrdinalIgnoreCase)) RemoveAt(i);
            }
        }

        /// <summary>
        ///     Remove all attributes in the list.
        /// </summary>
        public void RemoveAll()
        {
            Hashitems.Clear();
            _items.Clear();

            _ownernode.SetChanged();
        }

        #endregion Public Methods

        #region LINQ Methods

        /// <summary>
        ///     Returns all attributes with specified name. Handles case insentivity
        /// </summary>
        /// <param name="attributeName">Name of the attribute</param>
        /// <returns></returns>
        public IEnumerable<HtmlAttribute> AttributesWithName(string attributeName)
        {
            for (var i = 0; i < _items.Count; i++)
                if (string.Equals(_items[i].Name, attributeName, StringComparison.OrdinalIgnoreCase))
                    yield return _items[i];
        }

        /// <summary>
        ///     Removes all attributes from the collection
        /// </summary>
        public void Remove()
        {
            _items.Clear();
        }

        #endregion LINQ Methods

        #region Internal Methods

        /// <summary>
        ///     Clears the attribute collection
        /// </summary>
        internal void Clear()
        {
            Hashitems.Clear();
            _items.Clear();
        }

        internal int GetAttributeIndex(HtmlAttribute attribute)
        {
            if (attribute == null) throw new ArgumentNullException(nameof(attribute));

            for (var i = 0; i < _items.Count; i++)
                if (_items[i] == attribute)
                    return i;

            return -1;
        }

        internal int GetAttributeIndex(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            for (var i = 0; i < _items.Count; i++)
                if (string.Equals(_items[i].Name, name, StringComparison.OrdinalIgnoreCase))
                    return i;

            return -1;
        }

        #endregion Internal Methods
    }
}