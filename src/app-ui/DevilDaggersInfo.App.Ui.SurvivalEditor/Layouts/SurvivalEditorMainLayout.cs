using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern.Inversion.Layouts.SurvivalEditor;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetHistory;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetMenu;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Silk.NET.GLFW;
using Warp;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Layouts;

public class SurvivalEditorMainLayout : Layout, ISurvivalEditorMainLayout
{
	private readonly SpawnsWrapper _spawnsWrapper;
	private readonly HistoryWrapper _historyWrapper;

	public SurvivalEditorMainLayout()
		: base(Constants.Full)
	{
		Menu menu = new(new(0, 0, 1024, 16));
		ArenaWrapper arenaWrapper = new(Rectangle.At(400, 64, 400, 512));
		_spawnsWrapper = new(Rectangle.At(0, 64, 384, 512));
		_historyWrapper = new(Rectangle.At(768, 512, 256, 256));

		NestingContext.Add(menu);
		NestingContext.Add(arenaWrapper);
		NestingContext.Add(_spawnsWrapper);
		NestingContext.Add(_historyWrapper);

		// TODO: Move to own wrapper.
		AddSetting("Addit. gems", 800, 64);
		AddSetting("Timer start", 800, 80);

		void AddSetting(string labelText, int x, int y)
		{
			const int width = 112;
			const int height = 16;
			Label label = new(Rectangle.At(x, y, width, height), Color.White, labelText, TextAlign.Left, FontSize.F8X8);
			TextInput textInput = ComponentBuilder.CreateTextInput(Rectangle.At(x + width, y, width, height), true);
			NestingContext.Add(label);
			NestingContext.Add(textInput);
		}
	}

	public void SetSpawnset()
	{
		_spawnsWrapper.InitializeContent();
	}

	public void SetHistory()
	{
		_historyWrapper.InitializeContent();
	}

	public void Update()
	{
		if (!Input.IsKeyHeld(Keys.ControlLeft) && !Input.IsKeyHeld(Keys.ControlRight))
			return;

		if (Input.IsKeyPressed(Keys.Z))
			SpawnsetHistoryManager.Undo();
		else if (Input.IsKeyPressed(Keys.Y))
			SpawnsetHistoryManager.Redo();
		else if (Input.IsKeyPressed(Keys.N))
			StateManager.NewSpawnset();
		else if (Input.IsKeyPressed(Keys.O))
			LayoutManager.ToSurvivalEditorOpenLayout();
		else if (Input.IsKeyPressed(Keys.S))
			LayoutManager.ToSurvivalEditorSaveLayout();
	}

	public void Render3d()
	{
	}

	public void Render()
	{
		Root.Game.UiRenderer.RenderTopLeft(new(WindowWidth, WindowHeight), default, -100, new(0.1f));
	}

	public void RenderText()
	{
	}
}
