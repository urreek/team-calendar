using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace team_calendar.Repositories {
    public interface IRepository<T> where T : class
    {
        T Get(int id);
        Task<IEnumerable<T>> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> Predicate);

        void Add(T entity);
        void Remove(T entity);
    }
}