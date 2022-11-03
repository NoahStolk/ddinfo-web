using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Main.Authentication;

public record UpdateNameRequest
{
	public string CurrentName { get; init; } = null!;

	public string CurrentPassword { get; init; } = null!;

	[Required]
	[StringLength(32, MinimumLength = 2)]
	public string NewName { get; init; } = null!;
}
