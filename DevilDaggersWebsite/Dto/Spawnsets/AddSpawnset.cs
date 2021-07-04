using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Dto.Spawnsets
{
	public class AddSpawnset
	{
		[Required]
		public int PlayerId { get; init; }

		[StringLength(64)]
		public string Name { get; init; } = null!;

		public int? MaxDisplayWaves { get; init; }

		[StringLength(2048)]
		public string? HtmlDescription { get; init; }

		public bool IsPractice { get; init; }
	}
}
