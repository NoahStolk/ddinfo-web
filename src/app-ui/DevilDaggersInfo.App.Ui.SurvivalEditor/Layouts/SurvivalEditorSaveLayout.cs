using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditorSaveLayout : Layout, IFileDialogLayout
{
	private const int _entryHeight = 16;

	private readonly TextInput _pathTextInput;

	private readonly List<Button> _subDirectoryButtons = new(); // TODO: Implement scroll viewer instead.

	public SurvivalEditorSaveLayout()
		: base(Constants.Full)
	{
		Button backButton = new(Rectangle.At(0, 0, 24, 24), LayoutManager.ToSurvivalEditorMainLayout, Color.Black, Color.White, Color.White, Color.Red, "X", TextAlign.Left, 2, FontSize.F12X12);
		_pathTextInput = new(Rectangle.At(0, 24, 1024, 16), false, Color.Black, Color.Gray(0.75f), Color.Gray(0.25f), Color.White, Color.White, Color.Green, Color.Gray(0.5f), 2, FontSize.F8X8);
		TextInput fileTextInput = new(Rectangle.At(0, 48, 512, 16), false, Color.Black, Color.Gray(0.75f), Color.Gray(0.25f), Color.White, Color.White, Color.Green, Color.Gray(0.5f), 2, FontSize.F8X8);
		Button saveButton = new(Rectangle.At(512, 48, 128, 16), () => SaveSpawnset(Path.Combine(_pathTextInput.Value.ToString(), fileTextInput.Value.ToString())), Color.Black, Color.White, Color.White, Color.Green, "Save", TextAlign.Middle, 2, FontSize.F8X8);

		NestingContext.Add(backButton);
		NestingContext.Add(_pathTextInput);
		NestingContext.Add(fileTextInput);
		NestingContext.Add(saveButton);
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

		int i = 4;
		DirectoryInfo? parent = Directory.GetParent(path);
		if (parent != null)
			_subDirectoryButtons.Add(new Button.PathButton(Rectangle.At(0, ++i * _entryHeight, 1024, _entryHeight), () => SetComponentsFromPath(parent.FullName), "..", Color.Green));

		foreach (string directory in Directory.GetDirectories(path))
			_subDirectoryButtons.Add(new Button.PathButton(Rectangle.At(0, ++i * _entryHeight, 1024, _entryHeight), () => SetComponentsFromPath(directory), Path.GetFileName(directory), Color.Yellow));

		foreach (string file in Directory.GetFiles(path))
			_subDirectoryButtons.Add(new Button.PathButton(Rectangle.At(0, ++i * _entryHeight, 1024, _entryHeight), () => {}, Path.GetFileName(file), Color.White));

		foreach (Button button in _subDirectoryButtons)
			NestingContext.Add(button);
	}

	private static void SaveSpawnset(string filePath)
	{
		if (Directory.Exists(filePath))
			return; // TODO: Show error.

		if (File.Exists(filePath))
			return; // TODO: Ask to overwrite or cancel.

		File.WriteAllBytes(filePath, StateManager.SpawnsetState.Spawnset.ToBytes());
	}

	private void Clear()
	{
		foreach (Button button in _subDirectoryButtons)
			NestingContext.Remove(button);

		_subDirectoryButtons.Clear();
	}
}
