using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace MarketBasketAnalysis.Server.Application.Extensions;

public static class ExceptionExtensions
{
    public static bool IsDbOrDbUpdateException(this Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return exception is DbException or DbUpdateException;
    }
}