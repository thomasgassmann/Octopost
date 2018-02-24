namespace Octopost.Services.Tagging
{
    using Newtonsoft.Json;
    using Octopost.Model.Dto.Tagging;
    using Octopost.Model.Settings;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text.Encodings.Web;

    public class PostTaggingService : IPostTaggingService
    {
        private readonly OctopostSettings octopostSettings;

        private Dictionary<long, string> classes;

        private WebClient webClient = new WebClient();

        public PostTaggingService(OctopostSettings octopostSettings)
        {
            this.octopostSettings = octopostSettings;
            if (string.IsNullOrEmpty(this.octopostSettings.UClassifyApiKey))
            {
                this.FillClasses();
            }
            else
            {
                this.FillClassesUClassify();
            }
        }

        public IDictionary<long, string> GetTags() => this.classes;
        
        public string PredictTag(string text)
        {
            if (string.IsNullOrEmpty(this.octopostSettings.UClassifyApiKey))
            {
                var encodedQuery = UrlEncoder.Default.Encode(text);
                var apiUrl = this.octopostSettings.TopicClassifierUrl + "/prediction?text=" + encodedQuery;
                var result = this.webClient.DownloadString(apiUrl);
                var prediction = JsonConvert.DeserializeObject<Prediction>(result);
                return prediction.Text;
            }

            var apiKey = this.octopostSettings.UClassifyApiKey;
            var url = $"https://api.uclassify.com/v1/uclassify/topics/classify?readkey={apiKey}&text={text}";
            using (var webClient = new WebClient())
            {
                var str = webClient.DownloadString(url);
                var result = JsonConvert.DeserializeObject<Dictionary<string, double>>(str);
                var max = result.Select(x => new { Class = x.Key, Probability = x.Value }).OrderByDescending(x => x.Probability).FirstOrDefault();
                return max.Class;
            }
        }

        private void FillClassesUClassify()
        {
            if (this.classes == null)
            {
                this.classes = new Dictionary<long, string>
                {
                    { 0, "Arts" },
                    { 1, "Business" },
                    { 2, "Computers" },
                    { 3, "Games" },
                    { 4, "Health" },
                    { 5, "Home" },
                    { 6, "Recreation" },
                    { 7, "Science" },
                    { 8, "Society" },
                    { 9, "Sports" }
                };
            }
        }

        private void FillClasses()
        {
            if (this.classes == null)
            {
                var url = this.octopostSettings.TopicClassifierUrl + "/classes";
                var result = this.webClient.DownloadString(url);
                var parsed = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                this.classes = new Dictionary<long, string>();
                foreach (var item in parsed)
                {
                    this.classes.Add(long.Parse(item.Key), item.Value);
                }
            }
        }
    }
}
