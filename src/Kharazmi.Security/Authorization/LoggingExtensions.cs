// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Owin.Logging;

namespace Kharazmi.Security.Authorization
{
    [ExcludeFromCodeCoverage]
    internal static class LoggingExtensions
    {
        public static void UserAuthorizationSucceeded(this ILogger logger, string userName)
        {
            Debug.Assert(condition: logger != null, message: "logger != null");
            logger.WriteInformation(message: string.Format(CultureInfo.CurrentCulture, ShareResources.LogAuthorizationSucceededForUser, userName));
        }

        public static void UserAuthorizationFailed(this ILogger logger, string userName)
        {
            Debug.Assert(condition: logger != null, message: "logger != null");
            logger.WriteInformation(message: string.Format(CultureInfo.CurrentCulture, ShareResources.LogAuthorizationFailedForUser, userName));
        }

        public static ILogger CreateDefaultLogger(this ILoggerFactory loggerFactory)
        {
            Debug.Assert(condition: loggerFactory != null);
            return loggerFactory.Create(name: "ResourceAuthorization");
        }
    }
}