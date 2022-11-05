using DevilDaggersInfo.Api.Ddcl;
using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.Types.Web;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;

public class LeaderboardList : AbstractComponent
{
	private readonly List<TextButton> _leaderboardComponents = new(); // TODO: Custom component.

	public LeaderboardList(Rectangle metric)
		: base(metric)
	{
		Button button = new(Rectangle.At(0, 0, 16, 16), Load, GlobalStyles.DefaultButtonStyle);
		NestingContext.Add(button);
	}

	public void Load() // TODO: Load on initialize.
	{
		AsyncHandler.Run(Populate, () => FetchCustomLeaderboards.HandleAsync(CustomLeaderboardCategory.Survival, 0, 10, 1, false));

		void Populate(Page<GetCustomLeaderboardForOverview>? cls)
		{
			foreach (TextButton leaderboardComponent in _leaderboardComponents)
				NestingContext.Remove(leaderboardComponent);

			_leaderboardComponents.Clear();

			if (cls == null)
				return;

			int y = 0;
			foreach (GetCustomLeaderboardForOverview cl in cls.Results)
			{
				const int height = 16;
				_leaderboardComponents.Add(new TextButton(Rectangle.At(0, y, 256, height), () => {}, GlobalStyles.DefaultButtonStyle, GlobalStyles.DefaultLeft, cl.SpawnsetName));
				y += height;
			}

			foreach (TextButton leaderboardComponent in _leaderboardComponents)
				NestingContext.Add(leaderboardComponent);
		}
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		const int border = 1;
		RenderBatchCollector.RenderRectangleTopLeft(Metric.Size, Metric.TopLeft + parentPosition, 0, Color.Green);
		RenderBatchCollector.RenderRectangleTopLeft(Metric.Size - new Vector2i<int>(border * 2), Metric.TopLeft + parentPosition + new Vector2i<int>(border), 1, Color.Black);
	}
}
