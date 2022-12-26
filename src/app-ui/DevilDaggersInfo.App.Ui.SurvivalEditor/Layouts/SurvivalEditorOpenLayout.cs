using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.Base.Settings;
using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;
using DevilDaggersInfo.Core.Spawnset;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;
using StateManager = DevilDaggersInfo.App.Ui.Base.StateManagement.StateManager;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditorOpenLayout : Layout, IExtendedLayout
{
	private readonly TextInput _pathTextInput;
	private readonly PathsScrollArea _pathsScrollArea;

	public SurvivalEditorOpenLayout()
	{
		PathsCloseButton closeButton = new(new PixelBounds(0, 0, 24, 24), () => StateManager.Dispatch(new SetLayout(Root.Game.SurvivalEditorMainLayout)));
		_pathTextInput = new(new PixelBounds(0, 24, 1024, 16), false, null, null, null, GlobalStyles.TextInput);
		_pathsScrollArea = new(new PixelBounds(0, 96, 1024, 640))
		{
			OnDirectorySelect = SetComponentsFromPath,
			OnFileSelect = OpenSpawnset,
		};

		NestingContext.Add(closeButton);
		NestingContext.Add(_pathTextInput);
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

	private void Initialize(SetLayout setLayout)
	{
		if (setLayout.Layout != Root.Game.SurvivalEditorOpenLayout)
			return;

		SetComponentsFromPath(UserSettings.DevilDaggersInstallationDirectory);
	}

	private void SetComponentsFromPath(string path)
	{
		_pathTextInput.KeyboardInput.SetText(path);
		_pathsScrollArea.Path = path;
		_pathsScrollArea.SetContent();
	}

	private void OpenSpawnset(string filePath)
	{
		byte[] bytes = File.ReadAllBytes(filePath);
		if (SpawnsetBinary.TryParse(bytes, out SpawnsetBinary? spawnsetBinary))
		{
			States.StateManager.SetSpawnset(Path.GetFileName(filePath), spawnsetBinary);
			StateManager.Dispatch(new SetLayout(Root.Game.SurvivalEditorMainLayout));
		}
		else
		{
			Popup popup = new(this, "File could not be parsed as a spawnset.");
			NestingContext.Add(popup);
		}
	}
}
