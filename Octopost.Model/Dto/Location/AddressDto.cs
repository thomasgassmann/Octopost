namespace Octopost.Model.Dto.Location
{
    using Newtonsoft.Json;

    public class AddressDto
    {
        [JsonProperty(PropertyName = "village")]
        public string Village { get; set; }

        [JsonProperty(PropertyName = "state_district")]
        public string District { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "country_code")]
        public string CountryCode { get; set; }
    }
}
