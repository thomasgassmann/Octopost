namespace Octopost.Services.UoW
{
    using Octopost.Model.Interfaces;
    using System.Linq;

    public interface IQueryService
    {
        IQueryable<T> Query<T>() where T : class, IIdentifiable;
    }
}
