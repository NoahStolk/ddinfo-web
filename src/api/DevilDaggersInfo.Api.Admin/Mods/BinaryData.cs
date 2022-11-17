using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.Mods;

public record BinaryData
{
	/// <summary>
	/// This name should not contain the type prefix or the mod name.
	/// </summary>
	[StringLength(64, MinimumLength = 1)]
	public required string Name { get; init; }

	public required byte[] Data { get; init; }
}
