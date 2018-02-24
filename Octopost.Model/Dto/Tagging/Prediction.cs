namespace Octopost.Model.Dto.Tagging
{
    using Newtonsoft.Json;

    public class Prediction
    {
        [JsonProperty("prediction_text")]
        public string Text { get; set; }

        [JsonProperty("prediction_value")]
        public long Value { get; set; }
    }
}
