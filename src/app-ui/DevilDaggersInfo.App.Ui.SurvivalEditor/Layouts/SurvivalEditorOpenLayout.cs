using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.Core.Spawnset;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditorOpenLayout : Layout, IFileDialogLayout
{
	private readonly TextInput _pathTextInput;
	private readonly ScrollViewer<Paths> _pathsWrapper;

	public SurvivalEditorOpenLayout()
	{
		PathsCloseButton closeButton = new(new PixelBounds(0, 0, 24, 24), LayoutManager.ToSurvivalEditorMainLayout);
		_pathTextInput = new(new PixelBounds(0, 24, 1024, 16), false, null, null, null, GlobalStyles.TextInput);
		PixelBounds pathsWrapperBounds = new(0, 96, 1024, 640);
		_pathsWrapper = new(pathsWrapperBounds, pathsWrapperBounds.CreateNested(0, 0, 1008, 640), pathsWrapperBounds.CreateNested(1008, 0, 16, 640))
		{
			Content =
			{
				OnDirectorySelect = SetComponentsFromPath,
				OnFileSelect = OpenSpawnset,
			},
		};

		NestingContext.Add(closeButton);
		NestingContext.Add(_pathTextInput);
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
		_pathsWrapper.Content.Path = path;
		_pathsWrapper.InitializeContent();
	}

	private void OpenSpawnset(string filePath)
	{
		byte[] bytes = File.ReadAllBytes(filePath);
		if (SpawnsetBinary.TryParse(bytes, out SpawnsetBinary? spawnsetBinary))
		{
			StateManager.SetSpawnset(Path.GetFileName(filePath), spawnsetBinary);
			LayoutManager.ToSurvivalEditorMainLayout();
		}
		else
		{
			Popup popup = new(this, "File could not be parsed as a spawnset.");
			NestingContext.Add(popup);
		}
	}
}
