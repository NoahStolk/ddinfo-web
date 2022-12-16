using DevilDaggersInfo.Api.Admin.Database;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/database")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class DatabaseController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;

	public DatabaseController(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetDatabaseTableEntry>>> GetDatabaseInfo()
	{
		List<InformationSchemaTable> tables = await _dbContext.InformationSchemaTables
			.FromSqlRaw($"""
				SELECT
					table_name AS `{nameof(InformationSchemaTable.Table)}`,
					data_length `{nameof(InformationSchemaTable.DataSize)}`,
					index_length `{nameof(InformationSchemaTable.IndexSize)}`,
					avg_row_length `{nameof(InformationSchemaTable.AverageRowLength)}`,
					table_rows `{nameof(InformationSchemaTable.TableRows)}`
				FROM information_schema.TABLES
				WHERE table_schema = 'devildaggers'
				ORDER BY table_name ASC;
				""")
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
