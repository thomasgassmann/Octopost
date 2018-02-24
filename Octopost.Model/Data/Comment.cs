namespace Octopost.Model.Data
{
    using Octopost.Model.Interfaces;
    using System;

    public class Comment : IIdentifiable, ICreated, ILocatable
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public long PostId { get; set; }

        public Post Post { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public long LocationNameId { get; set; }

        public LocationName LocationName { get; set; }

        public DateTime Created { get; set; }
    }
}
