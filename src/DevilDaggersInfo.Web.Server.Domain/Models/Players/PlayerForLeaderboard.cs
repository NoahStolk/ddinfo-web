using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Models.Players;

public record PlayerForLeaderboard
{
	public required int Id { get; init; }

	public required BanType BanType { get; init; }

	public required string? BanDescription { get; init; }

	public required int? BanResponsibleId { get; init; }

	public required string? CountryCode { get; init; }
}
