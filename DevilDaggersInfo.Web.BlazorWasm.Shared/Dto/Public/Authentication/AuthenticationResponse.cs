namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;

public class AuthenticationResponse
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;

	public List<string> RoleNames { get; init; } = new();
}
