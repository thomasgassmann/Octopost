namespace Octopost.Services.Location
{
    using Octopost.Model.Interfaces;

    public interface ILocationNameService
    {
        long NameLocation(ILocatable locatable);
    }
}
