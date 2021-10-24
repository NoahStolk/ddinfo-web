using Blazorise.Charts;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Utils;

public static class LineChartUtils
{
	public static LineChartOptions Options { get; } = new()
	{
		MaintainAspectRatio = true,
		Responsive = true,
		AspectRatio = 3.5f,
		ShowLines = true,
	};

	public static string AdditionalJsonOptions { get; } = @"{
	""tooltips"": {
		""intersect"": false,
		""mode"": ""label""
	},
	""hover"": {
		""mode"": ""label""
	},
	""scales"": {
		""xAxes"": [{
			""display"": true,
			""scaleLabel"": {
				""show"": true
			}
		}]
	}
}";
}
