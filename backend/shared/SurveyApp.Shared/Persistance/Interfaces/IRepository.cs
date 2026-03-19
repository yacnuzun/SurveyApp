using SurveyApp.Shared.Persistance.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SurveyApp.Shared.Persistance.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>>? predicate = null);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
