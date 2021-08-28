namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities;

[Table("Titles")]
public class TitleEntity
{
	[Key]
	public int Id { get; init; }

	[StringLength(16)]
	public string Name { get; set; } = null!;

	public List<PlayerTitleEntity> PlayerTitles { get; set; } = new();
}
