#region using
using Microsoft.EntityFrameworkCore;
using FlyMosquito.Common;
using FlyMosquito.Domain;
using System.Linq.Expressions;
#endregion

namespace FlyMosquito.Core
{
    /// <summary>
    /// 通用仓储模式实现，提供实体的CRUD操作
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly FlyMosquitoDbContext FlyMosquitoDbContext;

        /// <summary>
        /// 初始化仓储并注入数据库上下文
        /// </summary>
        /// <param name="context">数据库上下文</param>
        public Repository(FlyMosquitoDbContext context)
        {
            FlyMosquitoDbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region 查询操作
        /// <summary>
        /// 获取所有实体
        /// </summary>
        /// <returns>实体列表</returns>
        public List<TEntity> GetList()
        {
            return FlyMosquitoDbContext.Set<TEntity>().ToList();
        }

        /// <summary>
        /// 异步获取所有实体
        /// </summary>
        /// <returns>实体列表</returns>
        public async Task<List<TEntity>> GetListAsync()
        {
            return await FlyMosquitoDbContext.Set<TEntity>().ToListAsync();
        }

        /// <summary>
        /// 根据条件获取单个实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>符合条件的实体</returns>
        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return FlyMosquitoDbContext.Set<TEntity>().FirstOrDefault(predicate);
        }

        /// <summary>
        /// 异步根据条件获取单个实体
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>符合条件的实体</returns>
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await FlyMosquitoDbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// 根据条件获取实体列表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>符合条件的实体列表</returns>
        public List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            return FlyMosquitoDbContext.Set<TEntity>().Where(predicate).ToList();
        }

        /// <summary>
        /// 异步根据条件获取实体列表
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns>符合条件的实体列表</returns>
        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await FlyMosquitoDbContext.Set<TEntity>().Where(predicate).ToListAsync();
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
            return await FlyMosquitoDbContext.Set<TEntity>()
                .OrderBy(keySelector)
                .Skip((pageWithSortDto.PageIndex - 1) * pageWithSortDto.PageSize)
                .Take(pageWithSortDto.PageSize)
                .ToListAsync();
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
            FlyMosquitoDbContext.Set<TEntity>().Add(entity);
            return FlyMosquitoDbContext.SaveChanges();
        }

        /// <summary>
        /// 异步添加实体
        /// </summary>
        /// <param name="entity">要添加的实体</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> InsertAsync(TEntity entity)
        {
            await FlyMosquitoDbContext.Set<TEntity>().AddAsync(entity);
            return await FlyMosquitoDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 批量添加实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>受影响的行数</returns>
        public int Insert(List<TEntity> entities)
        {
            FlyMosquitoDbContext.Set<TEntity>().AddRange(entities);
            return FlyMosquitoDbContext.SaveChanges();
        }

        /// <summary>
        /// 异步批量添加实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> InsertAsync(List<TEntity> entities)
        {
            await FlyMosquitoDbContext.Set<TEntity>().AddRangeAsync(entities);
            return await FlyMosquitoDbContext.SaveChangesAsync();
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
            var entity = FlyMosquitoDbContext.Set<TEntity>().Find(id);
            if (entity == null) return false;

            FlyMosquitoDbContext.Set<TEntity>().Remove(entity);
            FlyMosquitoDbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// 异步根据ID删除实体
        /// </summary>
        /// <param name="id">实体ID</param>
        /// <returns>是否成功删除</returns>
        public async Task<bool> DeleteAsync(object id)
        {
            var entity = await FlyMosquitoDbContext.Set<TEntity>().FindAsync(id);
            if (entity == null) return false;

            FlyMosquitoDbContext.Set<TEntity>().Remove(entity);
            await FlyMosquitoDbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 根据条件删除实体
        /// </summary>
        /// <param name="predicate">删除条件</param>
        /// <returns>删除的记录数</returns>
        public int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = FlyMosquitoDbContext.Set<TEntity>().Where(predicate).ToList();
            if (!entities.Any()) return 0;

            FlyMosquitoDbContext.Set<TEntity>().RemoveRange(entities);
            return FlyMosquitoDbContext.SaveChanges();
        }

        /// <summary>
        /// 异步根据条件删除实体
        /// </summary>
        /// <param name="predicate">删除条件</param>
        /// <returns>删除的记录数</returns>
        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entities = await FlyMosquitoDbContext.Set<TEntity>().Where(predicate).ToListAsync();
            if (!entities.Any()) return 0;

            FlyMosquitoDbContext.Set<TEntity>().RemoveRange(entities);
            return await FlyMosquitoDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            FlyMosquitoDbContext.Remove(entity);
            return await FlyMosquitoDbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            FlyMosquitoDbContext.RemoveRange(entities);
            return await FlyMosquitoDbContext.SaveChangesAsync();
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
            FlyMosquitoDbContext.Set<TEntity>().Update(entity);
            return FlyMosquitoDbContext.SaveChanges();
        }

        /// <summary>
        /// 异步更新实体
        /// </summary>
        /// <param name="entity">要更新的实体</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> UpdateAsync(TEntity entity)
        {
            FlyMosquitoDbContext.Set<TEntity>().Update(entity);
            return await FlyMosquitoDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>受影响的行数</returns>
        public int Update(List<TEntity> entities)
        {
            FlyMosquitoDbContext.Set<TEntity>().UpdateRange(entities);
            return FlyMosquitoDbContext.SaveChanges();
        }

        /// <summary>
        /// 异步批量更新实体
        /// </summary>
        /// <param name="entities">实体列表</param>
        /// <returns>受影响的行数</returns>
        public async Task<int> UpdateAsync(List<TEntity> entities)
        {
            FlyMosquitoDbContext.Set<TEntity>().UpdateRange(entities);
            return await FlyMosquitoDbContext.SaveChangesAsync();
        }
        #endregion

        #region 事务处理
        /// <summary>
        /// 在事务中执行同步操作
        /// </summary>
        /// <param name="action">操作内容</param>
        public void ExecuteInTransaction(Action action)
        {
            using var transaction = FlyMosquitoDbContext.Database.BeginTransaction();
            try
            {
                action();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                LoggerHelper.Error($"事务执行失败: {ex.Message}");
                throw new InvalidOperationException("事务执行失败", ex);
            }
        }

        /// <summary>
        /// 在事务中执行异步操作
        /// </summary>
        /// <param name="func">异步操作内容</param>
        public async Task ExecuteInTransactionAsync(Func<Task> func)
        {
            using var transaction = await FlyMosquitoDbContext.Database.BeginTransactionAsync();
            try
            {
                await func();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                LoggerHelper.Error($"事务执行失败: {ex.Message}");
                throw new InvalidOperationException("事务执行失败", ex);
            }
        }
        #endregion
    }
}
