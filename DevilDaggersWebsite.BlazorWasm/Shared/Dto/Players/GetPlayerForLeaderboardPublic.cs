using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Players
{
	public class GetPlayerForLeaderboardPublic : IGetDto<int>
	{
		public int Id { get; init; }

		public bool IsBanned { get; init; }

		public string? BanDescription { get; init; }

		public List<string> Titles { get; init; } = null!;

		public bool IsPublicDonator { get; init; }

		public string? CountryCode { get; init; }
	}
}
