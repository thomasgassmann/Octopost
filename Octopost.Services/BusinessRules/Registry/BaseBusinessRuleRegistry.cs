namespace Octopost.Services.BusinessRules.Registry
{
    using Octopost.Services.Assembly;
    using Octopost.Services.BusinessRules.Interfaces;
    using Octopost.Services.BusinessRules.Registry.Interfaces;
    using Octopost.Services.UoW;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class BaseBusinessRuleRegistry : IBusinessRuleRegistry
    {
        private readonly IServiceProvider serviceProvider;

        private readonly IAssemblyContainer assemblyContainer;

        private IDictionary<Type, IList<Type>> registeredEntries = 
            new Dictionary<Type, IList<Type>>();

        public BaseBusinessRuleRegistry(IServiceProvider serviceProvider, IAssemblyContainer assemblyContainer)
        {
            this.serviceProvider = serviceProvider;
            this.assemblyContainer = assemblyContainer;
            this.RegisterEntries();
        }

        public IDictionary<Type, IList<Type>> RegisteredEntries => 
            this.registeredEntries;

        public void RegisterEntries()
        {
            var octopostAssemblies = this.assemblyContainer.GetAssemblies();
            foreach (var octopostAssembly in octopostAssemblies)
            {
                var types = octopostAssembly.GetTypes();
                var businessRules = types.Where(x => x.GetInterfaces().Contains(typeof(IBusinessRuleBase)) && !x.IsInterface);
                foreach (var businessRule in businessRules)
                {
                    if (!businessRule.IsAbstract && !businessRule.IsGenericType)
                    {
                        var type = businessRule.BaseType.GetGenericArguments().SingleOrDefault();
                        if (businessRule.BaseType == typeof(object))
                        {
                            type = typeof(object);
                        }

                        this.RegisterEntry(type, businessRule);
                    }
                }
            }
        }

        public IEnumerable<Type> GetBusinessRulesFor<TEntity>() =>
            this.GetBusinessRulesFor(typeof(TEntity));

        public IEnumerable<Type> GetBusinessRulesFor(Type type)
        {
            var list = new List<Type>();
            foreach (var businessRuleGroup in this.RegisteredEntries)
            {
                if (businessRuleGroup.Key.GetTypeInfo().IsAssignableFrom(type))
                {
                    foreach (var value in businessRuleGroup.Value)
                    {
                        list.Add(value);
                    }
                }
            }
    
            // Execute least specific business rule first
            return list.Distinct().OrderBy(x => x.GetTypeInfo().IsInterface);
        }

        public TBusinessRule InstantiateBusinessRule<TBusinessRule>(IUnitOfWork unitOfWork) where TBusinessRule : IBusinessRuleBase =>
            (TBusinessRule)this.InstantiateBusinessRule(typeof(TBusinessRule), unitOfWork);

        public IBusinessRuleBase InstantiateBusinessRule(Type type, IUnitOfWork unitOfWork)
        {
            this.ThrowIfInvalidBusinessRule(type);
            var instantiated = this.serviceProvider.GetService(type);
            return (IBusinessRuleBase)instantiated;
        }

        public void TriggerPreSaveBusinessRulesFor<TEntity>(IUnitOfWork unitOfWork, IList<TEntity> added, IList<TEntity> changed, IList<TEntity> removed)
        {
            var rules = this.GetBusinessRulesFor<TEntity>();
            foreach (var rule in rules)
            {
                var createdRule = this.InstantiateBusinessRule(rule, unitOfWork);
                createdRule.PreSave(
                    added.Cast<object>().ToList(), 
                    changed.Cast<object>().ToList(), 
                    removed.Cast<object>().ToList());
            }
        }

        public void TriggerPostSaveBusinessRulesFor<TEntity>(IUnitOfWork unitOfWork)
        {
            var rules = this.GetBusinessRulesFor<TEntity>();
            foreach (var rule in rules)
            {
                var createdRule = this.InstantiateBusinessRule(rule, unitOfWork);
                createdRule.PostSave(unitOfWork);
            }
        }

        private void ThrowIfInvalidBusinessRule(Type t)
        {
            if (!t.GetInterfaces().Contains(typeof(IBusinessRuleBase)))
            {
                throw new ArgumentException($"The business rule '{t.FullName}' must implement '{typeof(IBusinessRuleBase).FullName}'.");
            }
        }

        private void RegisterEntry(Type entityType, Type businessRuleType)
        {
            this.ThrowIfInvalidBusinessRule(businessRuleType);
            if (!this.registeredEntries.ContainsKey(entityType))
            {
                this.registeredEntries.Add(entityType, new List<Type>());
                this.registeredEntries[entityType].Add(businessRuleType);
                return;
            }

            this.registeredEntries[entityType].Add(businessRuleType);
        }
    }
}
