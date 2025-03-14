using System;
using System.Threading.Tasks;

namespace Mvc.Utility.Core.Managers.ResultManager
{
    /// <summary>
    ///     Extentions for async operations where the task appears in the both operands
    /// </summary>
    public static class AsyncResultExtensionsBothOperands
    {
        public static async Task<Result<K, E>> OnSuccess<T, K, E>(this Task<Result<T, E>> resultTask,
            Func<T, Task<K>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) return Result.Fail<K, E>(result.Error);

            var value = await func(result.Value)
                .ConfigureAwait(Result.DefaultConfigureAwait);

            return Result.Ok<K, E>(value);
        }

        public static async Task<Result<K>> OnSuccess<T, K>(this Task<Result<T>> resultTask, Func<T, Task<K>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) return Result.Fail<K>(result.Error);

            var value = await func(result.Value)
                .ConfigureAwait(Result.DefaultConfigureAwait);

            return Result.Ok(value);
        }

        public static async Task<Result<T>> OnSuccess<T>(this Task<Result> resultTask, Func<Task<T>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) return Result.Fail<T>(result.Error);

            var value = await func().ConfigureAwait(Result.DefaultConfigureAwait);

            return Result.Ok(value);
        }

        public static async Task<Result<K, E>> OnSuccess<T, K, E>(this Task<Result<T, E>> resultTask,
            Func<T, Task<Result<K, E>>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) return Result.Fail<K, E>(result.Error);

            return await func(result.Value).ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<Result<K>> OnSuccess<T, K>(this Task<Result<T>> resultTask,
            Func<T, Task<Result<K>>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) return Result.Fail<K>(result.Error);

            return await func(result.Value).ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<Result<T>> OnSuccess<T>(this Task<Result> resultTask, Func<Task<Result<T>>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) return Result.Fail<T>(result.Error);

            return await func().ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<Result<K, E>> OnSuccess<T, K, E>(this Task<Result<T, E>> resultTask,
            Func<Task<Result<K, E>>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) return Result.Fail<K, E>(result.Error);

            return await func().ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<Result<K>> OnSuccess<T, K>(this Task<Result<T>> resultTask, Func<Task<Result<K>>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) return Result.Fail<K>(result.Error);

            return await func().ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<Result> OnSuccess<T>(this Task<Result<T>> resultTask, Func<T, Task<Result>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) return Result.Fail(result.Error);

            return await func(result.Value).ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<Result> OnSuccess(this Task<Result> resultTask, Func<Task<Result>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) return result;

            return await func().ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<Result<T>> Ensure<T>(this Task<Result<T>> resultTask, Func<T, Task<bool>> predicate,
            string errorMessage)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) return result;

            if (!await predicate(result.Value)
                .ConfigureAwait(Result.DefaultConfigureAwait))
                return Result.Fail<T>(errorMessage);

            return result;
        }

        public static async Task<Result<T, E>> Ensure<T, E>(this Task<Result<T, E>> resultTask,
            Func<T, Task<bool>> predicate, E error)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) return result;

            if (!await predicate(result.Value)
                .ConfigureAwait(Result.DefaultConfigureAwait))
                return Result.Fail<T, E>(error);

            return result;
        }

        public static async Task<Result> Ensure(this Task<Result> resultTask, Func<Task<bool>> predicate,
            string errorMessage)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) return result;

            if (!await predicate().ConfigureAwait(Result.DefaultConfigureAwait)) return Result.Fail(errorMessage);

            return result;
        }

        public static Task<Result<K, E>> Map<T, K, E>(this Task<Result<T, E>> resultTask,
            Func<T, Task<K>> func)
        {
            return resultTask.OnSuccess(func);
        }

        public static Task<Result<K>> Map<T, K>(this Task<Result<T>> resultTask, Func<T, Task<K>> func)
        {
            return resultTask.OnSuccess(func);
        }

        public static Task<Result<T>> Map<T>(this Task<Result> resultTask, Func<Task<T>> func)
        {
            return resultTask.OnSuccess(func);
        }


        public static async Task<Result<T, E>> OnSuccess<T, E>(this Task<Result<T, E>> resultTask,
            Func<T, Task> action)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsSuccess) await action(result.Value).ConfigureAwait(Result.DefaultConfigureAwait);

            return result;
        }

        public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> resultTask, Func<T, Task> action)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsSuccess) await action(result.Value).ConfigureAwait(Result.DefaultConfigureAwait);

            return result;
        }

        public static async Task<Result> OnSuccess(this Task<Result> resultTask, Func<Task> action)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsSuccess) await action().ConfigureAwait(Result.DefaultConfigureAwait);

            return result;
        }

        public static async Task<T> OnBoth<T>(this Task<Result> resultTask, Func<Result, Task<T>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
            return await func(result).ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<K> OnBoth<T, K>(this Task<Result<T>> resultTask, Func<Result<T>, Task<K>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
            return await func(result).ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<K> OnBoth<T, K, E>(this Task<Result<T, E>> resultTask,
            Func<Result<T, E>, Task<K>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);
            return await func(result).ConfigureAwait(Result.DefaultConfigureAwait);
        }

        public static async Task<Result<T, E>> OnFailure<T, E>(this Task<Result<T, E>> resultTask,
            Func<Task> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) await func().ConfigureAwait(Result.DefaultConfigureAwait);

            return result;
        }

        public static async Task<Result<T>> OnFailure<T>(this Task<Result<T>> resultTask, Func<Task> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) await func().ConfigureAwait(Result.DefaultConfigureAwait);

            return result;
        }

        public static async Task<Result> OnFailure(this Task<Result> resultTask, Func<Task> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) await func().ConfigureAwait(Result.DefaultConfigureAwait);

            return result;
        }

        public static async Task<Result<T>> OnFailure<T>(this Task<Result<T>> resultTask, Func<string, Task> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) await func(result.Error).ConfigureAwait(Result.DefaultConfigureAwait);

            return result;
        }

        public static async Task<Result<T, E>> OnFailure<T, E>(this Task<Result<T, E>> resultTask,
            Func<E, Task> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) await func(result.Error).ConfigureAwait(Result.DefaultConfigureAwait);

            return result;
        }

        public static async Task<Result<T, E>> OnFailureCompensate<T, E>(this Task<Result<T, E>> resultTask,
            Func<Task<Result<T, E>>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) return await func().ConfigureAwait(Result.DefaultConfigureAwait);

            return result;
        }

        public static async Task<Result<T>> OnFailureCompensate<T>(this Task<Result<T>> resultTask,
            Func<Task<Result<T>>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) return await func().ConfigureAwait(Result.DefaultConfigureAwait);

            return result;
        }

        public static async Task<Result> OnFailureCompensate(this Task<Result> resultTask, Func<Task<Result>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure) return await func().ConfigureAwait(Result.DefaultConfigureAwait);

            return result;
        }

        public static async Task<Result<T>> OnFailureCompensate<T>(this Task<Result<T>> resultTask,
            Func<string, Task<Result<T>>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure)
                return await func(result.Error)
                    .ConfigureAwait(Result.DefaultConfigureAwait);

            return result;
        }

        public static async Task<Result<T, E>> OnFailureCompensate<T, E>(this Task<Result<T, E>> resultTask,
            Func<E, Task<Result<T, E>>> func)
        {
            var result = await resultTask.ConfigureAwait(Result.DefaultConfigureAwait);

            if (result.IsFailure)
                return await func(result.Error)
                    .ConfigureAwait(Result.DefaultConfigureAwait);

            return result;
        }
    }
}