namespace Octopost.WebApi.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class AssemblyUtilities
    {
        private const string OctopostAssemblyIdentifier = "Octopost";

        private static Lazy<List<Assembly>> octopostAssemblies = new Lazy<List<Assembly>>(
            () => AssemblyUtilities.IterateAssemblies().ToList());

        public static IEnumerable<Assembly> GetOctopostAssemblies() =>
            AssemblyUtilities.octopostAssemblies.Value;

        private static IEnumerable<Assembly> IterateAssemblies()
        {
            yield return Assembly.GetExecutingAssembly();
            var assemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly.Name.Contains(AssemblyUtilities.OctopostAssemblyIdentifier))
                {
                    var loaded = Assembly.Load(assembly);
                    yield return loaded;
                }
            }
        }
    }
}
