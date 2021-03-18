namespace DevilDaggersWebsite.Dto.Admin
{
	public class AdminSpawnsetFile
	{
		public string Name { get; init; } = null!;
		public int PlayerId { get; init; }
		public int? MaxDisplayWaves { get; init; }
		public string? HtmlDescription { get; init; }
		public bool IsPractice { get; init; }
	}
}
