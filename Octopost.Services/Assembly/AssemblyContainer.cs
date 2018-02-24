namespace Octopost.Services.Assembly
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class AssemblyContainer : IAssemblyContainer
    {
        private readonly Lazy<Assembly[]> assemblies;

        public AssemblyContainer(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = new Lazy<Assembly[]>(() => assemblies.ToArray());
        }

        public Assembly[] GetAssemblies() =>
            this.assemblies.Value;
    }
}
