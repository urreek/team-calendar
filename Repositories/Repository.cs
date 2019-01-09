using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace team_calendar.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext context;

        public Repository(DbContext context){
            this.context = context;
        }

        public void Add(T entity) => context.Set<T>().Add(entity);

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate) => context.Set<T>().Where(predicate);

        public T Get(int id) => context.Set<T>().Find(id);

        public async Task<IEnumerable<T>> GetAll() => await context.Set<T>().ToListAsync();

        public void Remove(T entity) => context.Set<T>().Remove(entity);
    }
}