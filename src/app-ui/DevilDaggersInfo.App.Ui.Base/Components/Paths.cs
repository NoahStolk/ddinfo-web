using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class Paths : ScrollContent<Paths, PathsWrapper>
{
	private const int _entryHeight = 16;

	private readonly int _componentWidth;

	private readonly Action<string> _onDirectorySelect;
	private readonly Action<string> _onFileSelect;

	private readonly List<Button> _subDirectoryButtons = new();

	public Paths(IBounds bounds, PathsWrapper parent, int scrollbarWidth, Action<string> onDirectorySelect, Action<string> onFileSelect)
		: base(bounds, parent)
	{
		_componentWidth = Constants.NativeWidth - scrollbarWidth;

		_onDirectorySelect = onDirectorySelect;
		_onFileSelect = onFileSelect;
	}

	public override int ContentHeightInPixels => _subDirectoryButtons.Count * _entryHeight;

	public void SetComponentsFromPath(string path)
	{
		foreach (Button button in _subDirectoryButtons)
			NestingContext.Remove(button);

		_subDirectoryButtons.Clear();

		DirectoryInfo? parent = Directory.GetParent(path);
		if (parent != null)
			_subDirectoryButtons.Add(new PathButton(new PixelBounds(0, 0, _componentWidth, _entryHeight), () => _onDirectorySelect(parent.FullName), true, ".."));

		int i = 0;
		foreach (string directory in Directory.GetDirectories(path))
			_subDirectoryButtons.Add(new PathButton(new PixelBounds(0, ++i * _entryHeight, _componentWidth, _entryHeight), () => _onDirectorySelect(directory), true, Path.GetFileName(directory)));

		foreach (string file in Directory.GetFiles(path))
			_subDirectoryButtons.Add(new PathButton(new PixelBounds(0, ++i * _entryHeight, _componentWidth, _entryHeight), () => _onFileSelect(file), false, Path.GetFileName(file)));

		foreach (Button button in _subDirectoryButtons)
			NestingContext.Add(button);
	}
}
