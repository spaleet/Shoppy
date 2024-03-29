﻿using Microsoft.AspNetCore.Http;

namespace _0_Framework.Application.Extensions;

public static class MaxFileSizeValidator
{
    public static bool IsValid(int maxFileSize, object value, bool isRequired = true)
    {
        var file = value as IFormFile;

        if (!isRequired && file is null)
            return true;

        if (file is null)
            return false;

        return file.Length <= maxFileSize;
    }
}