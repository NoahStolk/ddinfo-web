using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class PathsScrollArea : ScrollArea
{
	private const int _entryWidth = 1008;
	private const int _entryHeight = 16;

	private readonly List<Button> _subDirectoryButtons = new();

	public PathsScrollArea(IBounds bounds)
		: base(bounds, 96, 16, GlobalStyles.DefaultScrollAreaStyle)
	{
	}

	public string Path { get; set; } = string.Empty;

	public Action<string>? OnDirectorySelect { get; set; }
	public Action<string>? OnFileSelect { get; set; }

	public void SetContent()
	{
		SetComponentsFromPath(Path);
	}

	private void SetComponentsFromPath(string path)
	{
		foreach (Button button in _subDirectoryButtons)
			NestingContext.Remove(button);

		_subDirectoryButtons.Clear();

		DirectoryInfo? parent = Directory.GetParent(path);
		if (parent != null)
			_subDirectoryButtons.Add(new PathButton(Bounds.CreateNested(0, 0, _entryWidth, _entryHeight), () => OnDirectorySelect?.Invoke(parent.FullName), true, ".."));

		int i = 0;
		foreach (string directory in Directory.GetDirectories(path))
			_subDirectoryButtons.Add(new PathButton(Bounds.CreateNested(0, ++i * _entryHeight, _entryWidth, _entryHeight), () => OnDirectorySelect?.Invoke(directory), true, System.IO.Path.GetFileName(directory)));

		foreach (string file in Directory.GetFiles(path))
			_subDirectoryButtons.Add(new PathButton(Bounds.CreateNested(0, ++i * _entryHeight, _entryWidth, _entryHeight), () => OnFileSelect?.Invoke(file), false, System.IO.Path.GetFileName(file)));

		foreach (Button button in _subDirectoryButtons)
			NestingContext.Add(button);
	}
}
