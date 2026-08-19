namespace DevilDaggersInfo.Web.ApiSpec.Admin.Users;

public sealed record ToggleRole
{
	public required string RoleName { get; init; }
}
