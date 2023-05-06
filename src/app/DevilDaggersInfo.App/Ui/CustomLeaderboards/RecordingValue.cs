using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.App.Engine.InterpolationStates;
using DevilDaggersInfo.App.Engine.Maths.Numerics;
using ImGuiNET;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboards;

public class RecordingValue
{
	private readonly string _labelText;
	private readonly FloatState _intensity = new(0);

	private string _value = string.Empty;
	private Color _color;
	private readonly Func<MainBlock, string> _valueGetter;
	private readonly Func<MainBlock, Color>? _colorGetter;

	public RecordingValue(string labelText, Color color, Func<MainBlock, string> valueGetter, Func<MainBlock, Color>? colorGetter)
	{
		_labelText = labelText;
		_color = color;
		_valueGetter = valueGetter;
		_colorGetter = colorGetter;
	}

	public void SetState()
	{
		string value = _valueGetter(Root.GameMemoryService.MainBlock);
		Color? color = _colorGetter?.Invoke(Root.GameMemoryService.MainBlock);
		UpdateValue(value, color);
	}

	private void UpdateValue(string value, Color? color = null)
	{
		if (_value == value)
			return;

		_value = value;
		if (color.HasValue)
			_color = color.Value;

		_intensity.Physics = 1;
	}

	public void Update(float delta)
	{
		_intensity.PrepareUpdate();

		_intensity.Physics = Math.Max(0, _intensity.Physics - delta);
	}

	public void Render()
	{
		_intensity.PrepareRender();

		ImGui.Text(_labelText);
		ImGui.SameLine(256 - ImGui.CalcTextSize(_value).X);
		ImGui.TextColored(Color.Lerp(Color.White, _color, _intensity.Render), _value);
	}
}
