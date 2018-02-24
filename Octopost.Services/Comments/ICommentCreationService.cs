namespace Octopost.Services.Comments
{
    using Octopost.Model.Dto;

    public interface ICommentCreationService
    {
        long CreateComment(long postId, CreateCommentDto createPostDto);
    }
}
