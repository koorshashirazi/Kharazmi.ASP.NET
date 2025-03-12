﻿using System;
using System.DirectoryServices.Protocols;
using System.Linq.Expressions;
using Mvc.Utility.Core.Managers.LinqToLdap.Logging;
using Mvc.Utility.Core.Managers.LinqToLdap.Mapping;
using Mvc.Utility.Core.Managers.LinqToLdap.QueryCommands;
using Mvc.Utility.Core.Managers.LinqToLdap.Visitors;

namespace Mvc.Utility.Core.Managers.LinqToLdap
{
    internal class DirectoryQueryProvider : QueryProvider, IDisposable
    {
        private bool _disposed;
        private IObjectMapping _mapping;
        private readonly SearchScope _scope;
#if !NET45
        private WeakReference _connection;
#else
        private WeakReference<LdapConnection> _connection;
#endif
        private readonly bool _pagingEnabled;

        public DirectoryQueryProvider(LdapConnection connection, SearchScope scope, IObjectMapping mapping,
            bool pagingEnabled)
        {
            if (mapping == null) throw new ArgumentNullException("mapping");
            if (connection == null) throw new ArgumentNullException("connection");

            _scope = scope;
#if !NET45
            _connection = new WeakReference(connection);
#else
        _connection = new WeakReference<LdapConnection>(connection);
#endif

            _mapping = mapping;
            _pagingEnabled = pagingEnabled;
        }

        public ILinqToLdapLogger Log { private get; set; }

        public bool IsDynamic { private get; set; }

        public int MaxPageSize { get; set; }

        public string NamingContext { get; set; }

        private IQueryCommand TranslateExpression(Expression expression)
        {
            if (Log != null && Log.TraceEnabled) Log.Trace("Expression: " + expression);

            var translator = new QueryTranslator(_mapping) {IsDynamic = IsDynamic};
            return translator.Translate(expression);
        }

        public override string GetQueryText(Expression expression)
        {
            var translated = TranslateExpression(expression);

            return translated.ToString();
        }

        public override object Execute(Expression expression)
        {
            try
            {
                if (_disposed) throw new ObjectDisposedException(GetType().FullName);

                var command = TranslateExpression(expression);

                LdapConnection connection;
#if !NET45
                if (!_connection.IsAlive || (connection = _connection.Target as LdapConnection) == null)
                    throw new ObjectDisposedException("_connection",
                        "The LdapConnection associated with this provider has been disposed.");
#else
                if (!_connection.TryGetTarget(out connection))
                {
                    throw new ObjectDisposedException("_connection", "The LdapConnection associated with this provider has been disposed.");
                }
#endif
                return command.Execute(connection, _scope, MaxPageSize, _pagingEnabled, Log, NamingContext);
            }
            catch (Exception ex)
            {
                if (Log != null) Log.Error(ex);
                throw;
            }
        }

        ~DirectoryQueryProvider()
        {
            Dispose(false);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            _disposed = true;
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            _mapping = null;
            _connection = null;
            Log = null;
        }
    }
}