using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.Core.CanvasChart.Components;

public partial class ChartHighlighter
{
	public bool IsVisible { get; set; }

	public double Width { get; set; }

	public double Top { get; set; }
	public double Left { get; set; }

	[Parameter, EditorRequired] public RenderFragment ChildContent { get; set; } = null!;

	public void ChangeState() => StateHasChanged();
}
