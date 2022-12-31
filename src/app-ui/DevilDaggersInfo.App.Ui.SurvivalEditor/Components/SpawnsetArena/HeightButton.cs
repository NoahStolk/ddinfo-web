using DevilDaggersInfo.App.Ui.Base.StateManagement;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.RenderImpl.Ui.Components.Styles;
using Warp.NET.RenderImpl.Ui.Rendering.Text;
using Warp.NET.Text;
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

	public override void Update(Vector2i<int> scrollOffset)
	{
		base.Update(scrollOffset);

		TextButtonStyle = Math.Abs(StateManager.ArenaEditorState.SelectedHeight - _height) < 0.001f ? new(Color.Blue, TextAlign.Middle, FontSize.H12) : _textButtonStyle;
	}
}
