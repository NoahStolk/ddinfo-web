using DevilDaggersInfo.Web.Server.Domain.Entities;

namespace DevilDaggersInfo.Web.Server.Domain.Models.Players;

public record Player
{
	public int Id { get; init; }

	public bool IsBanned { get; init; }

	public string? BanDescription { get; init; }

	public bool IsPublicDonator { get; init; }

	public string? CountryCode { get; init; }

	public PlayerSettings? Settings { get; init; }

	public static Player FromEntity(PlayerEntity player, bool isPublicDonator) => new()
	{
		BanDescription = player.BanDescription,
		CountryCode = player.CountryCode,
		Id = player.Id,
		IsBanned = player.BanType != Types.Web.BanType.NotBanned,
		IsPublicDonator = isPublicDonator,
		Settings = player.HasVisibleSettings() ? PlayerSettings.FromEntity(player) : null,
	};
}
