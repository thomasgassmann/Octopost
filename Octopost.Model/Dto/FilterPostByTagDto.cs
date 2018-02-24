namespace Octopost.Model.Dto
{
    using Microsoft.AspNetCore.Mvc;

    public class FilterPostByTagDto : PagedPostDto
    {
        [FromQuery(Name = "tags")]
        public string Tags { get; set; }
    }
}
