using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.Core.Spawnset;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditorOpenLayout : Layout, IExtendedLayout
{
	private readonly Button _backButton;
	private readonly TextInput _pathTextInput;

	private readonly List<Button> _subDirectoryButtons = new(); // TODO: Implement scroll viewer instead.

	private string _path = @"C:\Program Files (x86)\Steam\steamapps\common\devildaggers";

	public SurvivalEditorOpenLayout()
		: base(new(0, 0, 1920, 1080))
	{
		_backButton = new(Rectangle.At(0, 0, 32, 32), () => Root.Game.ActiveLayout = Root.Game.SurvivalEditorMainLayout, Color.Black, Color.White, Color.White, Color.Red, "X", TextAlign.Left, 2, false);
		_pathTextInput = new(Rectangle.At(0, 32, 1024, 32), false, Color.Black, Color.Gray(0.75f), Color.Gray(0.25f), Color.White, Color.White, Color.Green, Color.Gray(0.5f), 8, 2);

		NestingContext.Add(_backButton);
		NestingContext.Add(_pathTextInput);
		SetComponentsFromPath(_path);
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

	private void SetComponentsFromPath(string path)
	{
		Clear();
		_pathTextInput.SetText(path);
		_path = path;

		int i = 1;
		DirectoryInfo? parent = Directory.GetParent(path);
		if (parent != null)
			_subDirectoryButtons.Add(new Button.PathButton(new(0, ++i * 32, 1024, i * 32 + 32), () => SetComponentsFromPath(parent.FullName), "..", Color.Green));

		foreach (string directory in Directory.GetDirectories(_path))
			_subDirectoryButtons.Add(new Button.PathButton(new(0, ++i * 32, 1024, i * 32 + 32), () => SetComponentsFromPath(directory), Path.GetFileName(directory), Color.Yellow));

		foreach (string file in Directory.GetFiles(_path))
			_subDirectoryButtons.Add(new Button.PathButton(new(0, ++i * 32, 1024, i * 32 + 32), () => OpenSpawnset(file), Path.GetFileName(file), Color.White));

		foreach (Button button in _subDirectoryButtons)
			NestingContext.Add(button);
	}

	private static void OpenSpawnset(string filePath)
	{
		byte[] bytes = File.ReadAllBytes(filePath);
		if (SpawnsetBinary.TryParse(bytes, out SpawnsetBinary? spawnsetBinary))
		{
			StateManager.SetSpawnset(Path.GetFileName(filePath), spawnsetBinary);
			Root.Game.ActiveLayout = Root.Game.SurvivalEditorMainLayout;
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
