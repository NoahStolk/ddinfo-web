using System.ComponentModel.DataAnnotations;

namespace DevilDaggersWebsite.Entities
{
	public class ToolStatistic
	{
		[Key]
		public int Id { get; set; }
		public string ToolName { get; set; }
		public string VersionNumber { get; set; }
		public int DownloadCount { get; set; }
	}
}
