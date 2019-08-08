using DevilDaggersCore.Tools;
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
				VersionNumber = "2.4.0.0"
			},
			new Tool
			{
				Name = "DDCL",
				VersionNumber = "0.4.2.0",
				VersionNumberRequired = "0.4.0.1"
			}
		};
	}
}