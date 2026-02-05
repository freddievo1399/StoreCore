dotnet ef migrations add "$(Get-Date -Format yyyyMMddHHmmss)" `
  --project ../../StoreCore.WebApp.Infrastructure/StoreCore.WebApp.Infrastructure.csproj `
  --startup-project . 
Read-Host "Press Enter to exit"
# InitialCreate