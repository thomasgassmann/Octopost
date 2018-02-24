namespace Octopost.Services.BusinessRules.Registry.Interfaces
{
    using Octopost.Services.BusinessRules.Interfaces;
    using Octopost.Services.UoW;
    using System;
    using System.Collections.Generic;

    public interface IBusinessRuleRegistry
    {
        IDictionary<Type, IList<Type>> RegisteredEntries { get; }

        IEnumerable<Type> GetBusinessRulesFor<TEntity>();

        IEnumerable<Type> GetBusinessRulesFor(Type type);

        void TriggerPreSaveBusinessRulesFor<TEntity>(IUnitOfWork unitOfWork, IList<TEntity> added, IList<TEntity> changed, IList<TEntity> removed);

        void TriggerPostSaveBusinessRulesFor<TEntity>(IUnitOfWork unitOfWork);

        TBusinessRule InstantiateBusinessRule<TBusinessRule>(IUnitOfWork unitOfWork) where TBusinessRule : IBusinessRuleBase;

        IBusinessRuleBase InstantiateBusinessRule(Type type, IUnitOfWork unitOfWork);

        void RegisterEntries();
    }
}
