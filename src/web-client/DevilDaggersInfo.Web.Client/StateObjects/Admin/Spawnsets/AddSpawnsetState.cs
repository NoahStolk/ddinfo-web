using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.Spawnsets;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Client.StateObjects.Admin.Spawnsets;

public class AddSpawnsetState : IStateObject<AddSpawnset>
{
	[Required]
	public int PlayerId { get; set; }

	[StringLength(64)]
	public required string Name { get; set; }

	[Range(0, 400)]
	public int? MaxDisplayWaves { get; set; }

	[StringLength(2048)]
	public string? HtmlDescription { get; set; }

	public bool IsPractice { get; set; }

	[MaxLength(SpawnsetConstants.MaxFileSize, ErrorMessage = SpawnsetConstants.MaxFileSizeErrorMessage)]
	public required byte[] FileContents { get; set; }

	public AddSpawnset ToModel() => new()
	{
		PlayerId = PlayerId,
		Name = Name,
		MaxDisplayWaves = MaxDisplayWaves,
		HtmlDescription = HtmlDescription,
		IsPractice = IsPractice,
		FileContents = FileContents,
	};
}
