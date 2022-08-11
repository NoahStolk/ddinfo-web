namespace DevilDaggersInfo.Web.Server.Domain.Commands.Mods.Models;

public record BinaryData
{
	// TODO: Required.
	public string Name { get; init; } = string.Empty;

	// TODO: Required.
	public byte[] Data { get; init; } = Array.Empty<byte>();
}
