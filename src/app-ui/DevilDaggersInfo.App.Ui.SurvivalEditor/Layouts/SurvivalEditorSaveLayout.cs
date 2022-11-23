using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditorSaveLayout : Layout, IFileDialogLayout
{
	private readonly TextInput _pathTextInput;
	private readonly PathsWrapper _pathsWrapper;

	public SurvivalEditorSaveLayout()
		: base(Constants.Full)
	{
		PathsCloseButton closeButton = new(Rectangle.At(0, 0, 24, 24), LayoutManager.ToSurvivalEditorMainLayout);
		_pathTextInput = new(Rectangle.At(0, 24, 1024, 16), false, null, null, null, GlobalStyles.TextInput);
		TextInput fileTextInput = new(Rectangle.At(0, 48, 512, 16), false, null, null, null, GlobalStyles.TextInput);
		TextButton saveButton = new(Rectangle.At(512, 48, 128, 16), () => SaveSpawnset(Path.Combine(_pathTextInput.KeyboardInput.Value.ToString(), fileTextInput.KeyboardInput.Value.ToString())), GlobalStyles.DefaultButtonStyle, GlobalStyles.FileSaveButton, "Save");
		_pathsWrapper = new(Rectangle.At(0, 96, 1024, 640), SetComponentsFromPath, SaveSpawnset);

		NestingContext.Add(closeButton);
		NestingContext.Add(_pathTextInput);
		NestingContext.Add(fileTextInput);
		NestingContext.Add(saveButton);
		NestingContext.Add(_pathsWrapper);
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

	public void SetComponentsFromPath(string path)
	{
		_pathTextInput.KeyboardInput.SetText(path);
		_pathsWrapper.Path = path;
		_pathsWrapper.InitializeContent();
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
