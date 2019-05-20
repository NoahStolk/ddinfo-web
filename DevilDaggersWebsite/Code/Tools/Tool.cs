using Newtonsoft.Json;

namespace DevilDaggersWebsite.Code.Tools
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Tool
	{
		[JsonProperty]
		public string Name { get; set; }
		[JsonProperty]
		public string VersionNumber { get; set; }
		[JsonProperty]
		public string VersionNumberRequired { get; set; }
	}
}