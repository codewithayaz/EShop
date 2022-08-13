using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using EShop.Core.Models;
using EShop.Data.Interfaces;

namespace EShop.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        internal ApplicationDbContext context;
        internal DbSet<TEntity> dbSet;
        internal IMapper _mapper;

        public GenericRepository(ApplicationDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            this.context = context;
            this.dbSet = context.Set<TEntity>(); 
        }

        public PaginatedResult<VM> GetPaginatedResults<VM>(PaginationFilter filter,
            string includeProperties = "")
        {
            filter.Search = filter.Search == null ? "" : filter.Search.ToUpper().Trim();

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            validFilter.Search = filter.Search;

            IQueryable<TEntity> query = dbSet.AsNoTracking();
            
            int totalRecords = 0;
            if (!string.IsNullOrWhiteSpace(filter.Search) && filter.SearchColumnList != null && filter.SearchColumnList.Count>0)
            {
                List<string> conditionList = new List<string>();
                foreach (var item in filter.SearchColumnList)
                {
                    if (item.Value == "string")
                    {
                        conditionList.Add($"{item.Key}.Contains(\"{filter.Search}\")");
                    }
                    else
                    {
                        conditionList.Add($"{item.Key}.ToString().Contains(\"{filter.Search}\")");
                    }
                }
                var condition = string.Join(" or ", conditionList);
                query = query.Where(condition);


                //consider search result only
                totalRecords = query.Count();
            }
            else
            {
                //consider all data
                totalRecords = query.Count();
            }

            if (filter.SortDirection > 0 && !string.IsNullOrEmpty(filter.SortLabel))
            {
                var sortDirection = filter.SortDirection == 1 ? "asc" : "desc";
                query = query.OrderBy($"{filter.SortLabel} {sortDirection}");
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var pagedData = query.Skip((validFilter.PageNumber) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();

            var data = _mapper.Map<List<VM>>(pagedData);

            var response = new PaginatedResult<VM>(data, validFilter.PageNumber, validFilter.PageSize, totalRecords);
            response.Search = validFilter.Search;
            response.SortDirection = validFilter.SortDirection;
            response.SortLabel = validFilter.SortLabel;
            return response;
        }

        public PaginatedResult<TEntity> GetPaginatedResults(PaginationFilter filter,
            string includeProperties = "")
        {
            filter.Search = filter.Search == null ? "" : filter.Search.ToUpper().Trim();

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            validFilter.Search = filter.Search;
            IQueryable<TEntity> query = dbSet.AsNoTracking();

            int totalRecords = 0;
            if (!string.IsNullOrWhiteSpace(filter.Search) && filter.SearchColumnList != null && filter.SearchColumnList.Count > 0)
            {
                List<string> conditionList = new List<string>();
                foreach (var item in filter.SearchColumnList)
                {
                    if (item.Value == "string")
                    {
                        conditionList.Add($"{item.Key}.Contains(\"{filter.Search}\")");
                    }
                    else
                    {
                        conditionList.Add($"{item.Key}.ToString().Contains(\"{filter.Search}\")");
                    }
                }
                var condition = string.Join(" or ", conditionList);
                query = query.Where(condition);


                //consider search result only
                totalRecords = query.Count();
            }
            else
            {
                //consider all data
                totalRecords = query.Count();
            }

            if (filter.SortDirection > 0 && !string.IsNullOrEmpty(filter.SortLabel))
            {
                var sortDirection = filter.SortDirection == 1 ? "asc" : "desc";
                query = query.OrderBy($"{filter.SortLabel} {sortDirection}");
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            var pagedData = query.Skip((validFilter.PageNumber) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();

            var response = new PaginatedResult<TEntity>(pagedData, validFilter.PageNumber, validFilter.PageSize, totalRecords);
            response.Search = validFilter.Search;
            response.SortDirection = validFilter.SortDirection;
            response.SortLabel = validFilter.SortLabel;
            return response;
        }

        public virtual IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return query;
        }

        public virtual TEntity GetByID(int id)
        {
            return dbSet.Find(id);
        }

        public virtual IEnumerable<TEntity> FromSqlRaw(string query, params object[] parameters)
        {
            return dbSet.FromSqlRaw(query, parameters).ToList();
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(int id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> filter)
        {
            return dbSet.Any(filter);
        }
    }
}
