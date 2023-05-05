using ImGuiNET;

namespace DevilDaggersInfo.App.Utils;

public sealed class ImGuiIoState
{
	private readonly bool _isMouse;
	private readonly int _input;

	public ImGuiIoState(bool isMouse, int input)
	{
		_isMouse = isMouse;
		_input = input;
	}

	// TODO: Implement JustPressedOrHeld (or something).
	public bool Down { get; private set; }
	public bool JustPressed { get; private set; }
	public bool JustReleased { get; private set; }

	public void Update(ImGuiIOPtr io)
	{
		bool inputDownPrevious = Down;
		Down = _isMouse ? io.MouseDown[_input] : io.KeysDown[_input];
		JustPressed = Down && !inputDownPrevious;
		JustReleased = !Down && inputDownPrevious;
	}
}
