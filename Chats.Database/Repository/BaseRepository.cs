using System.Linq.Expressions;
using Chats.Common.Models;
using Chats.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Chats.Database.Repository
{
    public class BaseRepository : IBaseRepository
    {
        protected readonly CommonContext CommonContext;

        public BaseRepository(CommonContext commonContext)
            => CommonContext = commonContext;

        public async Task<TModel?> GetOneAsync<TModel>(Expression<Func<TModel, bool>> predicate)
            where TModel : BaseEntity
            => await CommonContext.Set<TModel>().AsNoTracking().FirstOrDefaultAsync(predicate);

        public IQueryable<TModel> FindByCondition<TModel>(Expression<Func<TModel, bool>> expression)
            where TModel : BaseEntity
            => CommonContext.Set<TModel>()
                .Where(expression)
                .AsNoTracking();

        public async Task<TModel> CreateAsync<TModel>(TModel item)
            where TModel : BaseEntity
        {
            await CommonContext.Set<TModel>().AddAsync(item);
            await CommonContext.SaveChangesAsync();
            return item;
        }

        public async Task CreateRangeAsync<TModel>(IEnumerable<TModel> items)
            where TModel : BaseEntity
        {
            await CommonContext.Set<TModel>().AddRangeAsync(items);
            await CommonContext.SaveChangesAsync();
        }

        public async Task<TModel>? UpdateAsync<TModel>(TModel item)
            where TModel : BaseEntity
        {
            CommonContext.Set<TModel>().Update(item);
            await CommonContext.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<TModel>> UpdateRange<TModel>(IEnumerable<TModel> item)
            where TModel : BaseEntity
        {
            CommonContext.Set<TModel>().UpdateRange(item);
            await CommonContext.SaveChangesAsync();
            return item;
        }

        public async Task<TModel?> DeleteAsync<TModel>(Guid id)
            where TModel : BaseEntity
        {
            var result = await CommonContext.Set<TModel>().FirstOrDefaultAsync(p => p.Id == id);
            if (result is null) return null;
            CommonContext.Set<TModel>().Remove(result);
            await CommonContext.SaveChangesAsync();
            return result;
        }

        public async Task<IEnumerable<TModel>> DeleteByCondition<TModel>(Expression<Func<TModel, bool>> expression)
            where TModel : BaseEntity
        {
            var result = await CommonContext.Set<TModel>()
                .Where(expression)
                .ToListAsync();

            CommonContext.Set<TModel>().RemoveRange(result);
            await CommonContext.SaveChangesAsync();

            return result;
        }
    }
}