using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mvc.Utility.Core.Helpers
{
    public static class ThreadingHelper
    {
        private static readonly TaskFactory _taskFactory = new
            TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None,
                TaskScheduler.Default);

        public static void TryLock(Action code, object lockObject, int retryCount, int waitSeconds = 0)
        {
            var tryEnter = false;

            var retries = 0;
            try
            {
                while (retries < retryCount)
                    try
                    {
                        tryEnter = Monitor.TryEnter(lockObject, TimeSpan.FromSeconds(waitSeconds));
                        if (tryEnter)
                        {
                            code.Invoke();
                            break;
                        }
                        else
                        {
                            retries++;
                        }
                    }
                    finally
                    {
                        if (tryEnter) Monitor.Exit(lockObject);
                    }
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.ThrowException<OverflowException>(ex.Message);
            }
        }

        public static T TryLock<T>(Func<T> code, object lockObject, int retryCount, int waitSeconds = 0)
        {
            var tryEnter = false;
            var codeResult = default(T);

            var retries = 0;
            try
            {
                while (retries < retryCount)
                    try
                    {
                        tryEnter = Monitor.TryEnter(lockObject, TimeSpan.FromSeconds(waitSeconds));
                        if (tryEnter)
                        {
                            codeResult = code.Invoke();
                            break;
                        }
                        else
                        {
                            retries++;
                        }
                    }
                    finally
                    {
                        if (tryEnter) Monitor.Exit(lockObject);
                    }
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.ThrowException<OverflowException>(ex.Message);
            }

            return codeResult;
        }

        public static object TryLock(Func<object> code, object lockObject, int retryCount, int waitSeconds = 0)
        {
            var tryEnter = false;
            object codeResult = null;

            var retries = 0;
            try
            {
                while (retries < retryCount)
                    try
                    {
                        tryEnter = Monitor.TryEnter(lockObject, TimeSpan.FromSeconds(waitSeconds));
                        if (tryEnter)
                        {
                            codeResult = code.Invoke();
                            break;
                        }
                        else
                        {
                            retries++;
                        }
                    }
                    finally
                    {
                        if (tryEnter) Monitor.Exit(lockObject);
                    }
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.ThrowException<OverflowException>(ex.Message);
            }

            return codeResult;
        }

        /// <summary>
        ///     Executes an async Task method which has a void return value synchronously
        ///     USAGE: AsyncUtil.RunSync(() => AsyncMethod());
        /// </summary>
        /// <param name="task">Task method to execute</param>
        public static void RunSync(Func<Task> task)
        {
            _taskFactory.StartNew(task).Unwrap().GetAwaiter().GetResult();
        }

        /// <summary>
        ///     Executes an async Task
        ///     <T>
        ///         method which has a T return type synchronously
        ///         USAGE: T result = AsyncUtil.RunSync(() => AsyncMethod<T>());
        /// </summary>
        /// <typeparam name="TResult">Return Type</typeparam>
        /// <param name="task">Task<T> method to execute</param>
        /// <returns></returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> task)
        {
            return _taskFactory.StartNew(task).Unwrap().GetAwaiter().GetResult();
        }
    }
}