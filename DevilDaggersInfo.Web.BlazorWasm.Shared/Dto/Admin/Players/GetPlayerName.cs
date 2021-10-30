namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Players;

public class GetPlayerName : IGetDto
{
	public int Id { get; init; }

	public string PlayerName { get; init; } = null!;
}
