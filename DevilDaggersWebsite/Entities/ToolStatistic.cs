using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Entities
{
	public class ToolStatistic
	{
		[Key]
		public int Id { get; set; }
		public string ToolName { get; set; } = null!;
		public string VersionNumber { get; set; } = null!;
		public int DownloadCount { get; set; }
	}
}
