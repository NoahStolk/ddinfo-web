using DevilDaggersInfo.App.Ui.Base;
using Warp.Ui;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetSpawns;

public sealed class SpawnsWrapper : ScrollViewer<Spawns>
{
	public SpawnsWrapper(Rectangle metric)
		: base(metric)
	{
		Rectangle spawnsMetric = Rectangle.At(0, 0, 512, 768);

		Content = new(spawnsMetric, this);
		Scrollbar = new(spawnsMetric with { X1 = spawnsMetric.X2, X2 = spawnsMetric.X2 + 32 }, ScrollbarOnChange);

		NestingContext.Add(Content);
		NestingContext.Add(Scrollbar);

		void ScrollbarOnChange(float percentage)
		{
			Content.SetScrollOffset(new(0, (int)MathF.Round(percentage * -Content.ContentHeightInPixels)));
		}
	}

	protected override Scrollbar Scrollbar { get; }
	protected override Spawns Content { get; }

	public override void InitializeContent()
	{
		base.InitializeContent();

		Content.SetSpawnset();
	}
}
