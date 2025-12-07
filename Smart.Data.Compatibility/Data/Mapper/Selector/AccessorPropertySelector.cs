namespace Smart.Data.Mapper.Selector;

using System;
using System.Reflection;

using Smart.Data.Accessor.Attributes;
using Smart.Text;

public sealed class AccessorPropertySelector : IPropertySelector
{
    public static AccessorPropertySelector Instance { get; } = new();

    private AccessorPropertySelector()
    {
    }

    public PropertyInfo? SelectProperty(PropertyInfo[] properties, string name)
    {
        PropertyInfo? caseInsensitiveMatch = null;
        PropertyInfo? pascalMatch = null;
        PropertyInfo? pascalCaseInsensitiveMatch = null;
        string? pascalName = null;

        foreach (var pi in properties)
        {
            var propertyName = pi.GetCustomAttribute<NameAttribute>()?.Name ?? pi.Name;
            if (String.Equals(propertyName, name, StringComparison.Ordinal))
            {
                return pi;
            }

            if ((caseInsensitiveMatch is null) && String.Equals(propertyName, name, StringComparison.OrdinalIgnoreCase))
            {
                caseInsensitiveMatch = pi;
            }

            pascalName ??= Inflector.Pascalize(name);
            if (pascalName != name)
            {
                if ((pascalMatch is null) && String.Equals(propertyName, pascalName, StringComparison.Ordinal))
                {
                    pascalMatch = pi;
                }

                if ((pascalCaseInsensitiveMatch is null) && String.Equals(propertyName, pascalName, StringComparison.OrdinalIgnoreCase))
                {
                    pascalCaseInsensitiveMatch = pi;
                }
            }
        }

        return caseInsensitiveMatch ?? pascalMatch ?? pascalCaseInsensitiveMatch;
    }
}
