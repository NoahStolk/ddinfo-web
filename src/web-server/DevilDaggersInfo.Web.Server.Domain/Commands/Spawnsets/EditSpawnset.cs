namespace DevilDaggersInfo.Web.Server.Domain.Commands.Spawnsets;

public record EditSpawnset
{
	public int SpawnsetId { get; init; }

	public int PlayerId { get; init; }

	public string Name { get; init; } = null!;

	public int? MaxDisplayWaves { get; init; }

	public string? HtmlDescription { get; init; }

	public bool IsPractice { get; init; }
}
