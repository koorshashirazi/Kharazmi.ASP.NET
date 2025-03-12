using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap
{
    internal class DirectoryQuery<T> : IOrderedQueryable<T>
    {
        private readonly QueryProvider _provider;

        public DirectoryQuery(QueryProvider provider)
        {
            if (provider == null) throw new ArgumentNullException("provider");

            _provider = provider;
            Expression = Expression.Constant(this);
        }

        public DirectoryQuery(QueryProvider provider, Expression expression)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            if (expression == null) throw new ArgumentNullException("expression");

            _provider = provider;
            Expression = expression;
        }

        /// <summary>
        ///     Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable" />.
        /// </summary>
        /// <returns>
        ///     The <see cref="T:System.Linq.Expressions.Expression" /> that is associated with this instance of
        ///     <see cref="T:System.Linq.IQueryable" />.
        /// </returns>
        public Expression Expression { get; }

        /// <summary>
        ///     Gets the type of the element(s) that are returned when the expression tree associated with this instance of
        ///     <see cref="T:System.Linq.IQueryable" /> is executed.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Type" /> that represents the type of the element(s) that are returned when the expression
        ///     tree associated with this object is executed.
        /// </returns>
        public Type ElementType => typeof(T);

        /// <summary>
        ///     Gets the query provider that is associated with this data source.
        /// </summary>
        /// <returns>
        ///     The <see cref="T:System.Linq.IQueryProvider" /> that is associated with this data source.
        /// </returns>
        public IQueryProvider Provider => _provider;

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator()
        {
            var enumerator = _provider.Execute(Expression);
            return ((IEnumerable<T>) enumerator).GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}