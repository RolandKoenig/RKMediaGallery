﻿using System;
using System.Collections.Generic;

namespace RKMediaGallery.ExceptionViewer.Data.Analyzers;

public class AggregateExceptionAnalyzer : IExceptionAnalyzer
{
    /// <inheritdoc />
    public IEnumerable<ExceptionProperty>? ReadExceptionInfo(Exception ex)
    {
        return null;
    }

    /// <inheritdoc />
    public IEnumerable<Exception>? GetInnerExceptions(Exception ex)
    {
        if (ex is AggregateException aggEx)
        {
            foreach (var actInnerException in aggEx.InnerExceptions)
            {
                yield return actInnerException;
            }
        }
    }
}