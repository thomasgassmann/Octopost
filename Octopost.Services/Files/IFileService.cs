namespace Octopost.Services.Files
{
    using Microsoft.AspNetCore.Http;
    using Octopost.Model.Dto;

    public interface IFileService
    {
        FileDto GetFile(long id);

        FileInfoDto GetFileInfoForPost(long postId);

        FileInfoDto GetFileInfo(long id);

        long CreateLinkedFile(CreateLinkedFileDto dto);

        long CreateFile(IFormFile file);
    }
}
