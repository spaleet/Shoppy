﻿using _0_Framework.Application.ErrorMessages;
using System.Globalization;

namespace _0_Framework.Application.Exceptions;

public class NoContentApiException : Exception
{
    public NoContentApiException(string message = ApplicationErrorMessage.NoContent) : base(message)
    {
    }

    public NoContentApiException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}