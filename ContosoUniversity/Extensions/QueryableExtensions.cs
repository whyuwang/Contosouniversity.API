using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> PageAsync<T>(this IQueryable<T> source, int pageIndex, int pageSize)
    {
        var items = await source
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        int count = await source.CountAsync();
        return new PagedResult<T>(items, pageIndex, pageSize, count);
    }
}
