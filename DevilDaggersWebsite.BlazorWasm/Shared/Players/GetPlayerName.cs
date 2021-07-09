namespace DevilDaggersWebsite.BlazorWasm.Shared.Players
{
	public class GetPlayerName : IGetDto<int>
	{
		public int Id { get; init; }

		public string? PlayerName { get; init; }
	}
}
