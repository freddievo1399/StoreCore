using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StoreCore.WebApp.Abstractions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
LicenseRegister.SyncfusionLicenseRegister(builder.Configuration["SyncfusionLicenseKey"] ?? "");

var app = builder.Build();
await app.RunAsync();