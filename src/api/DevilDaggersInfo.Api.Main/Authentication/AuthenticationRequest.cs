using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Main.Authentication;

public record AuthenticationRequest
{
	[Required]
	public string Jwt { get; init; } = null!;
}
