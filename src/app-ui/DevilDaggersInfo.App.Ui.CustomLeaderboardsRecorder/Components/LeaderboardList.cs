using DevilDaggersInfo.Api.Ddcl;
using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.Enums;
using DevilDaggersInfo.App.Ui.Base.Rendering;
using DevilDaggersInfo.Types.Web;
using Warp.Numerics;
using Warp.Ui;
using Warp.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;

public class LeaderboardList : AbstractComponent
{
	private const int _headerHeight = 16;
	private const int _pageSize = 20;

	private readonly List<LeaderboardListEntry> _leaderboardComponents = new();

	private int _maxPageIndex = int.MaxValue;
	private CustomLeaderboardCategory _category = CustomLeaderboardCategory.Survival;
	private int _pageIndex;

	public LeaderboardList(Rectangle metric)
		: base(metric)
	{
		IconButton prevButton = new(Rectangle.At(4, 4, 16, _headerHeight), () => ChangeAndLoad(() => --_pageIndex), GlobalStyles.DefaultButtonStyle, "Previous", Textures.ArrowLeft);
		IconButton nextButton = new(Rectangle.At(24, 4, 16, _headerHeight), () => ChangeAndLoad(() => ++_pageIndex), GlobalStyles.DefaultButtonStyle, "Next", Textures.ArrowRight);

		List<TextButton> categoryButtons = Enum.GetValues<CustomLeaderboardCategory>()
			.Select((c, i) => new TextButton(Rectangle.At(0, (i + 1) * 16, 96, 16), () => ChangeAndLoad(() => _category = c), GlobalStyles.DefaultButtonStyle, GlobalStyles.DefaultLeft, c.ToString()) { IsActive = false })
			.ToList();

		Dropdown dropdown = new(Rectangle.At(52, 4, 96, _headerHeight * (categoryButtons.Count + 1)), _headerHeight, GlobalStyles.DefaultLeft, categoryButtons.Cast<AbstractComponent>().ToList(), "Category");

		NestingContext.Add(prevButton);
		NestingContext.Add(nextButton);
		NestingContext.Add(dropdown);

		Load(); // TODO: Load when clicking purple CL button.

		void ChangeAndLoad(Action action)
		{
			action();
			_pageIndex = Math.Clamp(_pageIndex, 0, _maxPageIndex);

			Load();
		}
	}

	public void Load()
	{
		AsyncHandler.Run(Populate, () => FetchCustomLeaderboards.HandleAsync(_category, _pageIndex, _pageSize, 21854, false));

		void Populate(Page<GetCustomLeaderboardForOverview>? cls)
		{
			foreach (LeaderboardListEntry leaderboardComponent in _leaderboardComponents)
				NestingContext.Remove(leaderboardComponent);

			_leaderboardComponents.Clear();

			if (cls == null)
				return;

			_maxPageIndex = (int)Math.Ceiling((cls.TotalResults + 1) / (float)_pageSize) - 1;
			_pageIndex = Math.Clamp(_pageIndex, 0, _maxPageIndex);

			int y = 96;
			foreach (GetCustomLeaderboardForOverview cl in cls.Results)
			{
				const int height = 16;
				_leaderboardComponents.Add(new(Rectangle.At(0, y, 256, height), cl));
				y += height;
			}

			foreach (LeaderboardListEntry leaderboardComponent in _leaderboardComponents)
				NestingContext.Add(leaderboardComponent);
		}
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		const int border = 1;
		RenderBatchCollector.RenderRectangleTopLeft(Metric.Size, Metric.TopLeft + parentPosition, 0, Color.Green);
		RenderBatchCollector.RenderRectangleTopLeft(Metric.Size - new Vector2i<int>(border * 2), Metric.TopLeft + parentPosition + new Vector2i<int>(border), 1, Color.Black);

		RenderBatchCollector.RenderMonoSpaceText(FontSize.F8X8, new(1), parentPosition + Metric.TopLeft + new Vector2i<int>(4, 80), Depth + 2, Color.Blue, $"{_pageIndex + 1} / {_maxPageIndex + 1}", TextAlign.Left);
	}
}
