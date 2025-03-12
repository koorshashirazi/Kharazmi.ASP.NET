using System.DirectoryServices.Protocols;

namespace Mvc.Utility.Core.Managers.LinqToLdap.Mapping.PropertyMappings
{
    internal class BooleanPropertyMapping<T> : PropertyMappingGeneric<T> where T : class
    {
        private const string True = "TRUE";
        private const string False = "FALSE";

        public BooleanPropertyMapping(PropertyMappingArguments<T> arguments) : base(arguments)
        {
        }

        public override string FormatValueToFilter(object value)
        {
            return true.Equals(value) ? True : False;
        }

        public override DirectoryAttributeModification GetDirectoryAttributeModification(object instance)
        {
            var modification = new DirectoryAttributeModification
            {
                Name = AttributeName,
                Operation = DirectoryAttributeOperation.Replace
            };
            var value = (string) GetValueForDirectory(instance);

            if (value != null) modification.Add(value);

            return modification;
        }

        public override object GetValueForDirectory(object instance)
        {
            var value = GetValue(instance);

            if (value == null) return null;

            return true.Equals(value) ? True : False;
        }

        public override object FormatValueFromDirectory(DirectoryAttribute value, string dn)
        {
            string str;
            if (value != null && (str = value[0] as string) != null && !str.IsNullOrEmpty())
            {
                if (True.Equals(str)) return true;
                if (False.Equals(str)) return false;

                ThrowMappingException(str, dn);
            }

            AssertNullable(dn);

            return null;
        }
    }
}