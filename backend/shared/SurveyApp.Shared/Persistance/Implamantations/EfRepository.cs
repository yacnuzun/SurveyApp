using Microsoft.EntityFrameworkCore;
using SurveyApp.Shared.Persistance.Entities;
using SurveyApp.Shared.Persistance.Interfaces;
using System.Linq.Expressions;
using System.Security.Principal;

namespace SurveyApp.Shared.Persistance.Implamantations
{
    public class EfRepository<T, TContext> : IRepository<T>
    where T : class, IEntity
    where TContext : DbContext
    {
        protected readonly TContext Context;
        public EfRepository(TContext context) => Context = context;

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate) =>
            await Context.Set<T>().FirstOrDefaultAsync(predicate);

        public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, bool>>? predicate = null) =>
            predicate == null
                ? await Context.Set<T>().ToListAsync()
                : await Context.Set<T>().Where(predicate).ToListAsync();

        public async Task AddAsync(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            Context.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            Context.Set<T>().Remove(entity);
        }
    }
}
