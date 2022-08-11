namespace DevilDaggersInfo.Web.Server.Domain.Admin.Commands.Spawnsets;

public record AddSpawnset
{
	public int PlayerId { get; init; }

	public string Name { get; init; } = null!;

	public int? MaxDisplayWaves { get; init; }

	public string? HtmlDescription { get; init; }

	public bool IsPractice { get; init; }

	public byte[] FileContents { get; init; } = null!;
}
