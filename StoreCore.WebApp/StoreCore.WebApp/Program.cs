using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi.Models;
using StoreCore.WebApp;
using StoreCore.WebApp.Abstractions;
using StoreCore.WebApp.BaseBlazor;
using StoreCore.WebApp.Infrastructure;
using StoreCore.WebApp.Client.Component;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
var ConnectionStrings__DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine(ConnectionStrings__DefaultConnection);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("Admin", new OpenApiInfo { Title = "Admin API", Version = "v1" });
    option.SwaggerDoc("Customer", new OpenApiInfo { Title = "Customer API", Version = "v1" });
    option.SwaggerDoc("Auth", new OpenApiInfo { Title = "Auth API", Version = "v1" });

    option.DocInclusionPredicate((groupName, apiDesc) => apiDesc.GroupName == groupName);
});
builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped(typeof(IAppService<>),typeof(AutoDI<>));
builder.Services.AddScoped<IRouterConfig, RouterConfig>();
builder.Services.AddScoped<LazyAssemblyLoader>();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"/app/dpkeys"));


LicenseRegister.SyncfusionLicenseRegister(builder.Configuration["SyncfusionLicenseKey"] ?? "");
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/Admin/swagger.json", "Admin API");
        options.SwaggerEndpoint("/swagger/Customer/swagger.json", "Customer API");
        options.SwaggerEndpoint("/swagger/Auth/swagger.json", "Auth API");
        // các doc api
    });
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();
var assemblies = AssembliesUtil.GetAssembliesBlazor().ToList();
foreach (var assemblie in assemblies)
{
    Console.WriteLine($"{assemblie.FullName}: {assemblie.ManifestModule.ModuleVersionId}");
}
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(assemblies.ToArray());
app.UseStatusCodePagesWithRedirects("/Error/{0}");

app.Run();
