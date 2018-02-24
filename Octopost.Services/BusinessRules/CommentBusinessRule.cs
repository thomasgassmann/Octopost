namespace Octopost.Services.BusinessRules
{
    using Octopost.Model.ApiResponse.HTTP400;
    using Octopost.Model.Data;
    using Octopost.Model.Validation;
    using Octopost.Services.Exceptions;
    using Octopost.Services.UoW;
    using System.Collections.Generic;
    using System.Linq;

    public class CommentBusinessRule : BusinessRuleBase<Comment>
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public CommentBusinessRule(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public override void PreSave(IList<Comment> added, IList<Comment> updated, IList<Comment> removed)
        {
            var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork();
            var postRepository = unitOfWork.CreateEntityRepository<Post>();
            foreach (var item in added)
            {
                if (!postRepository.Query().Any(x => x.Id == item.PostId))
                {
                    throw new ApiException(x => x.BadRequestResult(
                        (ErrorCode.Parse(ErrorCodeType.InvalidReferenceId, OctopostEntityName.Comment, PropertyName.Comment.PostId, OctopostEntityName.Post),
                        new ErrorDefinition(item.PostId, "The given post does not exist", PropertyName.Comment.PostId))));
                }
            }
        }
    }
}
