﻿using System.Text.RegularExpressions;

namespace _0_Framework.Application.Extensions;

public static class GenerateSlug
{
    public static string ToSlug(this string value)
    {
        //First to lower case 
        value = value.ToLowerInvariant();

        //Replace spaces 
        value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

        //Trim dashes from end 
        value = value.Trim('-', '_');

        //Replace double occurences of - or \_ 
        value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

        return value;
    }
}