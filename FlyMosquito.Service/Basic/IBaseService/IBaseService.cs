#region using
using FlyMosquito.Domain;
using System.Linq.Expressions;
#endregion

namespace FlyMosquito.Service.Basic.IBaseService
{
    public interface IBaseService<TEntity> where TEntity : class, new()
    {
        bool Delete(object id);
        int Delete(Expression<Func<TEntity, bool>> predicate);
        Task<bool> DeleteAsync(object id);
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> DeleteAsync(TEntity entity);
        Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities);
        void ExecuteInTransaction(Action action);
        Task ExecuteInTransactionAsync(Func<Task> func);
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> GetList();
        List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetListAsync();
        Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> GetListPageAsync<TKey>(PageWithSortDto pageWithSortDto, Expression<Func<TEntity, TKey>> keySelector);
        int Insert(TEntity entity);
        int Insert(List<TEntity> entities);
        Task<int> InsertAsync(TEntity entity);
        Task<int> InsertAsync(List<TEntity> entities);
        int Update(TEntity entity);
        int Update(List<TEntity> entities);
        Task<int> UpdateAsync(TEntity entity);
        Task<int> UpdateAsync(List<TEntity> entities);
    }
}
