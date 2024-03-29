﻿using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace _0_Framework.Application.Extensions;

public static class EnumConventors
{
    public static string GetEnumDisplayName<T>(this T enumValue) where T : IComparable, IFormattable, IConvertible
    {
        if (!typeof(T).IsEnum)
            throw new ArgumentException("Argument must be of type Enum");

        var displayAttribute = enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>();

        string displayName = displayAttribute?.GetName();

        return displayName ?? enumValue.ToString();
    }
}