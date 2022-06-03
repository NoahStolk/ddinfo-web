using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public class AddCustomLeaderboard
{
	[Required]
	public int SpawnsetId { get; set; }

	[Required]
	public CustomLeaderboardCategory Category { get; set; }

	public AddCustomLeaderboardDaggers Daggers { get; set; } = new();

	[Required]
	public bool IsFeatured { get; set; }
}
