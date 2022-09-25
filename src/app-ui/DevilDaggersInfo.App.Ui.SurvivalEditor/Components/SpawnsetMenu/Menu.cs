using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.Core.Spawnset;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetMenu;

public class Menu : AbstractComponent
{
	public Menu(Rectangle metric)
		: base(metric)
	{
		Depth = 100;
		List<AbstractComponent> fileMenuButtons = new()
		{
			new Button.MenuButton(Rectangle.At(0, 16, 64, 16), () => StateManager.SetSpawnset("(untitled)", SpawnsetBinary.CreateDefault()), "New"),
			new Button.MenuButton(Rectangle.At(0, 32, 64, 16), LayoutManager.ToSurvivalEditorOpenLayout, "Open"),
			new Button.MenuButton(Rectangle.At(0, 48, 64, 16), LayoutManager.ToSurvivalEditorSaveLayout, "Save"),
			new Button.MenuButton(Rectangle.At(0, 64, 64, 16), LayoutManager.ToMainLayout, "Exit"),
		};

		Dropdown fileMenu = new(new(0, 0, 64, 80), fileMenuButtons, Color.White, "File")
		{
			Depth = 101,
		};

		NestingContext.Add(fileMenu);
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		Root.Game.UiRenderer.RenderTopLeft(Metric.Size, parentPosition + new Vector2i<int>(Metric.X1, Metric.Y1), Depth, Color.Gray(0.05f));
	}
}
