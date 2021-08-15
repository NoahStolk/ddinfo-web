using System.ComponentModel.DataAnnotations.Schema;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities;

[Table("PlayerTitles")]
public class PlayerTitleEntity
{
	public int PlayerId { get; set; }

	[ForeignKey(nameof(PlayerId))]
	public PlayerEntity Player { get; set; } = null!;

	public int TitleId { get; set; }

	[ForeignKey(nameof(TitleId))]
	public TitleEntity Title { get; set; } = null!;
}
