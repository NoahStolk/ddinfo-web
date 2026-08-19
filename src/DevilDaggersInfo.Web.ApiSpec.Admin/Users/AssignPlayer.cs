namespace DevilDaggersInfo.Web.ApiSpec.Admin.Users;

public sealed record AssignPlayer
{
	public required int PlayerId { get; set; }
}
