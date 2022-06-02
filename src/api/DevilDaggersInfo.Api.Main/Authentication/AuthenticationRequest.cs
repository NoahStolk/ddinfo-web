using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Shared.Dto.Public.Authentication;

public record AuthenticationRequest
{
	[Required]
	public string Jwt { get; init; } = null!;
}
