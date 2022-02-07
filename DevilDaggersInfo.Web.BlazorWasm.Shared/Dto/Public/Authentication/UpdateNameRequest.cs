namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;

public class UpdateNameRequest
{
	public string CurrentName { get; set; } = null!;

	public string CurrentPassword { get; set; } = null!;

	public string NewName { get; set; } = null!;
}
