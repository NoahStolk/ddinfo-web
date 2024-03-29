using DevilDaggersInfo.Web.Server.Domain.Entities;

namespace DevilDaggersInfo.Web.Server.Domain.Models.Players;

public record PlayerForSettings
{
	public required int Id { get; init; }

	public required string? CountryCode { get; init; }

	public required PlayerSettings Settings { get; init; }

	public static PlayerForSettings FromEntity(PlayerEntity player)
	{
		return new()
		{
			CountryCode = player.CountryCode,
			Id = player.Id,
			Settings = PlayerSettings.FromEntity(player),
		};
	}
}
