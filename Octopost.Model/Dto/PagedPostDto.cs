namespace Octopost.Model.Dto
{
    using Microsoft.AspNetCore.Mvc;

    public class PagedPostDto : PagedDto
    {
        [FromQuery(Name = "comments")]
        public int CommentNumber { get; set; }
    }
}
