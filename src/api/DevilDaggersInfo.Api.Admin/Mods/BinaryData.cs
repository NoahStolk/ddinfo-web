namespace DevilDaggersInfo.Api.Admin.Mods;

public record BinaryData
{
	/// <summary>
	/// This name should not contain the type prefix or the mod name.
	/// </summary>
	public string Name { get; set; } = string.Empty;

	public byte[] Data { get; set; } = Array.Empty<byte>();
}
