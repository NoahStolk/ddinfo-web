namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards;

public class EditCustomLeaderboard
{
	[Required]
	public CustomLeaderboardCategory Category { get; set; }

	public AddCustomLeaderboardDaggers? Daggers { get; set; }

	[Required]
	public bool IsFeatured { get; set; }
}
