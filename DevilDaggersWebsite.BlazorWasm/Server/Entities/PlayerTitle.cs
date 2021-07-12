using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersWebsite.BlazorWasm.Server.Entities
{
	public class PlayerTitle
	{
		public int PlayerId { get; set; }

		[ForeignKey(nameof(PlayerId))]
		public Player Player { get; set; } = null!;

		public int TitleId { get; set; }

		[ForeignKey(nameof(TitleId))]
		public Title Title { get; set; } = null!;
	}
}
