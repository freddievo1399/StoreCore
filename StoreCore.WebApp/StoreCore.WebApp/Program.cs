using Microsoft.OpenApi.Models;
using StoreCore.WebApp;
using StoreCore.WebApp.Abstractions;
using StoreCore.WebApp.Client.Service;
using StoreCore.WebApp.Components;
using StoreCore.WebApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

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


builder.Services.AddScoped(typeof(AutoDI<>), typeof(IAppService<>));


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
assemblies.Add(typeof(StoreCore.WebApp.Client._Imports).Assembly);
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(assemblies.ToArray());
app.UseStatusCodePagesWithRedirects("/Error/{0}");
app.Run();
