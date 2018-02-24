namespace Octopost.Services.UoW
{
    using Octopost.Services.BusinessRules.Registry.Interfaces;

    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IBusinessRuleRegistry businessRuleRegistry;

        private readonly string connectionString;

        public UnitOfWorkFactory(string connectionString, IBusinessRuleRegistry businessRuleRegistry)
        {
            this.connectionString = connectionString;
            this.businessRuleRegistry = businessRuleRegistry;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            var unitOfWork = new UnitOfWork(this.connectionString, this.businessRuleRegistry);
            return unitOfWork;
        }
    }
}
