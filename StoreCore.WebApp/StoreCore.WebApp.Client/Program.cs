using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using StoreCore.WebApp.BaseBlazor;
using StoreCore.WebApp.Client;
using Syncfusion.Blazor;
using System.Net.Http.Json;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});
builder.Services.AddScoped<LazyAssemblyLoader>();
builder.Services.AddScoped<IRouterConfig, RouterConfig>();
var app = builder.Build();
AssembliesClientUtil.GetDefaulAssembly = () => new List<Assembly>
{
    typeof(StoreCore.WebApp.BaseBlazor._Imports).Assembly,
    typeof(StoreCore.WebApp.Client._Imports).Assembly,
    typeof(StoreCore.Product.Shared.RegisterPermistion).Assembly,
    typeof(StoreCore.WebApp.Abstractions.IRegisterPermistion).Assembly
};
var abc = AssembliesClientUtil.GetAssemblies();
await app.RunAsync();
