﻿using System;
using System.Diagnostics;
using System.Globalization;
using Mvc.Utility.Core.Extensions;
using Mvc.Utility.Core.Helpers;

namespace Mvc.Utility.Core.Models
{
    public static class Error
    {
        [DebuggerStepThrough]
        public static Exception Application(string message, params object[] args)
        {
            return new ApplicationException(message.FormatCurrent(args));
        }

        [DebuggerStepThrough]
        public static Exception Application(Exception innerException, string message, params object[] args)
        {
            return new ApplicationException(message.FormatCurrent(args), innerException);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentNullOrEmpty(string argName)
        {
            return new ArgumentException("String parameter '{0}' cannot be null or empty.", argName);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentNullOrWhiteSpace(string argName)
        {
            return new ArgumentException("String parameter '{0}' cannot be null or all whitespace.", argName);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentNull(string argName)
        {
            return new ArgumentNullException(argName);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentNull<T>(string argName)
        {
            var message = "Argument of type '{0}' cannot be null".FormatInvariant(typeof(T));
            return new ArgumentNullException(argName, message);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange<T>(string argName)
        {
            var message = "Argument of type '{0}' cannot be out of range".FormatInvariant(typeof(T));
            return new ArgumentOutOfRangeException(argName, message);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange(string argName)
        {
            return new ArgumentOutOfRangeException(argName);
        }

        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange(string argName, string message, params object[] args)
        {
            return new ArgumentOutOfRangeException(argName, string.Format(CultureInfo.CurrentCulture, message, args));
        }

        [DebuggerStepThrough]
        public static Exception Argument(string argName, string message, params object[] args)
        {
            return new ArgumentException(string.Format(CultureInfo.CurrentCulture, message, args), argName);
        }


        [DebuggerStepThrough]
        public static Exception InvalidOperation(string message, params object[] args)
        {
            return InvalidOperation(message, null, args);
        }

        [DebuggerStepThrough]
        public static Exception InvalidOperation(string message, Exception innerException, params object[] args)
        {
            return new InvalidOperationException(message.FormatCurrent(args), innerException);
        }

        [DebuggerStepThrough]
        public static Exception InvalidOperation<T>(string message, Func<T> member)
        {
            return InvalidOperation(message, null, member);
        }

        [DebuggerStepThrough]
        public static Exception InvalidOperation<T>(string message, Exception innerException, Func<T> member)
        {
            Guard.ArgumentNotNull(message, nameof(member));
            Guard.ArgumentNotNull(member, nameof(member));

            return new InvalidOperationException(message.FormatCurrent(member.Method.Name), innerException);
        }

        [DebuggerStepThrough]
        public static Exception InvalidCast(Type fromType, Type toType, Exception innerException = null)
        {
            return
                new InvalidCastException(
                    "Cannot convert from type '{0}' to '{1}'.".FormatCurrent(fromType.FullName, toType.FullName),
                    innerException);
        }

        [DebuggerStepThrough]
        public static Exception NotSupported()
        {
            return new NotSupportedException();
        }

        [DebuggerStepThrough]
        public static Exception NotImplemented()
        {
            return new NotImplementedException();
        }

        [DebuggerStepThrough]
        public static Exception ObjectDisposed(string objectName)
        {
            return new ObjectDisposedException(objectName);
        }

        [DebuggerStepThrough]
        public static Exception ObjectDisposed(string objectName, string message, params object[] args)
        {
            return new ObjectDisposedException(objectName, string.Format(CultureInfo.CurrentCulture, message, args));
        }

        [DebuggerStepThrough]
        public static Exception NoElements()
        {
            return new InvalidOperationException("Sequence contains no elements.");
        }

        [DebuggerStepThrough]
        public static Exception MoreThanOneElement()
        {
            return new InvalidOperationException("Sequence contains more than one element.");
        }
    }
}