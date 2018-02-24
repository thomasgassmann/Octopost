namespace Octopost.Model.Data
{
    using Octopost.Model.Interfaces;
    using System;

    public class Post : IIdentifiable, ICreated, ILocatable
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public string Topic { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public long LocationNameId { get; set; }

        public LocationName LocationName { get; set; }

        public long? FileId { get; set; }

        public File File { get; set; }

        public DateTime Created { get; set; }
    }
}
