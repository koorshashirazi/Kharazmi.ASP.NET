// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Owin.Logging;

namespace Mvc.Utility.Core.Managers.Authorization
{
    [ExcludeFromCodeCoverage]
    internal static class LoggingExtensions
    {
        public static void UserAuthorizationSucceeded(this ILogger logger, string userName)
        {
            Debug.Assert(logger != null, "logger != null");
            logger.WriteInformation(string.Format(CultureInfo.CurrentCulture,
                ShareResources.LogAuthorizationSucceededForUser, userName));
        }

        public static void UserAuthorizationFailed(this ILogger logger, string userName)
        {
            Debug.Assert(logger != null, "logger != null");
            logger.WriteInformation(string.Format(CultureInfo.CurrentCulture,
                ShareResources.LogAuthorizationFailedForUser, userName));
        }

        public static ILogger CreateDefaultLogger(this ILoggerFactory loggerFactory)
        {
            Debug.Assert(loggerFactory != null);
            return loggerFactory.Create("ResourceAuthorization");
        }
    }
}