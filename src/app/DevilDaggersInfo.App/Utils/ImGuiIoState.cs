using ImGuiNET;

namespace DevilDaggersInfo.App.Utils;

public sealed class ImGuiIoState
{
	private readonly bool _isMouse;
	private readonly int _input;

	private bool _down;
	private bool _justPressed;
	private bool _justReleased;

	public ImGuiIoState(bool isMouse, int input)
	{
		_isMouse = isMouse;
		_input = input;
	}

	public bool Down => _down;
	public bool JustPressed => _justPressed;
	public bool JustReleased => _justReleased;

	public void Update(ImGuiIOPtr io)
	{
		bool inputDownPrevious = _down;
		_down = _isMouse ? io.MouseDown[_input] : io.KeysDown[_input];
		_justPressed = _down && !inputDownPrevious;
		_justReleased = !_down && inputDownPrevious;
	}
}
