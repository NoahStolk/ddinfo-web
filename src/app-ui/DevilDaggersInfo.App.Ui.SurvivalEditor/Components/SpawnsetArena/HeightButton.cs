using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using DevilDaggersInfo.App.Ui.SurvivalEditor.Utils;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

// TODO: Move to ComponentBuilder.
public class HeightButton : Button
{
	private readonly float _height;
	private readonly Color _heightColor;

	public HeightButton(Rectangle metric, Action onClick, float height)
		: base(metric, onClick, Color.White, Color.Black, Color.Black, Color.Black, string.Empty, TextAlign.Middle, 2, FontSize.F4X6)
	{
		_height = height;
		_heightColor = TileUtils.GetColorFromHeight(height);

		BackgroundColor = _heightColor;
		HoverBackgroundColor = _heightColor.Intensify(128); // TODO: Find a better way to do this.
		TextColor = BackgroundColor.ReadableColorForBrightness();
		Text = height.ToString();
		FontSize = Text.Length > 2 ? FontSize.F4X6 : FontSize.F8X8;
	}

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		BackgroundColor = Math.Abs(StateManager.ArenaEditorState.SelectedHeight - _height) < 0.001f ? Color.Blue : _heightColor;
	}
}
