namespace Correios.DneBasico.Api.Extensions;

public static class IQueryableExtensions
{
    public static async Task<PagedResponse<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        int totalCount,
        int pageNumber = 1,
        int pageSize = 100,
        CancellationToken cancellationToken = default)
    {
        var entities = await query.ToListAsync(cancellationToken);

        var pagedResult = new PagedResponse<T>
        {
            Data = entities ?? [],
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
            PageNumber = pageNumber,
            PageSize = pageSize,
            RowsReturned = entities?.Count ?? 0
        };

        return pagedResult;
    }
}