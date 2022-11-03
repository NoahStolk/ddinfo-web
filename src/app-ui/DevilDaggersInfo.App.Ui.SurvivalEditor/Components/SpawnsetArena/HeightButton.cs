using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

// TODO: Move to SpawnsetComponentBuilder.
public class HeightButton : TextButton
{
	private readonly float _height;
	private readonly Color _heightColor;

	public HeightButton(Rectangle metric, Action onClick, float height)
		: base(metric, onClick, Color.White, Color.Black, Color.Black, Color.Black, string.Empty, TextAlign.Middle, 1, FontSize.F4X6)
	{
		_height = height;
		_heightColor = TileUtils.GetColorFromHeight(height);

		BackgroundColor = _heightColor;
		HoverBackgroundColor = Color.Lerp(_heightColor, Color.White, 0.75f);
		TextColor = BackgroundColor.ReadableColorForBrightness();
		Text = height.ToString(); // TODO: -1000 should probably be written as -1K.
		FontSize = Text.Length > 2 ? FontSize.F4X6 : FontSize.F8X8;
	}

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		BackgroundColor = Math.Abs(StateManager.ArenaEditorState.SelectedHeight - _height) < 0.001f ? Color.Blue : _heightColor;
	}
}
