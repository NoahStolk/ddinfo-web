using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Components.Styles;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.Base.Rendering.Text;
using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Data;
using DevilDaggersInfo.App.Ui.Base.Styling;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class ArenaToolsWrapper : AbstractComponent
{
	private const int _arenaButtonSize = 20;

	private static readonly ButtonStyle _activeToolButtonStyle = new(Color.Blue, Color.White, Color.Blue, 1);

	private readonly Dictionary<ArenaTool, TooltipIconButton> _toolButtons = new();
	private readonly Dictionary<ArenaTool, AbstractComponent> _toolSettingsWrappers = new();

	public ArenaToolsWrapper(IBounds bounds)
		: base(bounds)
	{
		AddToolButton(0 + _arenaButtonSize * 0, 0, ArenaTool.Pencil, WarpTextures.Pencil, "Pencil");
		AddToolButton(0 + _arenaButtonSize * 1, 0, ArenaTool.Line, WarpTextures.Line, "Line");
		AddToolButton(0 + _arenaButtonSize * 2, 0, ArenaTool.Rectangle, WarpTextures.Rectangle, "Rectangle");
		AddToolButton(0 + _arenaButtonSize * 3, 0, ArenaTool.Bucket, WarpTextures.Bucket, "Bucket");
		AddToolButton(0 + _arenaButtonSize * 4, 0, ArenaTool.Dagger, WarpTextures.Dagger, "Race dagger");

		_toolSettingsWrappers.Add(ArenaTool.Bucket, new BucketToolSettingsWrapper(bounds.CreateNested(0, _arenaButtonSize, bounds.Size.X, 64)));

		foreach (KeyValuePair<ArenaTool, TooltipIconButton> kvp in _toolButtons)
			NestingContext.Add(kvp.Value);

		foreach (KeyValuePair<ArenaTool, AbstractComponent> kvp in _toolSettingsWrappers)
			NestingContext.Add(kvp.Value);

		TextButton button3d = new(bounds.CreateNested(bounds.Size.X - 96 + 2, 0, 96, 20), () => StateManager.Dispatch(new SetLayout(Root.Dependencies.SurvivalEditor3dLayout)), ButtonStyles.Default, new(Color.White, TextAlign.Middle, FontSize.H12), "Open 3D editor");
		NestingContext.Add(button3d);

		UpdateActiveButtonAndSettings();

		StateManager.Subscribe<SetArenaTool>(UpdateActiveButtonAndSettings);

		void AddToolButton(int offsetX, int offsetY, ArenaTool arenaTool, Texture texture, string tooltipText)
		{
			TooltipIconButton button = new(Bounds.CreateNested(offsetX, offsetY, _arenaButtonSize, _arenaButtonSize), () => StateManager.Dispatch(new SetArenaTool(arenaTool)), ButtonStyles.Default, texture, tooltipText, Color.HalfTransparentWhite, Color.White);
			_toolButtons.Add(arenaTool, button);
		}
	}

	private void UpdateActiveButtonAndSettings()
	{
		foreach (KeyValuePair<ArenaTool, TooltipIconButton> kvp in _toolButtons)
			kvp.Value.ButtonStyle = StateManager.ArenaEditorState.ArenaTool == kvp.Key ? _activeToolButtonStyle : ButtonStyles.Default;

		foreach (KeyValuePair<ArenaTool, AbstractComponent> kvp in _toolSettingsWrappers)
			kvp.Value.IsActive = StateManager.ArenaEditorState.ArenaTool == kvp.Key;
	}
}
