using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.ApiSpec.Main.Authentication;

public record UpdateNameRequest
{
	public required string CurrentName { get; init; }

	public required string CurrentPassword { get; init; }

	[StringLength(32, MinimumLength = 2)]
	public required string NewName { get; init; }
}
