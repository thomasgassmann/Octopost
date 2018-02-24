namespace Octopost.Model.Data
{
    using Octopost.Model.Interfaces;

    public class LocationName : ILocatable, IIdentifiable
    {
        public long Id { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string Name { get; set; }
    }
}
