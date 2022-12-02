using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class ArenaToolsWrapper : AbstractComponent
{
	private const int _arenaButtonSize = 20;

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

		_toolSettingsWrappers.Add(ArenaTool.Bucket, new BucketToolSettingsWrapper(new PixelBounds(0, _arenaButtonSize, bounds.Size.X, 64)));

		foreach (KeyValuePair<ArenaTool, TooltipIconButton> kvp in _toolButtons)
			NestingContext.Add(kvp.Value);

		foreach (KeyValuePair<ArenaTool, AbstractComponent> kvp in _toolSettingsWrappers)
			NestingContext.Add(kvp.Value);

		UpdateActiveButtonAndSettings();

		void AddToolButton(int offsetX, int offsetY, ArenaTool arenaTool, Texture texture, string tooltipText)
		{
			void SetArenaTool()
			{
				StateManager.SetArenaTool(arenaTool);
				UpdateActiveButtonAndSettings();
			}

			TooltipIconButton button = new(Bounds.CreateNested(offsetX, offsetY, _arenaButtonSize, _arenaButtonSize), SetArenaTool, GlobalStyles.DefaultButtonStyle, texture, tooltipText);
			_toolButtons.Add(arenaTool, button);
		}
	}

	private void UpdateActiveButtonAndSettings()
	{
		foreach (KeyValuePair<ArenaTool, TooltipIconButton> kvp in _toolButtons)
			kvp.Value.ButtonStyle = StateManager.ArenaEditorState.ArenaTool == kvp.Key ? GlobalStyles.ActiveToolButtonStyle : GlobalStyles.DefaultButtonStyle;

		foreach (KeyValuePair<ArenaTool, AbstractComponent> kvp in _toolSettingsWrappers)
			kvp.Value.IsActive = StateManager.ArenaEditorState.ArenaTool == kvp.Key;
	}
}
