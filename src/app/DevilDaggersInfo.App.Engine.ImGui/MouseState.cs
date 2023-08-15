using Silk.NET.Input;
using System.Buffers;

namespace DevilDaggersInfo.App.Engine.ImGui;

internal readonly unsafe struct MouseState : IDisposable
{
	private readonly int _buttonCount;
	private readonly int _pressedButtonCount;
	private readonly int _scrollWheelCount;
	private readonly IMemoryOwner<byte> _buttons;
	private readonly IMemoryOwner<byte> _pressedButtons;
	private readonly IMemoryOwner<byte> _scrollWheels;

	internal MouseState(IMouse mouse, MemoryPool<byte> pool)
	{
		Index = mouse.Index;
		IsConnected = mouse.IsConnected;
		IReadOnlyList<MouseButton> supportedButtons1 = mouse.SupportedButtons;
		_buttonCount = supportedButtons1.Count;
		_buttons = pool.Rent(_buttonCount * 4);
		_pressedButtons = pool.Rent(_buttonCount * 4);
		_pressedButtonCount = _buttonCount;
		Span<MouseButton> supportedButtons2 = GetSupportedButtons();
		Span<MouseButton> pressedButtons1 = GetPressedButtons();
		_pressedButtonCount = 0;
		for (int index = 0; index < _buttonCount; ++index)
		{
			supportedButtons2[index] = supportedButtons1[index];
			if (mouse.IsButtonPressed(supportedButtons2[index]))
				pressedButtons1[_pressedButtonCount++] = supportedButtons2[index];
		}

		using (_pressedButtons)
		{
			Span<MouseButton> pressedButtons2 = GetPressedButtons();
			_pressedButtons = pool.Rent(_pressedButtonCount * 4);
			Span<MouseButton> pressedButtons3 = GetPressedButtons();
			pressedButtons2.CopyTo(pressedButtons3);
			IReadOnlyList<ScrollWheel> scrollWheels1 = mouse.ScrollWheels;
			_scrollWheelCount = scrollWheels1.Count;
			_scrollWheels = pool.Rent(_scrollWheelCount * sizeof(ScrollWheel));
			Span<ScrollWheel> scrollWheels2 = GetScrollWheels();
			for (int index = 0; index < _scrollWheelCount; ++index)
				scrollWheels2[index] = scrollWheels1[index];
			Position = mouse.Position;
		}
	}

	public int Index { get; }

	public bool IsConnected { get; }

	public Vector2 Position { get; }

	public Span<MouseButton> GetSupportedButtons() => new(Unsafe.AsPointer(ref _buttons.Memory.Span.GetPinnableReference()), _buttonCount);

	public Span<MouseButton> GetPressedButtons() => new(Unsafe.AsPointer(ref _pressedButtons.Memory.Span.GetPinnableReference()), _pressedButtonCount);

	public Span<ScrollWheel> GetScrollWheels() => new(Unsafe.AsPointer(ref _scrollWheels.Memory.Span.GetPinnableReference()), _scrollWheelCount);

	public bool IsButtonPressed(MouseButton btn)
	{
		Span<MouseButton> pressedButtons = GetPressedButtons();
		for (int index = 0; index < pressedButtons.Length; ++index)
		{
			if (pressedButtons[index] == btn)
				return true;
		}

		return false;
	}

	public void Dispose()
	{
		_buttons.Dispose();
		_pressedButtons.Dispose();
		_scrollWheels.Dispose();
	}
}
