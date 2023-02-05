using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Rendering.Text;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;
using DevilDaggersInfo.App.Ui.Base.Styling;
using Warp.NET.Text;
using Warp.NET.Ui;
using StateManager = DevilDaggersInfo.App.Ui.Base.StateManagement.StateManager;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditorSaveLayout : Layout, IExtendedLayout
{
	private readonly TextInput _pathTextInput;
	private readonly PathsScrollArea _pathsScrollArea;

	public SurvivalEditorSaveLayout()
	{
		PathsCloseButton closeButton = new(new PixelBounds(0, 0, 24, 24), () => StateManager.Dispatch(new SetLayout(Root.Dependencies.SurvivalEditorMainLayout)));
		_pathTextInput = new(new PixelBounds(0, 24, 1024, 16), false, null, null, null, TextInputStyles.Default);
		TextInput fileTextInput = new(new PixelBounds(0, 48, 512, 16), false, null, null, null, TextInputStyles.Default);
		TextButton saveButton = new(new PixelBounds(512, 48, 128, 16), () => SaveSpawnset(Path.Combine(_pathTextInput.KeyboardInput.Value.ToString(), fileTextInput.KeyboardInput.Value.ToString())), ButtonStyles.Default, new(Color.Green, TextAlign.Middle, FontSize.H12), "Save");

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
		if (StateManager.LayoutState.CurrentLayout != Root.Dependencies.SurvivalEditorSaveLayout)
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
			Root.Dependencies.NativeDialogService.ReportMessage("Specified file path is an existing directory", "Please specify a file path.");

		if (File.Exists(filePath))
		{
			// TODO: Ask to overwrite or cancel.
			Root.Dependencies.NativeDialogService.ReportMessage("File already exists", "Please specify a different file path.");
		}

		File.WriteAllBytes(filePath, StateManager.SpawnsetState.Spawnset.ToBytes());
	}
}
