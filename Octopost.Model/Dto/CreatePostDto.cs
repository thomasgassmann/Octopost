namespace Octopost.Model.Dto
{
    using Octopost.Model.Interfaces;

    public class CreatePostDto : ILocatable
    {
        public string Text { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public long? FileId { get; set; }
    }
}
