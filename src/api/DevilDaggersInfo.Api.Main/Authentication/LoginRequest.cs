using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Main.Authentication;

public record LoginRequest
{
	[Required]
	public string Name { get; set; } = null!;

	[Required]
	public string Password { get; set; } = null!;
}
