using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.Mods;

public record BinaryData
{
	/// <summary>
	/// This name should not contain the type prefix or the mod name.
	/// </summary>
	[StringLength(64, MinimumLength = 1)]
	public string Name { get; set; } = string.Empty;

	public byte[] Data { get; init; } = Array.Empty<byte>();
}
