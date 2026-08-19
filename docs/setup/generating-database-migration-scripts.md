# Generating database migration scripts

You can update the migration tooling using `dotnet tool update --global dotnet-ef`. The tool major version must match the EF Core version the project uses (10.x).

1. Package references to `Microsoft.EntityFrameworkCore.Relational` and `MySql.EntityFrameworkCore` must temporarily be added to `DevilDaggersInfo.Web.Server.Domain` (both are already present as commented-out entries in its csproj).
2. `cd src/DevilDaggersInfo.Web.Server.Domain`
3. `dotnet ef migrations add V1 --startup-project ../DevilDaggersInfo.Web.Server/DevilDaggersInfo.Web.Server.csproj`
4. Make changes to entities.
5. `dotnet ef migrations add V2 --startup-project ../DevilDaggersInfo.Web.Server/DevilDaggersInfo.Web.Server.csproj`
6. `dotnet ef migrations script V1 V2 --startup-project ../DevilDaggersInfo.Web.Server/DevilDaggersInfo.Web.Server.csproj`
