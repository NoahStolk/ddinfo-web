using DevilDaggersInfo.App.Ui.ReplayEditor.Components;
using DevilDaggersInfo.Core.Replay.PostProcessing.ReplaySimulation;

namespace DevilDaggersInfo.App.Ui.ReplayEditor.Layouts;

public class ReplayEditor3dLayout : Layout, IExtendedLayout
{
	private readonly InputsVisualizer _inputsVisualizer;

	public ReplayEditor3dLayout()
	{
		_inputsVisualizer = new(new PixelBounds(0, 0, 256, 192));

		NestingContext.Add(_inputsVisualizer);
	}

	public void Update()
	{
		if (_replaySimulation != null && _currentTick < _replaySimulation.InputSnapshots.Count)
			_inputsVisualizer.SetInputs(_replaySimulation.InputSnapshots[_currentTick]);
	}
}
