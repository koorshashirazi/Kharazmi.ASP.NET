using System;
using System.Linq;

namespace Mvc.Utility.Core.Managers.LinqToLdap.Mapping
{
    /// <summary>
    ///     Maps an object to a naming context in the directory
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DirectorySchemaAttribute : Attribute
    {
        /// <summary>
        ///     Maps the object to the naming context
        /// </summary>
        /// <param name="namingContext">The naming context</param>
        public DirectorySchemaAttribute(string namingContext)
        {
            NamingContext = namingContext;
        }

        /// <summary>
        ///     Mapped object classes
        /// </summary>
        public string[] ObjectClasses { get; set; }

        /// <summary>
        ///     Mapped object class
        /// </summary>
        public string ObjectClass
        {
            get => ObjectClasses != null ? ObjectClasses.FirstOrDefault() : null;
            set
            {
                if (!value.IsNullOrEmpty()) ObjectClasses = new[] {value};
            }
        }

        /// <summary>
        ///     Mapped naming context
        /// </summary>
        public string NamingContext { get; }

        /// <summary>
        ///     Mapped object category
        /// </summary>
        public string ObjectCategory { get; set; }

        /// <summary>
        ///     Indicates if the object category should be included in filters.
        /// </summary>
        public bool IncludeObjectCategory { get; set; } = true;

        /// <summary>
        ///     Indicates if the object classes should be included in filters.
        /// </summary>
        public bool IncludeObjectClasses { get; set; } = true;
    }
}