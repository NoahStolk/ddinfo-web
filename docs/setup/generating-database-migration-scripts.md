# Generating database migration scripts

1. Set the default project to `DevilDaggersInfo.Web.Server.Domain`.
2. Set the startup project to `DevilDaggersInfo.Web.Server`.
3. Packages references to `Microsoft.EntityFrameworkCore.Relational` and `Pomelo.EntityFrameworkCore.MySql` must temporarily be added to `DevilDaggersInfo.Web.Server.Domain`.
4. Create initial migration: `add-migration V1`
5. Make changes to entities.
6. Create new migration: `add-migration V2`
7. Generate script: `script-migration V1 V2`
