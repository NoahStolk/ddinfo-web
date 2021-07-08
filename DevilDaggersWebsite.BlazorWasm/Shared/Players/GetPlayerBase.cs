namespace DevilDaggersWebsite.BlazorWasm.Shared.Players
{
	public class GetPlayerBase : IGetDto
	{
		public int Id { get; init; }

		public string? PlayerName { get; init; }

		public string? CountryCode { get; init; }

		public int? Dpi { get; init; }

		public float? InGameSens { get; init; }

		public int? Fov { get; init; }

		public bool IsBanned { get; init; }
	}
}
