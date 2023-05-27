using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Core.CanvasChart.Components;

public partial class ChartHighlighter
{
	public bool IsVisible { get; set; }

	public double Width { get; set; }

	public double Top { get; set; }
	public double Left { get; set; }

	[Parameter]
	public required RenderFragment ChildContent { get; set; }

	public void ChangeState() => StateHasChanged();
}
