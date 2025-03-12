//#if NETSTANDARD2_0

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Transactions;

namespace Mvc.Utility.Core.Managers.ResultManager
{
    public static partial class ResultExtensions
    {
        // Non-async extensions
        [DebuggerStepThrough]
        public static Result<K> OnSuccessWithTransactionScope<T, K>(this Result<T> self, Func<T, K> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Result<T> OnSuccessWithTransactionScope<T>(this Result self, Func<T> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Result<K> OnSuccessWithTransactionScope<T, K>(this Result<T> self, Func<T, Result<K>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Result<T> OnSuccessWithTransactionScope<T>(this Result self, Func<Result<T>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Result<K> OnSuccessWithTransactionScope<T, K>(this Result<T> self, Func<Result<K>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Result OnSuccessWithTransactionScope<T>(this Result<T> self, Func<T, Result> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Result OnSuccessWithTransactionScope(this Result self, Func<Result> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }


        // Async - Both Operands
        [DebuggerStepThrough]
        public static Task<Result<K>> OnSuccessWithTransactionScope<T, K>(this Task<Result<T>> self, Func<T, Task<K>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result<T>> OnSuccessWithTransactionScope<T>(this Task<Result> self, Func<Task<T>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result<K>> OnSuccessWithTransactionScope<T, K>(this Task<Result<T>> self,
            Func<T, Task<Result<K>>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result<T>> OnSuccessWithTransactionScope<T>(this Task<Result> self, Func<Task<Result<T>>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result<K>> OnSuccessWithTransactionScope<T, K>(this Task<Result<T>> self,
            Func<Task<Result<K>>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result> OnSuccessWithTransactionScope<T>(this Task<Result<T>> self, Func<T, Task<Result>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result> OnSuccessWithTransactionScope(this Task<Result> self, Func<Task<Result>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }


        // Async - Left Operands
        [DebuggerStepThrough]
        public static Task<Result<K>> OnSuccessWithTransactionScope<T, K>(this Task<Result<T>> self, Func<T, K> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result<T>> OnSuccessWithTransactionScope<T>(this Task<Result> self, Func<T> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result<K>> OnSuccessWithTransactionScope<T, K>(this Task<Result<T>> self,
            Func<T, Result<K>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result<T>> OnSuccessWithTransactionScope<T>(this Task<Result> self, Func<Result<T>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result<K>> OnSuccessWithTransactionScope<T, K>(this Task<Result<T>> self, Func<Result<K>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result> OnSuccessWithTransactionScope<T>(this Task<Result<T>> self, Func<T, Result> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result> OnSuccessWithTransactionScope(this Task<Result> self, Func<Result> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result<K>> MapWithTransactionScope<T, K>(this Task<Result<T>> self, Func<T, K> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result<T>> MapWithTransactionScope<T>(this Task<Result> self, Func<T> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }


        // Async - Right Operands
        [DebuggerStepThrough]
        public static Task<Result<K>> OnSuccessWithTransactionScope<T, K>(this Result<T> self, Func<T, Task<K>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result<T>> OnSuccessWithTransactionScope<T>(this Result self, Func<Task<T>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result<K>> OnSuccessWithTransactionScope<T, K>(this Result<T> self,
            Func<T, Task<Result<K>>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result<T>> OnSuccessWithTransactionScope<T>(this Result self, Func<Task<Result<T>>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result<K>> OnSuccessWithTransactionScope<T, K>(this Result<T> self, Func<Task<Result<K>>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result> OnSuccessWithTransactionScope<T>(this Result<T> self, Func<T, Task<Result>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        [DebuggerStepThrough]
        public static Task<Result> OnSuccessWithTransactionScope(this Result self, Func<Task<Result>> f)
        {
            return WithTransactionScope(() => self.OnSuccess(f));
        }

        private static T WithTransactionScope<T>(Func<T> f) where T : IResult
        {
            using (var trans = new TransactionScope(TransactionScopeOption.Required))
            {
                var result = f();
                if (result.IsSuccess) trans.Complete();

                return result;
            }
        }

        private static async Task<T> WithTransactionScope<T>(Func<Task<T>> f) where T : IResult
        {
            using (var trans = new TransactionScope(TransactionScopeOption.Required))
            {
                var result = await f().ConfigureAwait(Result.DefaultConfigureAwait);
                if (result.IsSuccess) trans.Complete();

                return result;
            }
        }
    }
}
//#endif