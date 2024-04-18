using System.Linq.Expressions;
using Chats.Common.Models;

namespace Chats.Database
{
    public interface IBaseRepository
    {
        Task<TModel?> GetOneAsync<TModel>(Expression<Func<TModel, bool>> predicate)
            where TModel : BaseEntity;

        IQueryable<TModel> FindByCondition<TModel>(Expression<Func<TModel, bool>> expression)
            where TModel : BaseEntity;

        Task<TModel> CreateAsync<TModel>(TModel item)
            where TModel : BaseEntity;

        Task CreateRangeAsync<TModel>(IEnumerable<TModel> items)
            where TModel : BaseEntity;

        Task<TModel>? UpdateAsync<TModel>(TModel item)
            where TModel : BaseEntity;

        Task<IEnumerable<TModel>> UpdateRange<TModel>(IEnumerable<TModel> item)
            where TModel : BaseEntity;

        Task<TModel?> DeleteAsync<TModel>(Guid id)
            where TModel : BaseEntity;

        Task<IEnumerable<TModel>> DeleteByCondition<TModel>(Expression<Func<TModel, bool>> predicate)
            where TModel : BaseEntity;
    }
}