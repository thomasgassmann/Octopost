namespace Octopost.Model.Dto.Location
{
    using Newtonsoft.Json;

    public class LocationApiDto
    {
        [JsonProperty(PropertyName = "place_id")]
        public string PlaceId { get; set; }

        [JsonProperty(PropertyName = "license")]
        public string License { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "lon")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "address")]
        public AddressDto Address { get; set; }
    }
}
