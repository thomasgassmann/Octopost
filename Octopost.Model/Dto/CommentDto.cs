namespace Octopost.Model.Dto
{
    using System;

    public class CommentDto
    {
        public long Id { get; set; }

        public long PostId { get; set; }

        public string Text { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime Created { get; set; }

        public string LocationName { get; set; }
    }
}
