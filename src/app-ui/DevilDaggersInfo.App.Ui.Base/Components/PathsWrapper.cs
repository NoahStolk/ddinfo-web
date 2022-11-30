using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class PathsWrapper : AbstractScrollViewer<PathsWrapper, Paths>
{
	public PathsWrapper(IBounds bounds, Action<string> onDirectorySelect, Action<string> onFileSelect)
		: base(bounds)
	{
		const int scrollbarWidth = 16;

		Content = new(new PixelBounds(0, 0, bounds.Size.X - scrollbarWidth, bounds.Size.Y), this, scrollbarWidth, onDirectorySelect, onFileSelect);
		Scrollbar = new(new PixelBounds(bounds.Size.X - scrollbarWidth, 0, scrollbarWidth, bounds.Size.Y), SetScrollPercentage);

		NestingContext.Add(Content);
		NestingContext.Add(Scrollbar);
	}

	public override Scrollbar Scrollbar { get; }
	public override Paths Content { get; }

	public string Path { get; set; } = string.Empty;

	public override void InitializeContent()
	{
		Content.SetComponentsFromPath(Path);

		SetThumbPercentageSize();
		SetScrollPercentage(0);
	}
}
