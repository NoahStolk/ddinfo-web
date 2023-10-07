namespace DevilDaggersInfo.Api.Admin.Database;

public record GetDatabaseTableEntry
{
	public required string Name { get; init; }

	public required int Count { get; init; }

	public required int DataSize { get; init; }

	public required int IndexSize { get; init; }
}
