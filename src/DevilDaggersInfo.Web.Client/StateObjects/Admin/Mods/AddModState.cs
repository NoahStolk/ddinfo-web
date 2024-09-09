using DevilDaggersInfo.Web.ApiSpec.Admin;
using DevilDaggersInfo.Web.ApiSpec.Admin.Mods;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Client.StateObjects.Admin.Mods;

public class AddModState : IStateObject<AddMod>
{
	[StringLength(64)]
	public string Name { get; set; } = string.Empty;

	public bool IsHidden { get; set; }

	[StringLength(64)]
	public string? TrailerUrl { get; set; }

	[StringLength(2048)]
	public string? HtmlDescription { get; set; }

	public List<int>? ModTypes { get; set; }

	[StringLength(128)]
	public string? Url { get; set; }

	public List<int>? PlayerIds { get; set; }

	[MaxLength(ModConstants.BinaryMaxFiles, ErrorMessage = ModConstants.BinaryMaxFilesErrorMessage)]
	public List<BinaryDataState> Binaries { get; set; } = [];

	[MaxLength(ModConstants.ScreenshotMaxFiles, ErrorMessage = ModConstants.ScreenshotMaxFilesErrorMessage)]
	public Dictionary<string, byte[]> Screenshots { get; set; } = new();

	public AddMod ToModel()
	{
		return new AddMod
		{
			Name = Name,
			IsHidden = IsHidden,
			TrailerUrl = TrailerUrl,
			HtmlDescription = HtmlDescription,
			ModTypes = ModTypes,
			Url = Url,
			PlayerIds = PlayerIds,
			Binaries = Binaries.ConvertAll(b => b.ToModel()),
			Screenshots = Screenshots,
		};
	}
}
