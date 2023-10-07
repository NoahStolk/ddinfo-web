using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.Spawnsets;

public record EditSpawnset
{
	public required int PlayerId { get; init; }

	[StringLength(64)]
	public required string Name { get; init; }

	[Range(0, 400)]
	public required int? MaxDisplayWaves { get; init; }

	[StringLength(2048)]
	public required string? HtmlDescription { get; init; }

	public required bool IsPractice { get; init; }
}
