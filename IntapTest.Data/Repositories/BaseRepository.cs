using IntapTest.Data.Base;
using IntapTest.Data.Repositories.Interfaces;
using IntapTest.Shared.Dtos;
using IntapTest.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace IntapTest.Data.Repositories
{
    public class BaseRepository<T, TDbContext> : IBaseRepository<T> where T : class, IBase where TDbContext : DbContext
    {
        public readonly DbContext _dbContext;
        protected IRequestInfo<TDbContext> _requestInfo { get; private set; }

        public BaseRepository(IRequestInfo<TDbContext> RequestInfo)
        {
            _requestInfo = RequestInfo;
            _dbContext = RequestInfo.Context;
        }

        public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default) =>
            _dbContext.Set<T>().AnyAsync(predicate, cancellationToken);

        public async Task<T> Create(T entity)
        {
            var date = DateTime.UtcNow;
            entity.Id = Guid.NewGuid();
            entity.CreatedBy = _requestInfo.UserId;
            entity.CreatedOn = date;
            entity.LastModifiedBy = _requestInfo.UserId;
            entity.LastModifiedOn = date;
            entity.IsDeleted = false;
            entity.IsActive = true;
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<EntityEntry> Delete(T entity)
        {
            var deleteEntity = _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
            return deleteEntity;
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null, bool tracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (!tracking)
                query = query.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<List<T>> GetByFilter(Expression<Func<T, bool>>? filter = null, bool tracking = true)
        {
            return await GetByFilter(x => x.CreatedOn, SortDirectionEnum.Ascend, filter);
        }

        public async Task<List<T>> GetByFilter(Expression<Func<T, object>> orderBy,
            SortDirectionEnum sortDirection, Expression<Func<T, bool>>? filter = null,
            int? takenItems = null, bool tracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (!tracking)
                query = query.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var result = sortDirection == SortDirectionEnum.Ascend
                ? query.OrderBy(orderBy)
                : query.OrderByDescending(orderBy);

            if (takenItems != null)
                result = (IOrderedQueryable<T>)result.Take(takenItems.Value);

            return await result.ToListAsync();
        }

        public async Task<PaginatedResultDto<T>> GetByFilterPagination(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int? page = null, int? pageSize = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (filter != null)
                query = query.Where(filter);

            var totalCount = await query.CountAsync();

            if (orderBy != null)
                query = orderBy(query);

            if (page != null && pageSize != null)
                query = query.Skip(((int)page - 1) * (int)pageSize).Take((int)pageSize);

            var items = await query.ToListAsync();

            return new PaginatedResultDto<T>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = items
            };
        }

        public async Task<T> GetById(Guid id)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> GetCountByFilter(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            query = query.Where(filter);
            return await query.CountAsync();
        }

        public async Task<T> GetFirstOrDefaultByFilter(Expression<Func<T, bool>>? filter = null, bool tracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (!tracking)
                query = query.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> Query(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();
        }

        public async Task Remove(T entity)
        {
            entity.IsDeleted = true;
            entity.IsActive = false;
            entity.LastModifiedBy = _requestInfo.UserId;
            entity.LastModifiedOn = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<T> Update(T entity)
        {
            entity.LastModifiedBy = _requestInfo.UserId;
            entity.LastModifiedOn = DateTime.UtcNow;
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}
