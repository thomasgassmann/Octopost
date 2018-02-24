namespace Octopost.Services.UoW
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Octopost.DataAccess.Context;
    using Octopost.Model.Data;
    using Octopost.Services.BusinessRules.Interfaces;
    using Octopost.Services.BusinessRules.Registry.Interfaces;
    using Octopost.Services.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly OctopostDbContext octopostDbContext;

        private readonly Lazy<ChangeTracker> changeTracker;

        private readonly IBusinessRuleRegistry businessRuleRegistry;

        private readonly string connectionString;

        private readonly IDictionary<Type, Type> repositories = new Dictionary<Type, Type>();

        public UnitOfWork(string connectionString, IBusinessRuleRegistry businessRuleRegistry)
        {
            this.RegisterRepositories();
            this.connectionString = connectionString;
            this.octopostDbContext = this.CreateContext();
            this.businessRuleRegistry = businessRuleRegistry;
            this.changeTracker = new Lazy<ChangeTracker>(() => new ChangeTracker(this.octopostDbContext));
        }

        public IRepository<T> CreateEntityRepository<T>()
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                throw new ArgumentException($"No repository for type {typeof(T).FullName} registered");
            }

            var type = this.repositories[typeof(T)];
            return (IRepository<T>)Activator.CreateInstance(type, this.octopostDbContext);
        }

        public void Dispose() =>
            this.octopostDbContext.Dispose();

        public void Save()
        {
            var changeTrackerValue = this.changeTracker.Value;
            var businessRules = this.ExecutePreSaveBusinessRules(changeTrackerValue);
            this.octopostDbContext.SaveChanges();
            this.ExecutePostSaveBusinessRules(businessRules);
        }

        private OctopostDbContext CreateContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<OctopostDbContext>();
            optionsBuilder.UseMySql(this.connectionString);
            return new OctopostDbContext(optionsBuilder.Options);
        }

        private IEnumerable<IBusinessRuleBase> ExecutePreSaveBusinessRules(ChangeTracker changeTracker)
        {
            var list = new List<IBusinessRuleBase>();
            foreach (var changedEntityGroup in changeTracker.Entries().GroupBy(x => x.Entity.GetType()))
            {
                var addedIds = changedEntityGroup.Where(x => x.State == EntityState.Added).Select(x => x.Entity).ToList();
                var changedIds = changedEntityGroup.Where(x => x.State == EntityState.Modified).Select(x => x.Entity).ToList();
                var removedIds = changedEntityGroup.Where(x => x.State == EntityState.Deleted).Select(x => x.Entity).ToList();
                var businessRulesToExecute = this.businessRuleRegistry.GetBusinessRulesFor(changedEntityGroup.Key).ToList();
                foreach (var businessRule in businessRulesToExecute)
                {
                    var instantiatedBusinessRule = this.businessRuleRegistry.InstantiateBusinessRule(businessRule, this);
                    instantiatedBusinessRule.PreSave(
                        addedIds,
                        changedIds,
                        removedIds);
                    list.Add(instantiatedBusinessRule);
                }
            }

            return list;
        }

        private void ExecutePostSaveBusinessRules(IEnumerable<IBusinessRuleBase> businessRulesToExecute)
        {
            foreach (var businessRule in businessRulesToExecute)
            {
                businessRule.PostSave(this);
            }
        }

        private void RegisterRepositories()
        {
            this.repositories.Add(typeof(Post), typeof(Repository<Post>));
            this.repositories.Add(typeof(Vote), typeof(Repository<Vote>));
            this.repositories.Add(typeof(LocationName), typeof(Repository<LocationName>));
            this.repositories.Add(typeof(Comment), typeof(Repository<Comment>));
            this.repositories.Add(typeof(File), typeof(Repository<File>));
        }
    }
}
