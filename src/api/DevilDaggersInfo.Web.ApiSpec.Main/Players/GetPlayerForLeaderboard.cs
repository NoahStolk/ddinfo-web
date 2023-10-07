namespace DevilDaggersInfo.Web.ApiSpec.Main.Players;

public record GetPlayerForLeaderboard
{
	public required int Id { get; init; }

	public required BanType BanType { get; init; }

	public required string? BanDescription { get; init; }

	public required int? BanResponsibleId { get; init; }

	public required string? CountryCode { get; init; }
}
