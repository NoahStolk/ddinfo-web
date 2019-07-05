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
				VersionNumber = "2.1.1.0"
			},
			new Tool
			{
				Name = "DDCL",
				VersionNumber = "0.4.0.1",
				VersionNumberRequired = "0.4.0.1"
			}
		};
	}
}