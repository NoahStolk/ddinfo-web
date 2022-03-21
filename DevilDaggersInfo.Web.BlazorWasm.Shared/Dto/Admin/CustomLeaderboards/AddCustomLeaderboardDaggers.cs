namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards;

public class AddCustomLeaderboardDaggers
{
	[Range(1, 1500)]
	public double Bronze { get; set; }

	[Range(1, 1500)]
	public double Silver { get; set; }

	[Range(1, 1500)]
	public double Golden { get; set; }

	[Range(1, 1500)]
	public double Devil { get; set; }

	[Range(1, 1500)]
	public double Leviathan { get; set; }
}
