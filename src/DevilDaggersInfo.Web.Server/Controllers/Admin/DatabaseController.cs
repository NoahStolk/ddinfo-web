using DevilDaggersInfo.Web.ApiSpec.Admin.Database;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/database")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public sealed class DatabaseController(ApplicationDbContext dbContext) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetDatabaseTableEntry>>> GetDatabaseInfo()
	{
		// The schema is taken from the connection rather than hard-coded, so this reports on whichever database the
		// application is actually configured against.
		string schema = dbContext.Database.GetDbConnection().Database;

		List<InformationSchemaTable> tables = await dbContext.InformationSchemaTables
			.FromSqlRaw(
				// Two dollar signs so that {{ }} interpolates the column names while {0} stays a literal placeholder
				// for the parameter that follows.
				$$"""
				SELECT
					table_name AS `{{nameof(InformationSchemaTable.Table)}}`,
					data_length `{{nameof(InformationSchemaTable.DataSize)}}`,
					index_length `{{nameof(InformationSchemaTable.IndexSize)}}`,
					avg_row_length `{{nameof(InformationSchemaTable.AverageRowLength)}}`,
					table_rows `{{nameof(InformationSchemaTable.TableRows)}}`
				FROM information_schema.TABLES
				WHERE table_schema = {0}
				ORDER BY table_name ASC;
				""",
				schema)
			.ToListAsync();

		return tables
			.OrderBy(t => t.Table)
			.Select(t => new GetDatabaseTableEntry
			{
				DataSize = t.DataSize,
				IndexSize = t.IndexSize,
				Count = t.TableRows,
				Name = t.Table ?? string.Empty,
			})
			.ToList();
	}
}
