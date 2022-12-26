using DevilDaggersInfo.Api.App;
using DevilDaggersInfo.Api.App.CustomLeaderboards;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States.Actions;
using DevilDaggersInfo.Types.Web;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components.LeaderboardList;

public class LeaderboardListWrapper : AbstractComponent
{
	private const int _borderSize = 1;

	private readonly List<LeaderboardListEntry> _leaderboardComponents = new();
	private readonly TooltipIconButton _prevButton;
	private readonly TooltipIconButton _nextButton;

	public LeaderboardListWrapper(IBounds bounds)
		: base(bounds)
	{
		Dropdown categoryDropdown = new(bounds.CreateNested(48, 32, 96, 20), "Category", GlobalStyles.DefaultDropdownStyle) { Depth = Depth + 1 };
		NestingContext.Add(categoryDropdown);

		CustomLeaderboardCategory[] categories = Enum.GetValues<CustomLeaderboardCategory>();
		for (int i = 0; i < categories.Length; i++)
		{
			CustomLeaderboardCategory category = categories[i];
			DropdownEntry dropdownEntry = new(categoryDropdown.Bounds.CreateNested(0, (i + 1) * 20, 96, 20), categoryDropdown, () => StateManager.Dispatch(new SetCategory(category)), category.ToString(), GlobalStyles.DefaultDropdownEntryStyle)
			{
				IsActive = false,
				Depth = Depth + 100,
			};

			categoryDropdown.AddChild(dropdownEntry);
			NestingContext.Add(dropdownEntry);
		}

		_prevButton = new(bounds.CreateNested(4, 32, 20, 20), () => StateManager.Dispatch(new SetPageIndex(StateManager.LeaderboardListState.PageIndex - 1)), GlobalStyles.NavigationButtonStyle, WarpTextures.ArrowLeft, "Previous", Color.HalfTransparentWhite, Color.White) { Depth = Depth + 100 };
		_nextButton = new(bounds.CreateNested(24, 32, 20, 20), () => StateManager.Dispatch(new SetPageIndex(StateManager.LeaderboardListState.PageIndex + 1)), GlobalStyles.NavigationButtonStyle, WarpTextures.ArrowRight, "Next", Color.HalfTransparentWhite, Color.White) { Depth = Depth + 100 };

		NestingContext.Add(_prevButton);
		NestingContext.Add(_nextButton);

		StateManager.Subscribe<LoadLeaderboardList>(_ => Load());
		StateManager.Subscribe<SetCategory>(_ => Load());
		StateManager.Subscribe<SetPageIndex>(_ => Load());

		StateManager.Subscribe<SetTotalResults>(_ => UpdateNavigationButtons());
	}

	private void UpdateNavigationButtons()
	{
		_prevButton.IsDisabled = StateManager.LeaderboardListState.PageIndex == 0;
		_nextButton.IsDisabled = StateManager.LeaderboardListState.PageIndex == StateManager.LeaderboardListState.MaxPageIndex;
	}

	private void Load()
	{
		foreach (LeaderboardListEntry leaderboardComponent in _leaderboardComponents)
			NestingContext.Remove(leaderboardComponent);

		_leaderboardComponents.Clear();

		_prevButton.IsDisabled = true;
		_nextButton.IsDisabled = true;

		AsyncHandler.Run(Populate, () => FetchCustomLeaderboards.HandleAsync(StateManager.LeaderboardListState.Category, StateManager.LeaderboardListState.PageIndex, StateManager.LeaderboardListState.PageSize, StateManager.RecordingState.CurrentPlayerId, false));

		void Populate(Page<GetCustomLeaderboardForOverview>? customLeaderboards)
		{
			Set();

			void Set()
			{
				if (customLeaderboards == null)
					return;

				StateManager.Dispatch(new SetTotalResults(customLeaderboards.TotalResults));

				int y = 96;
				foreach (GetCustomLeaderboardForOverview cl in customLeaderboards.Results)
				{
					const int height = 16;
					_leaderboardComponents.Add(new(Bounds.CreateNested(_borderSize, y, Bounds.Size.X - _borderSize * 2, height), cl) { Depth = Depth + 3 });
					y += height;
				}

				foreach (LeaderboardListEntry leaderboardComponent in _leaderboardComponents)
					NestingContext.Add(leaderboardComponent);
			}

			StateManager.Dispatch(new SetLoading(false));
		}
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		Root.Game.RectangleRenderer.Schedule(Bounds.Size, Bounds.Center + scrollOffset, Depth, Color.Green);
		Root.Game.RectangleRenderer.Schedule(Bounds.Size - new Vector2i<int>(_borderSize * 2), Bounds.Center, Depth + 1, Color.Black);

		Root.Game.MonoSpaceFontRenderer24.Schedule(new(1), scrollOffset + Bounds.TopLeft + new Vector2i<int>(4, 4), Depth + 2, Color.Yellow, $"{StateManager.LeaderboardListState.Category} leaderboards", TextAlign.Left);

		string text;
		Color color;
		if (StateManager.LeaderboardListState.IsLoading)
		{
			text = "Loading...";
			color = Color.Red;
		}
		else
		{
			int page = StateManager.LeaderboardListState.PageIndex + 1;
			int totalPages = StateManager.LeaderboardListState.MaxPageIndex + 1;
			int start = StateManager.LeaderboardListState.PageIndex * StateManager.LeaderboardListState.PageSize + 1;
			int end = Math.Min(StateManager.LeaderboardListState.TotalResults, (StateManager.LeaderboardListState.PageIndex + 1) * StateManager.LeaderboardListState.PageSize);
			int total = StateManager.LeaderboardListState.TotalResults;
			text = $"Page {page} of {totalPages} ({start} - {end} of {total})";
			color = Color.Yellow;
		}

		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), scrollOffset + Bounds.TopLeft + new Vector2i<int>(4, 64), Depth + 2, color, text, TextAlign.Left);
	}
}
