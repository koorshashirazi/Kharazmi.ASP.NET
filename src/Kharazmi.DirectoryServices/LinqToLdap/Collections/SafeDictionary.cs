﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap.Collections
{
    internal class SafeDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> _internal = new Dictionary<TKey, TValue>();
        private ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();

        ~SafeDictionary()
        {
            var locker = _locker;
            if (locker != null)
            {
                locker.Dispose();
                _locker = locker = null;
            }

            var dictionary = _internal;
            if (dictionary != null)
            {
                dictionary.Clear();
                _internal = dictionary = null;
            }
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            _locker.EnterReadLock();
            try
            {
                if (_internal.ContainsKey(key)) return _internal[key];
            }
            finally
            {
                _locker.ExitReadLock();
            }

            var value = valueFactory(key);
            _locker.EnterWriteLock();
            try
            {
                if (!_internal.ContainsKey(key)) _internal.Add(key, value);
                return _internal[key];
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }

        public ReadOnlyDictionary<TKey, TValue> ToReadOnly()
        {
            _locker.EnterReadLock();
            try
            {
                return new ReadOnlyDictionary<TKey, TValue>(_internal.ToDictionary(d => d.Key, d => d.Value));
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            _locker.EnterReadLock();
            try
            {
                if (_internal.ContainsKey(key))
                {
                    value = _internal[key];
                    return true;
                }

                value = default;
                return false;
            }
            finally
            {
                _locker.ExitReadLock();
            }
        }

        public void TryAdd(TKey key, TValue value)
        {
            _locker.EnterWriteLock();
            try
            {
                if (!_internal.ContainsKey(key)) _internal.Add(key, value);
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }
    }
}