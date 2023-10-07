using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.ApiSpec.Admin.Spawnsets;

public record AddSpawnset
{
	public required int PlayerId { get; init; }

	[StringLength(64)]
	public required string Name { get; init; }

	[Range(0, 400)]
	public required int? MaxDisplayWaves { get; init; }

	[StringLength(2048)]
	public required string? HtmlDescription { get; init; }

	public required bool IsPractice { get; init; }

	[MaxLength(SpawnsetConstants.MaxFileSize, ErrorMessage = SpawnsetConstants.MaxFileSizeErrorMessage)]
	public required byte[] FileContents { get; init; }
}
