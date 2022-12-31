using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;
using DevilDaggersInfo.App.Ui.Base.Styling;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;
using StateManager = DevilDaggersInfo.App.Ui.Base.StateManagement.StateManager;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditorSaveLayout : Layout, IExtendedLayout
{
	private readonly TextInput _pathTextInput;
	private readonly PathsScrollArea _pathsScrollArea;

	public SurvivalEditorSaveLayout()
	{
		PathsCloseButton closeButton = new(new PixelBounds(0, 0, 24, 24), () => StateManager.Dispatch(new SetLayout(Root.Game.SurvivalEditorMainLayout)));
		_pathTextInput = new(new PixelBounds(0, 24, 1024, 16), false, null, null, null, GlobalStyles.TextInput);
		TextInput fileTextInput = new(new PixelBounds(0, 48, 512, 16), false, null, null, null, GlobalStyles.TextInput);
		TextButton saveButton = new(new PixelBounds(512, 48, 128, 16), () => SaveSpawnset(Path.Combine(_pathTextInput.KeyboardInput.Value.ToString(), fileTextInput.KeyboardInput.Value.ToString())), GlobalStyles.DefaultButtonStyle, GlobalStyles.FileSaveButton, "Save");

		_pathsScrollArea = new(new PixelBounds(0, 96, 1024, 640))
		{
			OnDirectorySelect = SetComponentsFromPath,
			OnFileSelect = SaveSpawnset,
		};

		NestingContext.Add(closeButton);
		NestingContext.Add(_pathTextInput);
		NestingContext.Add(fileTextInput);
		NestingContext.Add(saveButton);
		NestingContext.Add(_pathsScrollArea);

		StateManager.Subscribe<SetLayout>(Initialize);
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

	private void Initialize()
	{
		if (StateManager.LayoutState.CurrentLayout != Root.Game.SurvivalEditorSaveLayout)
			return;

		SetComponentsFromPath(UserSettings.DevilDaggersInstallationDirectory);
	}

	private void SetComponentsFromPath(string path)
	{
		_pathTextInput.KeyboardInput.SetText(path);
		_pathsScrollArea.Path = path;
		_pathsScrollArea.SetContent();
	}

	private void SaveSpawnset(string filePath)
	{
		if (Directory.Exists(filePath))
		{
			Popup popup = new(this, "Specified file path is an existing directory.");
			NestingContext.Add(popup);
		}

		if (File.Exists(filePath))
		{
			// TODO: Ask to overwrite or cancel.
			Popup popup = new(this, "File already exists.");
			NestingContext.Add(popup);
		}

		File.WriteAllBytes(filePath, StateManager.SpawnsetState.Spawnset.ToBytes());
	}
}
