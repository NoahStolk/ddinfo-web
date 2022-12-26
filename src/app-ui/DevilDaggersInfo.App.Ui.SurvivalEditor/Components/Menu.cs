using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;
using StateManager = DevilDaggersInfo.App.Ui.Base.StateManagement.StateManager;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components;

public class Menu : AbstractComponent
{
	private const int _headerHeight = 24;

	public Menu(IBounds bounds)
		: base(bounds)
	{
		const int backButtonWidth = 24;

		Depth = 100;

		MainLayoutBackButton backButton = new(new PixelBounds(0, 0, backButtonWidth, _headerHeight), () => StateManager.Dispatch(new SetLayout(Root.Game.MainLayout)))
		{
			Depth = 101,
		};

		const int menuItemWidth = 160;
		const int menuItemHeight = 16;

		Dropdown fileMenu = new(new PixelBounds(backButtonWidth, 0, 64, _headerHeight), "File", GlobalStyles.DefaultDropdownStyle)
		{
			Depth = 101,
		};

		const int dropdownEntryDepth = 102;
		DropdownEntry entryNew = new(new PixelBounds(fileMenu.Bounds.X1, _headerHeight + menuItemHeight * 0, menuItemWidth, menuItemHeight), fileMenu, States.StateManager.NewSpawnset, "New", GlobalStyles.DefaultDropdownEntryStyle) { Depth = dropdownEntryDepth };
		DropdownEntry entryOpen = new(new PixelBounds(fileMenu.Bounds.X1, _headerHeight + menuItemHeight * 1, menuItemWidth, menuItemHeight), fileMenu, () => StateManager.Dispatch(new SetLayout(Root.Game.SurvivalEditorOpenLayout)), "Open", GlobalStyles.DefaultDropdownEntryStyle) { Depth = dropdownEntryDepth };
		DropdownEntry entryOpenDefault = new(new PixelBounds(fileMenu.Bounds.X1, _headerHeight + menuItemHeight * 2, menuItemWidth, menuItemHeight), fileMenu, States.StateManager.OpenDefaultV3Spawnset, "Open default (V3)", GlobalStyles.DefaultDropdownEntryStyle) { Depth = dropdownEntryDepth };
		DropdownEntry entrySave = new(new PixelBounds(fileMenu.Bounds.X1, _headerHeight + menuItemHeight * 3, menuItemWidth, menuItemHeight), fileMenu, () => StateManager.Dispatch(new SetLayout(Root.Game.SurvivalEditorSaveLayout)), "Save", GlobalStyles.DefaultDropdownEntryStyle) { Depth = dropdownEntryDepth };
		DropdownEntry entryReplace = new(new PixelBounds(fileMenu.Bounds.X1, _headerHeight + menuItemHeight * 4, menuItemWidth, menuItemHeight), fileMenu, States.StateManager.ReplaceSpawnset, "Replace", GlobalStyles.DefaultDropdownEntryStyle) { Depth = dropdownEntryDepth };

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

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		Root.Game.RectangleRenderer.Schedule(new(Bounds.Size.X, _headerHeight), scrollOffset + Bounds.TopLeft + new Vector2i<int>(Bounds.Size.X, _headerHeight) / 2, Depth, Color.Gray(0.05f));
	}
}
