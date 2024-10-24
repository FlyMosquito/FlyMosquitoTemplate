#region using
using FlyMosquito.Core;
using FlyMosquito.Domain;
using FlyMosquito.Service.Basic.IBaseService;
using System.Linq.Expressions;
#endregion

namespace FlyMosquito.Service.Basic.BaseService
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class, new()
    {
        //从子类构造函数中传入
        protected IRepository<TEntity> Repository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public BaseService(IRepository<TEntity> repository)
        {
            Repository = repository;
        }

        #region 查询操作
        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>实体列表</returns>
        public List<TEntity> GetList()
        {
            return Repository.GetList();
        }

        /// <summary>
        /// 异步获取所有实体
        /// </summary>
        /// <returns>实体列表</returns>
        public async Task<List<TEntity>> GetListAsync()
        {
            return await Repository.GetListAsync();
        }

        /// <summary>
        /// 根据条件获取单个实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>符合条件的实体</returns>
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.Get(predicate);
        }

        /// <summary>
        /// 异步根据条件获取单个实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>符合条件的实体</returns>
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Repository.GetAsync(predicate);
        }

        /// <summary>
        /// 根据条件获取实体列表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>符合条件的实体列表</returns>
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.GetList(predicate);
        }

        /// <summary>
        /// 异步根据条件获取实体列表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>符合条件的实体列表</returns>
        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Repository.GetListAsync(predicate);
        }

        /// <summary>
        /// 分页获取实体列表，并按指定字段排序
        /// </summary>
        /// <typeparam name="TKey">排序字段类型</typeparam>
        /// <param name="pageWithSortDto">分页和排序信息</param>
        /// <param name="keySelector">排序依据的字段表达式</param>
        /// <returns>分页后的实体列表</returns>
        public async Task<List<TEntity>> GetListPageAsync<TKey>(PageWithSortDto pageWithSortDto, Expression<Func<TEntity, TKey>> keySelector)
        {
            return await Repository.GetListPageAsync(pageWithSortDto, keySelector);
        }
        #endregion

        #region 插入操作
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity">要添加的实体</param>
        /// <returns>受影响的行数</returns>
        public int Insert(TEntity entity)
        {
            return Repository.Insert(entity);
        }

        /// <summary>
        /// 异步添加实体
        /// </summary>
        /// <param name="entity">要添加的实体</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> InsertAsync(TEntity entity)
        {
            return await Repository.InsertAsync(entity);
        }

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>受影响的行数</returns>
        public int Insert(List<TEntity> entities)
        {
            return Repository.Insert(entities);
        }

        /// <summary>
        /// 异步批量添加实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> InsertAsync(List<TEntity> entities)
        {
            return await Repository.InsertAsync(entities);
        }
        #endregion

        #region 删除操作
        /// <summary>
        /// 根据ID删除实体
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>是否成功删除</returns>
        public bool Delete(object id)
        {
            return Repository.Delete(id);
        }

        /// <summary>
        /// 异步根据ID删除实体
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>是否成功删除</returns>
        public async Task<bool> DeleteAsync(object id)
        {
            return await Repository.DeleteAsync(id);
        }

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="predicate">删除条件</param>
        /// <returns>删除的记录数</returns>
        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            return Repository.Delete(predicate);
        }

        /// <summary>
        /// 异步根据条件删除实体
        /// </summary>
        /// <param name="predicate">删除条件</param>
        /// <returns>删除的记录数</returns>
        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Repository.DeleteAsync(predicate);
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            return await Repository.DeleteAsync(entity);
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            return await Repository.DeleteRangeAsync(entities);
        }
        #endregion

        #region 更新操作
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">要更新的实体</param>
        /// <returns>受影响的行数</returns>
        public int Update(TEntity entity)
        {
            return Repository.Update(entity);
        }

        /// <summary>
        /// 异步更新实体
        /// </summary>
        /// <param name="entity">要更新的实体</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> UpdateAsync(TEntity entity)
        {
            return await Repository.UpdateAsync(entity);
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>受影响的行数</returns>
        public int Update(List<TEntity> entities)
        {
            return Repository.Update(entities);
        }

        /// <summary>
        /// 异步批量更新实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> UpdateAsync(List<TEntity> entities)
        {
            return await Repository.UpdateAsync(entities);
        }
        #endregion

        #region 事务处理
        /// <summary>
        /// 在事务中执行同步操作
        /// </summary>
        /// <param name="action">操作内容</param>
        public void ExecuteInTransaction(Action action)
        {
            Repository.ExecuteInTransaction(action);
        }

        /// <summary>
        /// 在事务中执行异步操作
        /// </summary>
        /// <param name="func">异步操作内容</param>
        public async Task ExecuteInTransactionAsync(Func<Task> func)
        {
            await Repository.ExecuteInTransactionAsync(func);
        }
        #endregion
    }
}
