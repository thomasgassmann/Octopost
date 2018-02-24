namespace Octopost.Services.Repository
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IRepository<T>
    {
        IQueryable<T> Query();

        void Delete(Expression<Func<T, bool>> func);

        void Delete(long id);

        T Create(T entity);

        T FindById(long id);

        T Update(T entity);
    }
}
