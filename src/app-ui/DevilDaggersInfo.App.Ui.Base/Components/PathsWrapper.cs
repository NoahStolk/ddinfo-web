using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class PathsWrapper : AbstractScrollViewer<PathsWrapper, Paths>
{
	public PathsWrapper(Rectangle metric, Action<string> onDirectorySelect, Action<string> onFileSelect)
		: base(metric)
	{
		const int scrollbarWidth = 16;

		Rectangle pathsMetric = Rectangle.At(0, 0, metric.Size.X - scrollbarWidth, metric.Size.Y);
		Content = new(pathsMetric, this, scrollbarWidth, onDirectorySelect, onFileSelect);
		Scrollbar = new(pathsMetric with { X1 = pathsMetric.X2, X2 = pathsMetric.X2 + scrollbarWidth }, ScrollbarOnChange);

		NestingContext.Add(Content);
		NestingContext.Add(Scrollbar);
	}

	protected override Scrollbar Scrollbar { get; }
	protected override Paths Content { get; }

	public string Path { get; set; } = string.Empty;

	public override void InitializeContent()
	{
		Content.SetComponentsFromPath(Path);

		base.InitializeContent();
	}
}
