﻿using FluentValidation.Results;
using System.Collections.Generic;

namespace _0_Framework.Application.Exceptions;

public class ValidationException : Exception
{
    public ValidationException() : base("One or more validation failures have occurred.")
    {
        Errors = new List<string>();
    }

    public List<string> Errors { get; }

    public ValidationException(List<ValidationFailure> failures)
        : this()
    {
        foreach (var failure in failures) 
            Errors.Add(failure.ErrorMessage);
    }
}