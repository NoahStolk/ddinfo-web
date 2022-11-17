using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Components.Styles;
using DevilDaggersInfo.App.Ui.SurvivalEditor.States;
using Warp.NET.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena;

public class HeightButton : TextButton
{
	private readonly TextButtonStyle _textButtonStyle;
	private readonly float _height;

	public HeightButton(IBounds bounds, Action onClick, ButtonStyle buttonStyle, TextButtonStyle textButtonStyle, string text, float height)
		: base(bounds, onClick, buttonStyle, textButtonStyle, text)
	{
		_textButtonStyle = textButtonStyle;
		_height = height;
	}

	public override void Update(Vector2i<int> parentPosition)
	{
		base.Update(parentPosition);

		TextButtonStyle = Math.Abs(StateManager.ArenaEditorState.SelectedHeight - _height) < 0.001f ? GlobalStyles.SelectedHeightButton : _textButtonStyle;
	}
}
