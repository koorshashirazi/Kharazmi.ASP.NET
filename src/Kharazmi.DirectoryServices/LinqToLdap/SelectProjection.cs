using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kharazmi.DirectoryServices.Ldap.LinqToLdap
{
    internal class SelectProjection
    {
        public SelectProjection(IDictionary<string, string> selectedProperties, LambdaExpression projection)
        {
            SelectedProperties = selectedProperties;
            Projection = projection.Compile();
#if NET35
            ReturnType = projection.Body.Type;
#else
            ReturnType = projection.ReturnType;
#endif
        }

        public IDictionary<string, string> SelectedProperties { get; }
        public Delegate Projection { get; }
        public Type ReturnType { get; }
    }
}