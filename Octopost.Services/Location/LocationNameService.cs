namespace Octopost.Services.Location
{
    using Newtonsoft.Json;
    using Octopost.Model.Data;
    using Octopost.Model.Dto.Location;
    using Octopost.Model.Interfaces;
    using Octopost.Model.Settings;
    using Octopost.Services.UoW;
    using System;
    using System.Linq;
    using System.Net;

    public class LocationNameService : ILocationNameService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly OctopostSettings octopostSettings;

        public LocationNameService(IUnitOfWorkFactory unitOfWorkFactory, OctopostSettings settings)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.octopostSettings = settings;
        }

        public long NameLocation(ILocatable locatable)
        {
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var maxDifference = this.octopostSettings.MaxDifferenceForLocationName;
                var locationNameRepository = unitOfWork.CreateEntityRepository<LocationName>();
                var locationName = locationNameRepository.Query()
                    .Select(x => new
                    {
                        Name = x,
                        Difference = Math.Sqrt(Math.Pow(Math.Abs(x.Longitude - locatable.Longitude), 2) * Math.Pow(Math.Abs(x.Latitude - locatable.Latitude), 2))
                    })
                    .OrderBy(x => x.Difference)
                    .FirstOrDefault(x => x.Difference <= maxDifference);
                if (locationName != null)
                {
                    return locationName.Name.Id;
                }

                var location = this.GetLocationApiResponse(locatable.Latitude, locatable.Longitude);
                var locationNameToCreate = new LocationName
                {
                    Latitude = location?.Latitude ?? locatable.Latitude,
                    Longitude = location?.Longitude ?? locatable.Longitude,
                    Name = this.GetDisplayName(location)
                };
                locationNameRepository.Create(locationNameToCreate);
                unitOfWork.Save();
                return locationNameToCreate.Id;
            }
        }

        private string GetDisplayName(LocationApiDto location)
        {
            if (location == null)
            {
                return "Unidentified";
            }

            if (location.Address?.Village != null)
            {
                return location.Address.Village;
            }

            if (location.Address?.District != null)
            {
                return location.Address.District;
            }

            if (location.Address?.Country != null)
            {
                return location.Address.Country;
            }

            return location.DisplayName;
        }

        private LocationApiDto GetLocationApiResponse(double lat, double lng)
        {
            var url = this.GetLocationUrl(lat, lng, this.octopostSettings.LocationApiKey);
            using (var webClient = new WebClient())
            {
                var failed = false;
                var response = string.Empty;
                try
                {
                    response = webClient.DownloadString(new Uri(url));
                    failed = response.Contains("Unable to geocode");
                }
                catch (WebException)
                {
                    failed = true;
                }

                if (failed)
                {
                    return null;
                }
                
                var obj = JsonConvert.DeserializeObject<LocationApiDto>(response);
                return obj;
            }
        }

        private string GetLocationUrl(double lat, double lng, string apiKey) =>
            $"http://locationiq.org/v1/reverse.php?key={apiKey}&format=json&lat={lat}&lon={lng}";
    }
}
