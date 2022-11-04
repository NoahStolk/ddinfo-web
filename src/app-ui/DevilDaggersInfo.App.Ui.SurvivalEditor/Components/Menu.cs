using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components;

public class Menu : AbstractComponent
{
	public Menu(Rectangle metric)
		: base(metric)
	{
		const int backButtonWidth = 24;
		int rowHeight = metric.Size.Y;

		Depth = 100;

		MainLayoutBackButton backButton = new(Rectangle.At(0, 0, backButtonWidth, rowHeight), LayoutManager.ToMainLayout)
		{
			Depth = 101,
		};

		const int menuItemHeight = 16;
		List<AbstractComponent> fileMenuButtons = new()
		{
			new MenuButton(Rectangle.At(0, rowHeight + menuItemHeight * 0, 160, menuItemHeight), StateManager.NewSpawnset, "New"),
			new MenuButton(Rectangle.At(0, rowHeight + menuItemHeight * 1, 160, menuItemHeight), LayoutManager.ToSurvivalEditorOpenLayout, "Open"),
			new MenuButton(Rectangle.At(0, rowHeight + menuItemHeight * 2, 160, menuItemHeight), StateManager.OpenDefaultV3Spawnset, "Open default (V3)"),
			new MenuButton(Rectangle.At(0, rowHeight + menuItemHeight * 3, 160, menuItemHeight), LayoutManager.ToSurvivalEditorSaveLayout, "Save"),
		};
		Dropdown fileMenu = new(Rectangle.At(backButtonWidth, 0, 64, rowHeight + fileMenuButtons.Count * menuItemHeight), 24, fileMenuButtons, "File")
		{
			Depth = 101,
		};

		NestingContext.Add(backButton);
		NestingContext.Add(fileMenu);
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		RenderBatchCollector.RenderRectangleTopLeft(Metric.Size, parentPosition + new Vector2i<int>(Metric.X1, Metric.Y1), Depth, Color.Gray(0.05f));
	}
}
