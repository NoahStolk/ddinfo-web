using Microsoft.AspNetCore.Html;

namespace DevilDaggersWebsite.Models
{
	public class Tool
	{
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public HtmlString Description { get; set; }
		public string Link { get; set; }
		public string LinkText { get; set; }
		public string VersionNumber { get; set; }
	}
}