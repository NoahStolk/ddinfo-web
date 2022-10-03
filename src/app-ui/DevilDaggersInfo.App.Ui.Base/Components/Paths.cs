using Warp.Numerics;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.Base.Components;

public class Paths : ScrollContent<Paths, PathsWrapper>
{
	private const int _entryHeight = 16;

	private readonly int _componentWidth;

	private readonly Action<string> _onDirectorySelect;
	private readonly Action<string> _onFileSelect;

	private readonly List<Button> _subDirectoryButtons = new();

	public Paths(Rectangle metric, PathsWrapper parent, int scrollbarWidth, Action<string> onDirectorySelect, Action<string> onFileSelect)
		: base(metric, parent)
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

		NestingContext.ScrollOffset = default;

		DirectoryInfo? parent = Directory.GetParent(path);
		if (parent != null)
			_subDirectoryButtons.Add(new Button.PathButton(new(0, 0, _componentWidth, _entryHeight), () => _onDirectorySelect(parent.FullName), "..", Color.Green));

		int i = 0;
		foreach (string directory in Directory.GetDirectories(path))
			_subDirectoryButtons.Add(new Button.PathButton(new(0, ++i * _entryHeight, _componentWidth, i * _entryHeight + _entryHeight), () => _onDirectorySelect(directory), Path.GetFileName(directory), Color.Yellow));

		foreach (string file in Directory.GetFiles(path))
			_subDirectoryButtons.Add(new Button.PathButton(new(0, ++i * _entryHeight, _componentWidth, i * _entryHeight + _entryHeight), () => _onFileSelect(file), Path.GetFileName(file), Color.White));

		foreach (Button button in _subDirectoryButtons)
			NestingContext.Add(button);
	}
}
