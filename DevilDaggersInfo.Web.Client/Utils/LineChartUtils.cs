using DevilDaggersInfo.Web.BlazorWasm.Client.Core.CanvasChart.Options.LineChart;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Utils;

public static class LineChartUtils
{
	public static List<LineChartBackground> GameVersionBackgrounds { get; } = new()
	{
		new() { Color = "#8002", ChartEndXValue = GameVersions.GetReleaseDate(GameVersion.V1_0).Ticks },
		new() { Color = "#0482", ChartEndXValue = GameVersions.GetReleaseDate(GameVersion.V2_0).Ticks },
		new() { Color = "#4802", ChartEndXValue = GameVersions.GetReleaseDate(GameVersion.V3_0).Ticks },
		new() { Color = "#8082", ChartEndXValue = GameVersions.GetReleaseDate(GameVersion.V3_1).Ticks },
		new() { Color = "#90c2", ChartEndXValue = GameVersions.GetReleaseDate(GameVersion.V3_2).Ticks },
		new() { Color = "#a0f2", ChartEndXValue = DateTime.UtcNow.Ticks },
	};
}
