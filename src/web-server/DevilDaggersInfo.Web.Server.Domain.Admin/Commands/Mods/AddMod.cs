using DevilDaggersInfo.Web.Server.Domain.Admin.Commands.Mods.Models;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Commands.Mods;

public record AddMod
{
	public string Name { get; init; } = null!;

	public bool IsHidden { get; init; }

	public string? TrailerUrl { get; init; }

	public string? HtmlDescription { get; init; }

	public List<int>? ModTypes { get; init; }

	public string? Url { get; init; }

	public List<int>? PlayerIds { get; init; }

	public List<BinaryData> Binaries { get; init; } = new();

	public Dictionary<string, byte[]> Screenshots { get; init; } = new();
}
