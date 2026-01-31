using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using StoreCore.WebApp.BaseBlazor;
using StoreCore.WebApp.Client.Component;
using System.Reflection;

namespace StoreCore.WebApp.Client;

public class RouterConfig(LazyAssemblyLoader AssemblyLoader) : IRouterConfig
{
    public async Task OnNavigateAsync(NavigationContext args)
    {
        try
        {
            var moduleName = args.Path.Split("/")[0] ?? "";
            IEnumerable<Assembly> assemblies = [];

            switch (moduleName)
            {
                case "product":
                    await LazyLoadModule("StoreCore.Product.Blazor");
                    break;
                default:
                    Console.WriteLine("haha");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("exxxx" + ex.Message);
        }
    }
    private async Task LazyLoadModule(string Name)
    {
        var isExit = Routes.assemblies.Exists(x => x.FullName == Name);
        if (isExit)
        {
            return;
        }
        var assemblies = await AssemblyLoader.LoadAssembliesAsync([$"{Name}.wasm"]);
        AssembliesClientUtil.AddAssemblies(assemblies);
        Routes.assemblies.AddRange(assemblies);
    }
}
