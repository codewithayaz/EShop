using System.Linq.Expressions;
using EShop.Core.Models;

namespace EShop.Data.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class, IEntity
    {
        PaginatedResult<VM> GetPaginatedResults<VM>(PaginationFilter filter, string includeProperties = "");
        PaginatedResult<TEntity> GetPaginatedResults(PaginationFilter filter, string includeProperties = "");

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");
        TEntity GetByID(int id);
        IEnumerable<TEntity> FromSqlRaw(string query, params object[] parameters);
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
        void Delete(int id);
        void Delete(TEntity entityToDelete);
        bool Any(Expression<Func<TEntity, bool>> filter);
        
    }
}