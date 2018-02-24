namespace Octopost.Services.Posts
{
    using Octopost.Model.Dto;

    public interface IPostCreationService
    {
        long CreatePost(CreatePostDto createPostDto);
    }
}
