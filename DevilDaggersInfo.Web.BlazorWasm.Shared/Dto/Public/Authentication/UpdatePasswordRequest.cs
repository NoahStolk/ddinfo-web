namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;

public class UpdatePasswordRequest
{
	public string CurrentName { get; set; } = null!;

	public string CurrentPassword { get; set; } = null!;

	public string NewPassword { get; set; } = null!;
}
