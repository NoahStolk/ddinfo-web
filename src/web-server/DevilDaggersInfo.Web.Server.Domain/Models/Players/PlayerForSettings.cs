using DevilDaggersInfo.Web.Server.Domain.Entities;

namespace DevilDaggersInfo.Web.Server.Domain.Models.Players;

public record PlayerForSettings
{
	public int Id { get; init; }

	public string? CountryCode { get; init; }

	public PlayerSettings Settings { get; init; } = null!;

	public static PlayerForSettings FromEntity(PlayerEntity player) => new()
	{
		CountryCode = player.CountryCode,
		Id = player.Id,
		Settings = PlayerSettings.FromEntity(player),
	};
}
