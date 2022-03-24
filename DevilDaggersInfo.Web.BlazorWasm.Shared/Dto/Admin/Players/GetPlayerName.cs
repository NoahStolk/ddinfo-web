namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Players;

public record GetPlayerName
{
	public int Id { get; init; }

	public string PlayerName { get; init; } = null!;
}
