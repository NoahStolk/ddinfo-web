namespace DevilDaggersInfo.Api.Main.Players;

public record GetPlayer
{
	public required int Id { get; init; }

	public required bool IsBanned { get; init; }

	public required string? BanDescription { get; init; }

	public required bool IsPublicDonator { get; init; }

	public required string? CountryCode { get; init; }

	public required GetPlayerSettings? Settings { get; init; }
}
