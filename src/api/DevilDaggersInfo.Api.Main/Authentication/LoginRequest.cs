using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Main.Authentication;

public record LoginRequest
{
	[Required]
	public required string Name { get; set; }

	[Required]
	public required string Password { get; set; }
}
