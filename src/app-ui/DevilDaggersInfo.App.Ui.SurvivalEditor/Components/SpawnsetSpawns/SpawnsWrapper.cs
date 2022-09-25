using DevilDaggersInfo.App.Ui.Base.Components;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;

public sealed class SpawnsWrapper : AbstractScrollViewer<SpawnsWrapper, Spawns>
{
	public SpawnsWrapper(Rectangle metric)
		: base(metric)
	{
		Rectangle spawnsMetric = Rectangle.At(0, 0, 384, 640);

		Content = new(spawnsMetric, this);
		Scrollbar = new(spawnsMetric with { X1 = spawnsMetric.X2, X2 = spawnsMetric.X2 + 16 }, ScrollbarOnChange);

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
