namespace Octopost.Model.Dto
{
    using Microsoft.AspNetCore.Mvc;

    public class CreateVoteDto
    {
        [FromRoute(Name = "postId")]
        public long PostId { get; set; }

        [FromQuery(Name = "state")]
        public string VoteState { get; set; }
    }
}
