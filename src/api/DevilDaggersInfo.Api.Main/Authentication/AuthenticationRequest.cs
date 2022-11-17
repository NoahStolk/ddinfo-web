using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Main.Authentication;

public record AuthenticationRequest
{
	[Required]
	public required string Jwt { get; init; }
}
