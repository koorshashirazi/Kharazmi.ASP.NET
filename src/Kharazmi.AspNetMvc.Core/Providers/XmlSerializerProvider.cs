using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Kharazmi.AspNetMvc.Core.Helpers;
using Mvc.Utility.Core;

namespace Kharazmi.AspNetMvc.Core.Providers
{
    public interface IXmlSerializerProvider<T> : IList<T>
    {
        void AddRange(IEnumerable<T> items);
        void CreateXmlFile(T item);
        void Save();
    }

    [Serializable]
    public abstract class XmlSerializerProvider<T> : IXmlSerializerProvider<T>
    {
        private readonly List<T> _innerList = new List<T>();
        private readonly XmlSerializer _xmlSer;

        protected XmlSerializerProvider(T item, string pathFile)
        {
            if (string.IsNullOrWhiteSpace(pathFile))
                throw ExceptionHelper.ThrowException<ArgumentNullException>(ShareResources.ArgumentNullException,
                    nameof(pathFile));

            PathFile = pathFile;

            _xmlSer = new XmlSerializer(GetType(), new XmlRootAttribute($"{typeof(T).Name}s"));

            ((IXmlSerializerProvider<T>) this).CreateXmlFile(item);
        }

        protected string PathFile { get; }


        void IXmlSerializerProvider<T>.CreateXmlFile(T item)
        {
            Execute(
                () =>
                {
                    if (item == null)
                        throw ExceptionHelper.ThrowException<ArgumentNullException>(
                            ShareResources.ArgumentNullException, nameof(item));


                    if (File.Exists(PathFile)) return;

                    _innerList.Insert(0, item);

                    using (var sWriter3 =
                        new StreamWriter(
                            new FileStream(PathFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read),
                            Encoding.GetEncoding("iso-8859-7")))
                    {
                        _xmlSer.Serialize(sWriter3, this);
                    }
                });
        }

        void IXmlSerializerProvider<T>.Save()
        {
            Execute(
                () =>
                {
                    LoadAll();
                    Write();
                });
        }

        void IXmlSerializerProvider<T>.AddRange(IEnumerable<T> items)
        {
            Execute(
                () =>
                {
                    if (items == null)
                        throw ExceptionHelper.ThrowException<ArgumentNullException>(
                            ShareResources.ArgumentNullException, nameof(items));

                    _innerList.AddRange(items);
                });
        }


        int ICollection<T>.Count => _innerList.Count;

        bool ICollection<T>.IsReadOnly => false;

        T IList<T>.this[int index]
        {
            get => _innerList[index];
            set => _innerList[index] = value;
        }

        int IList<T>.IndexOf(T item)
        {
            return Execute(
                () =>
                {
                    if (item == null)
                        throw ExceptionHelper.ThrowException<ArgumentNullException>(
                            ShareResources.ArgumentNullException, nameof(item));

                    return _innerList.IndexOf(item);
                });
        }

        void IList<T>.Insert(int index, T item)
        {
            Execute(
                () =>
                {
                    if (item == null)
                        throw ExceptionHelper.ThrowException<ArgumentNullException>(
                            ShareResources.ArgumentNullException, nameof(item));

                    _innerList.Insert(index, item);
                });
        }

        void IList<T>.RemoveAt(int index)
        {
            Execute(() => { _innerList.RemoveAt(index); });
        }

        void ICollection<T>.Add(T item)
        {
            Execute(
                () =>
                {
                    if (item == null)
                        throw ExceptionHelper.ThrowException<ArgumentNullException>(
                            ShareResources.ArgumentNullException, nameof(item));

                    _innerList.Add(item);
                });
        }

        void ICollection<T>.Clear()
        {
            Execute(() => { _innerList.Clear(); });
        }

        bool ICollection<T>.Contains(T item)
        {
            return Execute(
                () =>
                {
                    if (item == null)
                        throw ExceptionHelper.ThrowException<ArgumentNullException>(
                            ShareResources.ArgumentNullException, nameof(item));

                    return _innerList.Contains(item);
                });
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            Execute(
                () =>
                {
                    if (array.Length > 0) _innerList.CopyTo(array, arrayIndex);
                });
        }

        bool ICollection<T>.Remove(T item)
        {
            return Execute(
                () =>
                {
                    if (item == null)
                        throw ExceptionHelper.ThrowException<ArgumentNullException>(
                            ShareResources.ArgumentNullException, nameof(item));

                    return _innerList.Remove(item);
                });
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerList.GetEnumerator();
        }


        public void Add(T item)
        {
            Execute(
                () =>
                {
                    if (item == null)
                        throw ExceptionHelper.ThrowException<ArgumentNullException>(
                            ShareResources.ArgumentNullException, nameof(item));

                    _innerList.Add(item);
                });
        }


        #region // private Methods

        private static void Execute(Action code)
        {
            try
            {
                code.Invoke();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static TResult Execute<TResult>(Func<TResult> code)
        {
            try
            {
                return code.Invoke();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void LoadAll()
        {
            Execute(
                () =>
                {
                    if (!File.Exists(PathFile)) return;

                    ReadAll();
                });
        }

        private void Write()
        {
            using (var sWriter3 = new StreamWriter(PathFile, false, Encoding.GetEncoding("iso-8859-7")))
            {
                if (_innerList != null) _xmlSer.Serialize(sWriter3, this);
            }
        }

        private void ReadAll()
        {
            using (var sReader3 =
                new StreamReader(new FileStream(PathFile, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                var tempList = ((XmlSerializerProvider<T>) _xmlSer.Deserialize(sReader3))._innerList;
                _innerList?.AddRange(tempList);
            }
        }

        #endregion
    }
}