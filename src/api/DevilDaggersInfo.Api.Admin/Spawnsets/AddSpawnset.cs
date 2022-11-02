using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.Spawnsets;

public record AddSpawnset
{
	[Required]
	public int PlayerId { get; init; }

	[StringLength(64)]
	public string Name { get; init; } = null!;

	[Range(0, 400)]
	public int? MaxDisplayWaves { get; init; }

	[StringLength(2048)]
	public string? HtmlDescription { get; init; }

	public bool IsPractice { get; init; }

	[MaxLength(SpawnsetConstants.MaxFileSize, ErrorMessage = SpawnsetConstants.MaxFileSizeErrorMessage)]
	public byte[] FileContents { get; init; } = null!;
}
