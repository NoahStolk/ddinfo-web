using DevilDaggersWebsite.Code.Tools;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.Utils
{
	public static class ToolUtils
	{
		public static List<Tool> Tools { get; set; } = new List<Tool>
		{
			new Tool
			{
				Name = "DevilDaggersSurvivalEditor",
				VersionNumber = "1.1.5.0"
			},
			new Tool
			{
				Name = "DDCL",
				VersionNumber = "0.2.5.0",
				VersionNumberRequired = "0.2.1.0"
			}
		};
	}
}