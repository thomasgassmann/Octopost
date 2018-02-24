namespace Octopost.Services.UoW
{
    using Octopost.Services.Repository;
    using System;

    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> CreateEntityRepository<T>();

        void Save();
    }
}
