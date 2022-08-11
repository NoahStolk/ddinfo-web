using DevilDaggersInfo.Types.Web;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.CustomLeaderboards;

public record EditCustomLeaderboard
{
	[Required]
	public CustomLeaderboardCategory Category { get; set; }

	public AddCustomLeaderboardDaggers Daggers { get; set; } = new();

	[Required]
	public bool IsFeatured { get; set; }
}
