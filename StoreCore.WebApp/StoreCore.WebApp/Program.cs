using Microsoft.AspNetCore.Components.WebAssembly.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StoreCore.WebApp;
using StoreCore.WebApp.Abstractions;
using StoreCore.WebApp.BaseBlazor;
using StoreCore.WebApp.Client.Component;
using StoreCore.WebApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
var ConnectionStrings__DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
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


builder.Services.AddScoped(typeof(IAppService<>), typeof(AutoDI<>));
builder.Services.AddScoped<IRouterConfig, RouterConfig>();
builder.Services.AddScoped<LazyAssemblyLoader>();
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    options.UseNpgsql(ConnectionStrings__DefaultConnection);
    options.EnableSensitiveDataLogging();
});
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"/app/dpkeys"));


LicenseRegister.SyncfusionLicenseRegister(builder.Configuration["SyncfusionLicenseKey"] ?? "");
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();
}

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();
app.UseStatusCodePagesWithRedirects("/Error/{0}");

app.Run();
