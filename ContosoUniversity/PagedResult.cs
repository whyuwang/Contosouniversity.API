using System;

namespace ContosoUniversity;

public class PagedResult<T>
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }
    public List<T> Items { get; set; }

    public PagedResult(List<T> items, int pageIndex, int pageSize, int count)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Items = items;
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;
}
