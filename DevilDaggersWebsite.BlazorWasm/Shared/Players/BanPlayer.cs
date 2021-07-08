using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Players
{
	public class BanPlayer
	{
		[StringLength(64)]
		public string? BanDescription { get; init; }

		[Range(1, 9999999)]
		public int? BanResponsibleId { get; init; }
	}
}
