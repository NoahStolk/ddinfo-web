using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Extensions;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Utils;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.State;

public class StateWrapper : AbstractComponent
{
	private readonly Label _labelMemoryValue;
	private readonly Label _labelStateValue;
	private readonly Label _labelSpawnsetValue;
	private readonly Label _labelSubmissionValue;

	public StateWrapper(IBounds bounds)
		: base(bounds)
	{
		const int labelSeparatorPosition = 80;
		const int labelHeight = 16;
		int labelDepth = Depth + 2;

		string[] labelTexts =
		{
			"Memory", "State", "Spawnset", "Last upload",
		};

		for (int i = 0; i < labelTexts.Length; i++)
		{
			Label label = new(bounds.CreateNested(0, i * labelHeight, labelSeparatorPosition, labelHeight), labelTexts[i], GlobalStyles.LabelDefaultLeft) { Depth = labelDepth };
			NestingContext.Add(label);
		}

		_labelMemoryValue = new(bounds.CreateNested(labelSeparatorPosition, labelHeight * 0, labelSeparatorPosition, labelHeight), string.Empty, GlobalStyles.LabelDefaultLeft) { Depth = labelDepth };
		_labelStateValue = new(bounds.CreateNested(labelSeparatorPosition, labelHeight * 1, labelSeparatorPosition, labelHeight), string.Empty, GlobalStyles.LabelDefaultLeft) { Depth = labelDepth };
		_labelSpawnsetValue = new(bounds.CreateNested(labelSeparatorPosition, labelHeight * 2, labelSeparatorPosition, labelHeight), string.Empty, GlobalStyles.LabelDefaultLeft) { Depth = labelDepth };
		_labelSubmissionValue = new(bounds.CreateNested(labelSeparatorPosition, labelHeight * 3, labelSeparatorPosition, labelHeight), string.Empty, GlobalStyles.LabelDefaultLeft) { Depth = labelDepth };

		NestingContext.Add(_labelMemoryValue);
		NestingContext.Add(_labelStateValue);
		NestingContext.Add(_labelSpawnsetValue);
		NestingContext.Add(_labelSubmissionValue);
	}

	public void SetState()
	{
		_labelMemoryValue.Text = StateManager.MarkerState.Marker.HasValue ? $"0x{StateManager.MarkerState.Marker.Value:X}" : "Waiting...";
		_labelStateValue.Text = StateManager.RecordingState.RecordingStateType.ToDisplayString();
		_labelSpawnsetValue.Text = StateManager.ActiveSpawnsetState.Name ?? "(unknown)";
		_labelSubmissionValue.Text = DateTimeUtils.FormatTimeAgo(StateManager.RecordingState.LastSubmission);
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		const int border = 1;
		Root.Game.RectangleRenderer.Schedule(Bounds.Size, Bounds.Center + scrollOffset, Depth, Color.Red);
		Root.Game.RectangleRenderer.Schedule(Bounds.Size - new Vector2i<int>(border * 2), Bounds.Center + scrollOffset, Depth + 1, Color.Black);
	}
}