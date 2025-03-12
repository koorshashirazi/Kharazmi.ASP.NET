﻿using System;
using System.DirectoryServices.Protocols;

namespace Mvc.Utility.Core.Managers.LinqToLdap.EventListeners
{
    /// <summary>
    ///     Arguments passed for <see cref="IEventListener" />s. The arguments used by this class are
    ///     <see cref="WeakReference" />s.
    /// </summary>
    /// <typeparam name="TObject">The instance for the event.</typeparam>
    /// <typeparam name="TResponse">The response from the Directory.</typeparam>
    /// <typeparam name="TRequest">The full request that was sent to the server.</typeparam>
    public class ListenerPostArgs<TObject, TRequest, TResponse>
        where TRequest : DirectoryRequest
        where TResponse : DirectoryResponse
        where TObject : class
    {
#if !NET45
        private readonly WeakReference _connection;
        private readonly WeakReference _entry;
#else
        private readonly WeakReference<LdapConnection> _connection;
        private readonly WeakReference<TObject> _entry;
#endif

        internal ListenerPostArgs(TObject entry, TRequest request, TResponse response, LdapConnection connection)
        {
#if !NET45
            _entry = new WeakReference(entry);
            _connection = new WeakReference(connection);
#else
            _entry = new WeakReference<TObject>(entry);
            _connection = new WeakReference<LdapConnection>(connection);
#endif
            Request = request;
            Response = response;
        }

        /// <summary>
        ///     The entry.
        /// </summary>
        public TObject Entry
        {
            get
            {
#if !NET45
                return _entry.Target as TObject;
#else
                TObject target;
                return _entry.TryGetTarget(out target) ? target : default(TObject);
#endif
            }
        }

        /// <summary>
        ///     The <see cref="DirectoryRequest" />.
        /// </summary>
        public TRequest Request { get; }

        /// <summary>
        ///     The <see cref="DirectoryResponse" />.
        /// </summary>
        public TResponse Response { get; }

        /// <summary>
        ///     The connection that will be used when sending the <see cref="Request" />.
        /// </summary>
        public LdapConnection Connection
        {
            get
            {
#if !NET45
                return _connection.Target as LdapConnection;
#else
                LdapConnection target;
                return _connection.TryGetTarget(out target) ? target : null;
#endif
            }
        }
    }
}