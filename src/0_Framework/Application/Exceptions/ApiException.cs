﻿using System.Globalization;

namespace _0_Framework.Application.Exceptions;

public class ApiException : Exception
{
    public ApiException(string message) : base(message)
    {
    }

    public ApiException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}