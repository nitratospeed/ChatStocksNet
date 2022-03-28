using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<IEnumerable<T>> GetAllByFilter(Expression<Func<T, bool>> condition);
        Task<T> GetByFilter(Expression<Func<T, bool>> condition);
        Task<bool> AnyFilter(Expression<Func<T, bool>> condition);
        Task<T> Insert(T entity);
    }
}
