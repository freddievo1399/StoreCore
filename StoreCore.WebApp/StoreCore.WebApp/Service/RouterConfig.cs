using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using StoreCore.WebApp.BaseBlazor;
using StoreCore.WebApp.Client.Component;
using StoreCore.WebApp.Infrastructure;
using System.Reflection;

namespace StoreCore.WebApp;

public class RouterConfig() : IRouterConfig
{
    public async Task OnNavigateAsync(NavigationContext args)
    {
        if (Routes.assemblies.Count == 0)
        {
            Routes.assemblies = AssembliesUtil.GetAssembliesBlazor().ToList();
        }
    }
}
