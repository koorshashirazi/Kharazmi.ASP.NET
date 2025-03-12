using System.Collections;
using System.Collections.Generic;

namespace Mvc.Utility.Core.Managers.NullManager
{
    /// <summary>
    ///     Usage:
    ///     Example usage:
    ///     return model == null ? NullOption<Model>.Empty() : NullOption<Model>.Value(user);
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NullOption<T> : IEnumerable<T>
    {
        private readonly T[] _data;

        private NullOption(T[] data)
        {
            _data = data;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>) _data).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static NullOption<T> Value(T element)
        {
            return new NullOption<T>(new[] {element});
        }

        public static NullOption<T> Empty()
        {
            return new NullOption<T>(new T[0]);
        }
    }
}