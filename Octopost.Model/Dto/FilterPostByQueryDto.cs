namespace Octopost.Model.Dto
{
    using Microsoft.AspNetCore.Mvc;

    public class FilterPostByQueryDto : PagedPostDto
    {
        [FromQuery(Name = "query")]
        public string Query { get; set; }
    }
}
