namespace Octopost.Services.Repository
{
    using Octopost.DataAccess.Context;
    using Octopost.Model.Interfaces;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class Repository<T> : IRepository<T> where T : class, IIdentifiable
    {
        private readonly OctopostDbContext octopostDbContext;

        public Repository(OctopostDbContext octopostDbContext) =>
            this.octopostDbContext = octopostDbContext;

        public T Create(T entity)
        {
            var entry = this.octopostDbContext.Add(entity);
            return entry.Entity;
        }

        public void Delete(Expression<Func<T, bool>> func)
        {
            var results = this.Query().Where(func).ToArray();
            foreach (var result in results)
            {
                this.octopostDbContext.Remove(result);
            }
        }

        public void Delete(long id)
        {
            var entity = this.FindById(id);
            if (entity != null)
            {
                this.octopostDbContext.Set<T>().Remove(entity);
            }
        }

        public T FindById(long id) =>
            this.Query().FirstOrDefault(x => x.Id == id);

        public IQueryable<T> Query() =>
            this.octopostDbContext.Set<T>();

        public T Update(T entity)
        {
            this.octopostDbContext.Set<T>().Update(entity);
            return entity;
        }
    }
}
