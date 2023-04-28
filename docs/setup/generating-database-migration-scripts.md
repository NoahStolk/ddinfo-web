# Generating database migration scripts

You can update the migration tooling using `dotnet tool update --global dotnet-ef`.

1. Package references to `Microsoft.EntityFrameworkCore.Relational` and `Pomelo.EntityFrameworkCore.MySql` must temporarily be added to `DevilDaggersInfo.Web.Server.Domain`.
2. `cd src/web-server/DevilDaggersInfo.Web.Server.Domain`
3. `dotnet ef migrations add V1 --startup-project ../DevilDaggersInfo.Web.Server/DevilDaggersInfo.Web.Server.csproj`
4. Make changes to entities.
5. `dotnet ef migrations add V2 --startup-project ../DevilDaggersInfo.Web.Server/DevilDaggersInfo.Web.Server.csproj`
6. `dotnet ef migrations script V1 V2 --startup-project ../DevilDaggersInfo.Web.Server/DevilDaggersInfo.Web.Server.csproj`
