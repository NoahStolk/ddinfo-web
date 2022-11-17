using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.App.Ui.Base.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components;

public class Menu : AbstractComponent
{
	private const int _headerHeight = 24;

	public Menu(IBounds bounds)
		: base(bounds)
	{
		const int backButtonWidth = 24;

		Depth = 100;

		MainLayoutBackButton backButton = new(Rectangle.At(0, 0, backButtonWidth, _headerHeight), LayoutManager.ToMainLayout)
		{
			Depth = 101,
		};

		const int menuItemWidth = 160;
		const int menuItemHeight = 16;

		Dropdown fileMenu = new(Rectangle.At(backButtonWidth, 0, 64, _headerHeight), "File")
		{
			Depth = 101,
		};

		DropdownEntry entryNew = new(Rectangle.At(fileMenu.Bounds.X1, _headerHeight + menuItemHeight * 0, menuItemWidth, menuItemHeight), fileMenu, StateManager.NewSpawnset, "New");
		DropdownEntry entryOpen = new(Rectangle.At(fileMenu.Bounds.X1, _headerHeight + menuItemHeight * 1, menuItemWidth, menuItemHeight), fileMenu, LayoutManager.ToSurvivalEditorOpenLayout, "Open");
		DropdownEntry entryOpenDefault = new(Rectangle.At(fileMenu.Bounds.X1, _headerHeight + menuItemHeight * 2, menuItemWidth, menuItemHeight), fileMenu, StateManager.OpenDefaultV3Spawnset, "Open default (V3)");
		DropdownEntry entrySave = new(Rectangle.At(fileMenu.Bounds.X1, _headerHeight + menuItemHeight * 3, menuItemWidth, menuItemHeight), fileMenu, LayoutManager.ToSurvivalEditorSaveLayout, "Save");
		DropdownEntry entryReplace = new(Rectangle.At(fileMenu.Bounds.X1, _headerHeight + menuItemHeight * 4, menuItemWidth, menuItemHeight), fileMenu, StateManager.ReplaceSpawnset, "Replace");

		fileMenu.AddChild(entryNew);
		fileMenu.AddChild(entryOpen);
		fileMenu.AddChild(entryOpenDefault);
		fileMenu.AddChild(entrySave);
		fileMenu.AddChild(entryReplace);

		NestingContext.Add(entryNew);
		NestingContext.Add(entryOpen);
		NestingContext.Add(entryOpenDefault);
		NestingContext.Add(entrySave);
		NestingContext.Add(entryReplace);

		NestingContext.Add(backButton);
		NestingContext.Add(fileMenu);
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		RenderBatchCollector.RenderRectangleTopLeft(new(Bounds.Size.X, _headerHeight), parentPosition + new Vector2i<int>(Bounds.X1, Bounds.Y1), Depth, Color.Gray(0.05f));
	}
}
