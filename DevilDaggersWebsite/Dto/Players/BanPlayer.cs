using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Dto.Players
{
	public class BanPlayer
	{
		[StringLength(64)]
		public string? BanDescription { get; init; }

		public int? BanResponsibleId { get; init; }
	}
}
