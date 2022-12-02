using DevilDaggersInfo.Api.Ddcl;
using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;
using DevilDaggersInfo.Types.Web;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;

public class LeaderboardList : AbstractComponent
{
	private const int _borderSize = 1;

	private readonly List<LeaderboardListEntry> _leaderboardComponents = new();
	private readonly TooltipIconButton _prevButton;
	private readonly TooltipIconButton _nextButton;

	public LeaderboardList(IBounds bounds)
		: base(bounds)
	{
		Dropdown categoryDropdown = new(bounds.CreateNested(48, 32, 96, 20), "Category", GlobalStyles.DefaultDropdownStyle) { Depth = Depth + 1 };
		NestingContext.Add(categoryDropdown);

		CustomLeaderboardCategory[] categories = Enum.GetValues<CustomLeaderboardCategory>();
		for (int i = 0; i < categories.Length; i++)
		{
			CustomLeaderboardCategory category = categories[i];
			DropdownEntry dropdownEntry = new(categoryDropdown.Bounds.CreateNested(0, (i + 1) * 20, 96, 20), categoryDropdown, () => ChangeAndLoad(() => StateManager.SetCategory(category)), category.ToString(), GlobalStyles.DefaultDropdownEntryStyle)
			{
				IsActive = false,
				Depth = Depth + 100,
			};

			categoryDropdown.AddChild(dropdownEntry);
			NestingContext.Add(dropdownEntry);
		}

		_prevButton = new(bounds.CreateNested(4, 32, 20, 20), () => ChangeAndLoad(() => StateManager.SetPageIndex(StateManager.LeaderboardListState.PageIndex - 1)), GlobalStyles.NavigationButtonStyle, WarpTextures.ArrowLeft, "Previous") { Depth = Depth + 100 };
		_nextButton = new(bounds.CreateNested(24, 32, 20, 20), () => ChangeAndLoad(() => StateManager.SetPageIndex(StateManager.LeaderboardListState.PageIndex + 1)), GlobalStyles.NavigationButtonStyle, WarpTextures.ArrowRight, "Next") { Depth = Depth + 100 };

		NestingContext.Add(_prevButton);
		NestingContext.Add(_nextButton);

		Load(); // TODO: Load when clicking purple CL button.

		void ChangeAndLoad(Action action)
		{
			action();

			// Correct page index should it ever go out of bounds.
			StateManager.SetPageIndex(Math.Clamp(StateManager.LeaderboardListState.PageIndex, 0, StateManager.LeaderboardListState.MaxPageIndex));

			Load();
		}
	}

	public void Load()
	{
		foreach (LeaderboardListEntry leaderboardComponent in _leaderboardComponents)
			NestingContext.Remove(leaderboardComponent);

		_leaderboardComponents.Clear();

		StateManager.SetLoading(true);
		_prevButton.IsDisabled = true;
		_nextButton.IsDisabled = true;

		AsyncHandler.Run(Populate, () => FetchCustomLeaderboards.HandleAsync(StateManager.LeaderboardListState.Category, StateManager.LeaderboardListState.PageIndex, StateManager.LeaderboardListState.PageSize, 21854, false));

		void Populate(Page<GetCustomLeaderboardForOverview>? cls)
		{
			Set();

			StateManager.SetLoading(false);
			_prevButton.IsDisabled = StateManager.LeaderboardListState.PageIndex == 0;
			_nextButton.IsDisabled = StateManager.LeaderboardListState.PageIndex == StateManager.LeaderboardListState.MaxPageIndex;

			void Set()
			{
				if (cls == null)
					return;

				StateManager.SetTotalResults(cls.TotalResults);

				int y = 96;
				foreach (GetCustomLeaderboardForOverview cl in cls.Results)
				{
					const int height = 16;
					_leaderboardComponents.Add(new(Bounds.CreateNested(_borderSize, y, Bounds.Size.X - _borderSize * 2, height), cl) { Depth = Depth + 3 });
					y += height;
				}

				foreach (LeaderboardListEntry leaderboardComponent in _leaderboardComponents)
					NestingContext.Add(leaderboardComponent);
			}
		}
	}

	public override void Render(Vector2i<int> scrollOffset)
	{
		base.Render(scrollOffset);

		Root.Game.RectangleRenderer.Schedule(Bounds.Size, Bounds.Center + scrollOffset, Depth, Color.Green);
		Root.Game.RectangleRenderer.Schedule(Bounds.Size - new Vector2i<int>(_borderSize * 2), Bounds.Center, Depth + 1, Color.Black);

		Root.Game.MonoSpaceFontRenderer12.Schedule(new(2), scrollOffset + Bounds.TopLeft + new Vector2i<int>(4, 4), Depth + 2, Color.Yellow, $"{StateManager.LeaderboardListState.Category} leaderboards", TextAlign.Left);

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
