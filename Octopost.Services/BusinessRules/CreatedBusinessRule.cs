namespace Octopost.Services.BusinessRule
{
    using Octopost.Model.Interfaces;
    using Octopost.Services.BusinessRules;
    using System;
    using System.Collections.Generic;

    public class CreatedBusinessRule : BusinessRuleBase<ICreated>
    {
        public override void PreSave(IList<ICreated> added, IList<ICreated> updated, IList<ICreated> removed)
        {
            foreach (var addedItem in added)
            {
                addedItem.Created = DateTime.Now;
            }
        }
    }
}
