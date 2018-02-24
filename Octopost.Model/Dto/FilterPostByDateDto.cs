namespace Octopost.Model.Dto
{
    using Microsoft.AspNetCore.Mvc;
    using System;

    public class FilterPostByDateDto : PagedPostDto
    {
        [FromQuery(Name = "from")]
        public DateTime? From { get; set; }

        [FromQuery(Name = "to")]
        public DateTime? To { get; set; }
    }
}
