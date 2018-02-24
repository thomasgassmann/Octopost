namespace Octopost.Services.BusinessRules.Interfaces
{
    using Octopost.Services.UoW;
    using System.Collections.Generic;

    public interface IBusinessRuleBase
    {
        void PreSave(IList<object> added, IList<object> updated, IList<object> removed);

        void PostSave(IUnitOfWork unitOfWork);
    }
}