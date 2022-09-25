using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.Core.Spawnset;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditorOpenLayout : Layout, IFileDialogLayout
{
	private const int _entryHeight = 16;

	private readonly TextInput _pathTextInput;

	private readonly List<Button> _subDirectoryButtons = new(); // TODO: Implement scroll viewer instead.

	public SurvivalEditorOpenLayout()
		: base(Constants.Full)
	{
		Button backButton = new(Rectangle.At(0, 0, 24, 24), LayoutManager.ToSurvivalEditorMainLayout, Color.Black, Color.White, Color.White, Color.Red, "X", TextAlign.Left, 2, FontSize.F12X12);
		_pathTextInput = ComponentBuilder.CreateTextInput(Rectangle.At(0, 24, 1024, 16), false);

		NestingContext.Add(backButton);
		NestingContext.Add(_pathTextInput);
	}

	public void Update()
	{
	}

	public void Render3d()
	{
	}

	public void Render()
	{
	}

	public void RenderText()
	{
	}

	public void SetComponentsFromPath(string path)
	{
		Clear();
		_pathTextInput.SetText(path);

		int i = 2;
		DirectoryInfo? parent = Directory.GetParent(path);
		if (parent != null)
			_subDirectoryButtons.Add(new Button.PathButton(new(0, ++i * _entryHeight, 1024, i * _entryHeight + _entryHeight), () => SetComponentsFromPath(parent.FullName), "..", Color.Green));

		foreach (string directory in Directory.GetDirectories(path))
			_subDirectoryButtons.Add(new Button.PathButton(new(0, ++i * _entryHeight, 1024, i * _entryHeight + _entryHeight), () => SetComponentsFromPath(directory), Path.GetFileName(directory), Color.Yellow));

		foreach (string file in Directory.GetFiles(path))
			_subDirectoryButtons.Add(new Button.PathButton(new(0, ++i * _entryHeight, 1024, i * _entryHeight + _entryHeight), () => OpenSpawnset(file), Path.GetFileName(file), Color.White));

		foreach (Button button in _subDirectoryButtons)
			NestingContext.Add(button);
	}

	private static void OpenSpawnset(string filePath)
	{
		byte[] bytes = File.ReadAllBytes(filePath);
		if (SpawnsetBinary.TryParse(bytes, out SpawnsetBinary? spawnsetBinary))
		{
			StateManager.SetSpawnset(Path.GetFileName(filePath), spawnsetBinary);
			LayoutManager.ToSurvivalEditorMainLayout();
		}
		else
		{
			// TODO: Show popup.
		}
	}

	private void Clear()
	{
		foreach (Button button in _subDirectoryButtons)
			NestingContext.Remove(button);

		_subDirectoryButtons.Clear();
	}
}
