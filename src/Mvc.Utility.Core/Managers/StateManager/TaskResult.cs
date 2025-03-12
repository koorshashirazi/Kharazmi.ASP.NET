﻿using System.Threading.Tasks;

namespace Mvc.Utility.Core.Managers.StateManager
{
    internal static class TaskResult
    {
        internal static readonly Task Done = FromResult(1);

        private static Task<T> FromResult<T>(T value)
        {
            var tcs = new TaskCompletionSource<T>();
            tcs.SetResult(value);
            return tcs.Task;
        }
    }
}