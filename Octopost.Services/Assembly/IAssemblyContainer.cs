namespace Octopost.Services.Assembly
{
    using System.Reflection;

    public interface IAssemblyContainer
    {
        Assembly[] GetAssemblies();
    }
}
