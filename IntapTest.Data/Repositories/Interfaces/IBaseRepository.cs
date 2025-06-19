using IntapTest.Shared.Dtos;
using IntapTest.Shared.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace IntapTest.Data.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task Remove(T entity);
        Task<EntityEntry> Delete(T entity);
        Task<T> GetFirstOrDefaultByFilter(Expression<Func<T, bool>>? filter = null, bool tracking = true);
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null, bool tracking = true);
        Task<List<T>> GetByFilter(Expression<Func<T, bool>>? filter = null, bool tracking = true);
        Task<List<T>> GetByFilter(Expression<Func<T, object>> orderBy,
            SortDirectionEnum sortDirection, Expression<Func<T, bool>>? filter = null,
            int? takenItems = null, bool tracking = true);
        Task<PaginatedResultDto<T>> GetByFilterPagination(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int? page = null, int? pageSize = null);
        Task<int> GetCountByFilter(Expression<Func<T, bool>> filter);
        Task<T> GetById(Guid id);
        Task<List<T>> Query(Expression<Func<T, bool>>? filter = null);
    }
}
