namespace DevilDaggersInfo.Web.Shared.Dto.Public.Authentication;

public record LoginRequest
{
	[Required]
	public string Name { get; set; } = null!;

	[Required]
	public string Password { get; set; } = null!;
}
