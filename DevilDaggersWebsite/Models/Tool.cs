using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;

namespace DevilDaggersWebsite.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Tool
	{
		[JsonProperty]
		public string Name { get; set; }
		public string DisplayName { get; set; }
		public HtmlString Description { get; set; }
		public string Link { get; set; }
		public string LinkText { get; set; }
		[JsonProperty]
		public string VersionNumber { get; set; }
	}
}