using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace MamaFit.Repositories.Infrastructure
{
    public static class IQueryableExtension
    {
        public static Task<PaginatedList<T>> GetPaginatedList<T>(this IQueryable<T> source, int pageIndex, int pageSize) where T : class
            => PaginatedList<T>.CreateAsync(source.AsNoTracking(), pageIndex, pageSize);

        public static Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable, IConfigurationProvider configuration) where TDestination : class
            => queryable.ProjectTo<TDestination>(configuration).AsNoTracking().ToListAsync();
    }
}
