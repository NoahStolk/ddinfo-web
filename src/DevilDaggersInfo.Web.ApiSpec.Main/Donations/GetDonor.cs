namespace DevilDaggersInfo.Web.ApiSpec.Main.Donations;

public sealed record GetDonor
{
	public required int? PlayerId { get; init; }

	public required string PlayerName { get; init; }

	public required List<GetDonation> Donations { get; init; }
}
