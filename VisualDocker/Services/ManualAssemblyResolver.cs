using System;
using System.Reflection;

namespace VisualDocker.Services
{
    public class ManualAssemblyResolver : IDisposable
    {
        public ManualAssemblyResolver(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");

            _assemblies = new[] { assembly };
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        public ManualAssemblyResolver(params Assembly[] assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException("assemblies");

            if (assemblies.Length == 0)
                throw new ArgumentException("Assemblies should be not empty.", "assemblies");

            _assemblies = assemblies;
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;
        }

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            string path = Assembly.GetExecutingAssembly().Location;
            path = System.IO.Path.GetDirectoryName(path);

            if (args.Name.Contains("System.Reactive.Core") ||
                args.Name.Contains("System.Reactive.Interfaces"))
            {
                path = System.IO.Path.Combine(path, $"{args.Name.Substring(0, args.Name.IndexOf(", "))}.dll");
                Assembly ret = Assembly.LoadFrom(path);
                return ret;
            }

            return null;
        }

        private readonly Assembly[] _assemblies;
    }

}