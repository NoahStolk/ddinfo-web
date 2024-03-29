using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Models.Players;

public record Player
{
	public required int Id { get; init; }

	public required bool IsBanned { get; init; }

	public required string? BanDescription { get; init; }

	public required bool IsPublicDonor { get; init; }

	public required string? CountryCode { get; init; }

	public required PlayerSettings? Settings { get; init; }

	public static Player FromEntity(PlayerEntity player, bool isPublicDonor)
	{
		return new()
		{
			BanDescription = player.BanDescription,
			CountryCode = player.CountryCode,
			Id = player.Id,
			IsBanned = player.BanType != BanType.NotBanned,
			IsPublicDonor = isPublicDonor,
			Settings = player.HasVisibleSettings() ? PlayerSettings.FromEntity(player) : null,
		};
	}
}
