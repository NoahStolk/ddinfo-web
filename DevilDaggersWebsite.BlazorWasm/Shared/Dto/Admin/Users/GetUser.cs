using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.Users
{
	public class GetUser : IGetDto<string>
	{
		public string Id { get; init; } = null!;

		public string? UserName { get; init; }

		[Display(Name = "Admin")]
		public bool IsAdmin { get; init; }

		[Display(Name = "Custom LBs")]
		public bool IsCustomLeaderboardsMaintainer { get; init; }

		[Display(Name = "Donations")]
		public bool IsDonationsMaintainer { get; init; }

		[Display(Name = "Mods")]
		public bool IsModsMaintainer { get; init; }

		[Display(Name = "Players")]
		public bool IsPlayersMaintainer { get; init; }

		[Display(Name = "Spawnsets")]
		public bool IsSpawnsetsMaintainer { get; init; }
	}
}
