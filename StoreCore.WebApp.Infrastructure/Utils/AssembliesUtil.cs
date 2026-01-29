using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StoreCore.WebApp.Infrastructure;

public static class AssembliesUtil
{
    private static List<Assembly> allAssemblies = null;
    public static IEnumerable<Assembly> GetAssemblies()
    {
        if (allAssemblies == null)
        {
            var modules = new List<Assembly>();
            var abc = Assembly.GetExecutingAssembly();
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var files = Directory.GetFiles(path, "*.dll");

            foreach (string dll in files.Where(x => Path.GetFileName(x).StartsWith("StoreCore")))
            {
                if (modules.Any(t => t.GetName().Name == dll))
                    continue;

                modules.Add(Assembly.LoadFile(dll));
            }

            allAssemblies = modules;
        }

        return allAssemblies;
    }
    public static IEnumerable<Assembly> GetAssembliesBlazor()
    {
        if (allAssemblies == null)
        {
            GetAssemblies();
        }

        var Assemblies = allAssemblies?.Where(x => x.ManifestModule.Name.Contains(".Blazor")).ToList() ?? new List<Assembly>();
        return Assemblies;
    }
    public static IEnumerable<Assembly> GetAssembliesServer()
    {
        if (allAssemblies == null)
        {
            GetAssemblies();
        }

        var Assemblies = allAssemblies?.Where(x => x.ManifestModule.Name.EndsWith(".Server")).ToList() ?? new List<Assembly>();
        return Assemblies;
    }
    public static IEnumerable<TypeInfo> GetTypes<T>(this IEnumerable<Assembly> assemblies)
    {
        return assemblies.SelectMany(a => a.DefinedTypes.Where(x => x.GetInterfaces().Contains(typeof(T))));
    }
}
