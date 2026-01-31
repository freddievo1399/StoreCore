using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.WebApp.Client
{
    public static class AssembliesClientUtil
    {
        private static List<Assembly> allAssemblies = null;

        private static IEnumerable<Assembly> GetDefaulAssembly => new List<Assembly>
        {
            typeof(StoreCore.WebApp.BaseBlazor._Imports).Assembly,
            typeof(StoreCore.WebApp.Client._Imports).Assembly,
            typeof(StoreCore.WebApp.Abstractions.IRegisterPermistion).Assembly,
            typeof(StoreCore.Product.Shared.RegisterPermistion).Assembly,
        };

        public static IEnumerable<Assembly> GetAssemblies()
        {
            if (allAssemblies == null)
            {
                allAssemblies = [];
                var allAssembliesExtend = GetDefaulAssembly;
                foreach (var asembly in allAssembliesExtend)
                {
                    AddAssembly(asembly);
                }
            }
            foreach (var asembly in AppDomain.CurrentDomain.GetAssemblies().Where(x => x.ManifestModule.Name.StartsWith("StoreCore")))
            {
                AddAssembly(asembly);
            }
            return allAssemblies;
        }
        public static void AddAssembly(Assembly assembly)
        {
            if (allAssemblies == null)
            {
                allAssemblies = new List<Assembly>();
            }
            var assemblyTemp = allAssemblies.FirstOrDefault(t => t.FullName == assembly.FullName);
            if (assemblyTemp == null)
            {
                allAssemblies.Add(assembly);

            }
            else
            {
                if (!allAssemblies.Any(t => t.ManifestModule.ModuleVersionId == assembly.ManifestModule.ModuleVersionId))
                {
                    allAssemblies.Remove(assemblyTemp);
                    allAssemblies.Add(assembly);
                }
            }
        }
        public static void AddAssemblies(IEnumerable<Assembly> assemblys)
        {
            foreach (var assembly in assemblys)
            {
                AddAssembly(assembly);
            }
        }
        public static IEnumerable<Assembly> GetAssembliesBlazor()
        {
            if (allAssemblies == null)
            {
                GetAssemblies();
            }
            return allAssemblies?.Where(x => x.ManifestModule.Name.EndsWith(".Blazor.dll")) ?? new List<Assembly>();
        }
        public static IEnumerable<Assembly> GetAssembliesServer()
        {
            if (allAssemblies == null)
            {
                GetAssemblies();
            }
            return allAssemblies?.Where(x => x.ManifestModule.Name.EndsWith(".Server.dll")) ?? new List<Assembly>();
        }
        public static IEnumerable<TypeInfo> GetTypes<T>(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T))));
        }
    }
}
