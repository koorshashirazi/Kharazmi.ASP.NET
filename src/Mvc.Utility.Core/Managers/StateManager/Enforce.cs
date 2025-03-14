﻿using System;

namespace Mvc.Utility.Core.Managers.StateManager
{
    internal static class Enforce
    {
        public static T ArgumentNotNull<T>(T argument, string description)
            where T : class
        {
            if (argument == null) throw new ArgumentNullException(description);

            return argument;
        }
    }
}