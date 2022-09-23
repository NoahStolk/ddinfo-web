using DevilDaggersInfo.App.Ui.Base;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;

public sealed class SpawnsWrapper : AbstractScrollViewer<SpawnsWrapper, Spawns>
{
	public SpawnsWrapper(Rectangle metric)
		: base(metric)
	{
		Rectangle spawnsMetric = Rectangle.At(0, 0, 512, 768);

		Content = new(spawnsMetric, this);
		Scrollbar = new(spawnsMetric with { X1 = spawnsMetric.X2, X2 = spawnsMetric.X2 + 32 }, ScrollbarOnChange);

		NestingContext.Add(Content);
		NestingContext.Add(Scrollbar);
	}

	protected override Scrollbar Scrollbar { get; }
	protected override Spawns Content { get; }

	public override void InitializeContent()
	{
		Content.SetSpawnset();

		base.InitializeContent();
	}
}
