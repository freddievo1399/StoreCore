dotnet ef migrations add "InitialCreate" `
  --project ../../StoreCore.WebApp.Infrastructure/StoreCore.WebApp.Infrastructure.csproj `
  --startup-project . 
Read-Host "Press Enter to exit"