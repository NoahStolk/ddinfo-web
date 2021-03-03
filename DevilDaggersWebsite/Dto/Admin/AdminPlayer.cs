using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminPlayer
	{
		public int Id { get; init; }
		public string PlayerName { get; init; }
		public bool IsAnonymous { get; init; }
		public List<int>? AssetModIds { get; init; }
		public List<int>? TitleIds { get; init; }
		public string? CountryCode { get; init; }
		public int? Dpi { get; init; }
		public float? InGameSens { get; init; }
		public int? Fov { get; init; }
		public bool? RightHanded { get; init; }
		public bool? FlashEnabled { get; init; }
		public float? Gamma { get; init; }
		public bool IsBanned { get; init; }
		public string? BanDescription { get; init; }
		public int? BanResponsibleId { get; init; }
	}
}
