using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.Web.Server.Domain.Models.Players;

public record PlayerForLeaderboard
{
	public int Id { get; init; }

	public BanType BanType { get; init; }

	public string? BanDescription { get; init; }

	public int? BanResponsibleId { get; init; }

	public string? CountryCode { get; init; }
}
