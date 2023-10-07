namespace DevilDaggersInfo.Web.ApiSpec.Admin.Users;

public record ToggleRole
{
	public required string RoleName { get; init; }
}
