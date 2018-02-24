namespace Octopost.Services.UoW
{
    using Microsoft.EntityFrameworkCore;
    using Octopost.Model.Interfaces;
    using System.Linq;

    public class QueryService : IQueryService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private IUnitOfWork unitOfWork;

        public QueryService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        protected IUnitOfWork UnitOfWork =>
            this.unitOfWork ?? (this.unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork());

        public IQueryable<T> Query<T>() where T : class, IIdentifiable
        {
            var repository = this.UnitOfWork.CreateEntityRepository<T>();
            return repository.Query().AsNoTracking();
        }
    }
}
