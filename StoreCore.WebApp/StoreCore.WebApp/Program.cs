using StoreCore.WebApp.Abstractions;
using StoreCore.WebApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
LicenseRegister.SyncfusionLicenseRegister(builder.Configuration["SyncfusionLicenseKey"]??"");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies([typeof(StoreCore.WebApp.Client._Imports).Assembly, typeof(StoreCore.Product.Blazor._Imports).Assembly]);
app.UseStatusCodePagesWithRedirects("/Error/{0}");
app.Run();
