using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.State;

public class StateWrapper : AbstractComponent
{
	private readonly Label _labelProcessValue;
	private readonly Label _labelMemoryValue;
	private readonly Label _labelStateValue;
	private readonly Label _labelSpawnsetValue;
	private readonly Label _labelSubmissionValue;

	public StateWrapper(IBounds bounds)
		: base(bounds)
	{
		const int labelHalfWidth = 128;
		const int labelHeight = 16;
		int labelDepth = Depth + 2;

		string[] labelTexts =
		{
			"Process", "Memory", "State", "Spawnset", "Submission",
		};

		for (int i = 0; i < labelTexts.Length; i++)
		{
			Label label = new(bounds.CreateNested(0, i * labelHeight, labelHalfWidth, labelHeight), labelTexts[i], GlobalStyles.LabelDefaultLeft) { Depth = labelDepth };
			NestingContext.Add(label);
		}

		_labelProcessValue = new(bounds.CreateNested(labelHalfWidth, labelHeight * 0, labelHalfWidth, labelHeight), string.Empty, GlobalStyles.LabelDefaultRight) { Depth = labelDepth };
		_labelMemoryValue = new(bounds.CreateNested(labelHalfWidth, labelHeight * 1, labelHalfWidth, labelHeight), string.Empty, GlobalStyles.LabelDefaultRight) { Depth = labelDepth };
		_labelStateValue = new(bounds.CreateNested(labelHalfWidth, labelHeight * 2, labelHalfWidth, labelHeight), string.Empty, GlobalStyles.LabelDefaultRight) { Depth = labelDepth };
		_labelSpawnsetValue = new(bounds.CreateNested(labelHalfWidth, labelHeight * 3, labelHalfWidth, labelHeight), string.Empty, GlobalStyles.LabelDefaultRight) { Depth = labelDepth };
		_labelSubmissionValue = new(bounds.CreateNested(labelHalfWidth, labelHeight * 4, labelHalfWidth, labelHeight), string.Empty, GlobalStyles.LabelDefaultRight) { Depth = labelDepth };

		NestingContext.Add(_labelProcessValue);
		NestingContext.Add(_labelMemoryValue);
		NestingContext.Add(_labelStateValue);
		NestingContext.Add(_labelSpawnsetValue);
		NestingContext.Add(_labelSubmissionValue);
	}

	public void SetState()
	{
		_labelProcessValue.Text = Root.Game.GameMemoryService.HasProcess ? "Attached" : "Waiting...";
		_labelMemoryValue.Text = StateManager.MarkerState.Marker?.ToString() ?? "Waiting...";
		_labelStateValue.Text = StateManager.RecordingState.RecordingStateType.ToString();
		_labelSpawnsetValue.Text = StateManager.ActiveSpawnsetState.Name ?? "(unknown)";
		_labelSubmissionValue.Text = "...";
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		const int border = 1;
		Root.Game.RectangleRenderer.Schedule(Bounds.Size, Bounds.Center + scrollOffset, Depth, Color.Red);
		Root.Game.RectangleRenderer.Schedule(Bounds.Size - new Vector2i<int>(border * 2), Bounds.Center + scrollOffset, Depth + 1, Color.Black);
	}
}
