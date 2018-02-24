namespace Octopost.Services.Tagging
{
    using System.Collections.Generic;

    public interface IPostTaggingService
    {
        string PredictTag(string text);

        IDictionary<long, string> GetTags(); 
    }
}
