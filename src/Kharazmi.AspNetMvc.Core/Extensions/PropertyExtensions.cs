using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Kharazmi.AspNetMvc.Core.Extensions
{
    public static partial class Common
    {
        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MemberInfo GetMember<T>(this Expression<Func<T, object>> expression)
        {
            var mbody = expression.Body as MemberExpression;

            if (mbody != null) return mbody.Member;

            //This will handle Nullable<T> properties.
            if (expression.Body is UnaryExpression ubody) mbody = ubody.Operand as MemberExpression;

            if (mbody == null) throw new ArgumentException(@"Expression is not a MemberExpression", nameof(expression));

            return mbody.Member;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string PropertyName<T>(this Expression<Func<T, object>> expression)
        {
            return GetMember(expression).Name;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string PropertyDisplay<T>(this Expression<Func<T, object>> expression)
        {
            var propertyMember = GetMember(expression);
            var displayAttributes = propertyMember.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            return displayAttributes.Length == 1
                ? ((DisplayNameAttribute) displayAttributes[0]).DisplayName
                : propertyMember.Name;
        }
    }
}