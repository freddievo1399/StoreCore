using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StoreCore.WebApp.Abstractions;
using StoreCore.WebApp.Client;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddSyncfusionBlazor();
LicenseRegister.SyncfusionLicenseRegister(builder.Configuration["SyncfusionLicenseKey"] ?? "");
builder.Services.AddScoped(typeof(IAppService<>), typeof(HttpService<>));

//builder.
var app = builder.Build();
await app.RunAsync();