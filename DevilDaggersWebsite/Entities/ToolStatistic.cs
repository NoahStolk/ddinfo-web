using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Entities
{
	public class ToolStatistic : IEntity
	{
		[Key]
		public int Id { get; set; }

		[StringLength(64)]
		public string ToolName { get; set; } = null!;

		[StringLength(16)]
		public string VersionNumber { get; set; } = null!;

		public int DownloadCount { get; set; }
	}
}
