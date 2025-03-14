﻿using System;
using System.Threading.Tasks;
using Kharazmi.AspNetMvc.Core.Managers.ResultManager;

namespace Kharazmi.AspNetMvc.Core.Managers.NullManager
{
    public static class MaybeExtensions
    {
        public static Result<T> ToResult<T>(this Maybe<T> maybe, string errorMessage)
        {
            if (maybe.HasNoValue) return Result.Fail<T>(errorMessage);

            return Result.Ok(maybe.Value);
        }

        public static Result<T, E> ToResult<T, E>(this Maybe<T> maybe, E error)
        {
            if (maybe.HasNoValue) return Result.Fail<T, E>(error);

            return Result.Ok<T, E>(maybe.Value);
        }

        public static T Unwrap<T>(this Maybe<T> maybe, T defaultValue = default)
        {
            return maybe.Unwrap(x => x, defaultValue);
        }

        public static K Unwrap<T, K>(this Maybe<T> maybe, Func<T, K> selector, K defaultValue = default)
        {
            if (maybe.HasValue) return selector(maybe.Value);

            return defaultValue;
        }

        public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate)
        {
            if (maybe.HasNoValue) return Maybe<T>.None;

            if (predicate(maybe.Value)) return maybe;

            return Maybe<T>.None;
        }

        public static Maybe<K> Select<T, K>(this Maybe<T> maybe, Func<T, K> selector)
        {
            if (maybe.HasNoValue) return Maybe<K>.None;

            return selector(maybe.Value);
        }

        public static Maybe<K> Select<T, K>(this Maybe<T> maybe, Func<T, Maybe<K>> selector)
        {
            if (maybe.HasNoValue) return Maybe<K>.None;

            return selector(maybe.Value);
        }

        public static Maybe<V> SelectMany<T, U, V>(this Maybe<T> maybe,
            Func<T, Maybe<U>> selector,
            Func<T, U, V> project)
        {
            return maybe.Unwrap(
                x =>
                    selector(x)
                        .Unwrap(u => project(x, u), Maybe<V>.None),
                Maybe<V>.None);
        }

        public static void Execute<T>(this Maybe<T> maybe, Action<T> action)
        {
            if (maybe.HasNoValue) return;

            action(maybe.Value);
        }

        public static async Task<Result<T>> ToResult<T>(this Task<Maybe<T>> maybeTask, string errorMessage)
            where T : class
        {
            var maybe = await maybeTask.ConfigureAwait(Result.DefaultConfigureAwait);
            return maybe.ToResult(errorMessage);
        }

        public static async Task<Result<T, E>> ToResult<T, E>(this Task<Maybe<T>> maybeTask, E error) where T : class
        {
            var maybe = await maybeTask.ConfigureAwait(Result.DefaultConfigureAwait);
            return maybe.ToResult(error);
        }
    }
}