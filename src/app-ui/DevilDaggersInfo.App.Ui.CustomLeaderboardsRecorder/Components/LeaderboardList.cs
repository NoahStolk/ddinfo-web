using DevilDaggersInfo.Api.Ddcl;
using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.App.Ui.Base;
using DevilDaggersInfo.App.Ui.Base.Components;
using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using DevilDaggersInfo.Types.Web;
using Warp.NET.RenderImpl.Ui.Components;
using Warp.NET.Text;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Components;

public class LeaderboardList : AbstractComponent
{
	private const int _headerHeight = 16;
	private const int _pageSize = 20;

	private readonly List<LeaderboardListEntry> _leaderboardComponents = new();
	private readonly TooltipIconButton _prevButton;
	private readonly TooltipIconButton _nextButton;

	// TODO: Move to state class.
	private int _maxPageIndex = int.MaxValue;
	private int _totalResults = int.MaxValue;
	private CustomLeaderboardCategory _category = CustomLeaderboardCategory.Survival;
	private int _pageIndex;
	private bool _isLoading;

	public LeaderboardList(IBounds metric)
		: base(metric)
	{
		Dropdown categoryDropdown = new(Rectangle.At(4, 4, 96, _headerHeight), "Category", GlobalStyles.DefaultDropdownStyle) { Depth = Depth + 1 };
		NestingContext.Add(categoryDropdown);

		CustomLeaderboardCategory[] categories = Enum.GetValues<CustomLeaderboardCategory>();
		for (int i = 0; i < categories.Length; i++)
		{
			CustomLeaderboardCategory category = categories[i];
			DropdownEntry dropdownEntry = new(Rectangle.At(4, 4 + (i + 1) * 16, 96, 16), categoryDropdown, () => ChangeAndLoad(() => _category = category), category.ToString(), GlobalStyles.DefaultDropdownEntryStyle)
			{
				IsActive = false,
				Depth = Depth + 100,
			};

			categoryDropdown.AddChild(dropdownEntry);
			NestingContext.Add(dropdownEntry);
		}

		_prevButton = new(Rectangle.At(4, 64, 20, 20), () => ChangeAndLoad(() => --_pageIndex), GlobalStyles.DefaultButtonStyle, WarpTextures.ArrowLeft, "Previous");
		_nextButton = new(Rectangle.At(24, 64, 20, 20), () => ChangeAndLoad(() => ++_pageIndex), GlobalStyles.DefaultButtonStyle, WarpTextures.ArrowRight, "Next");

		NestingContext.Add(_prevButton);
		NestingContext.Add(_nextButton);

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
		foreach (LeaderboardListEntry leaderboardComponent in _leaderboardComponents)
			NestingContext.Remove(leaderboardComponent);

		_leaderboardComponents.Clear();

		SetLoading();

		AsyncHandler.Run(Populate, () => FetchCustomLeaderboards.HandleAsync(_category, _pageIndex, _pageSize, 21854, false));

		void Populate(Page<GetCustomLeaderboardForOverview>? cls)
		{
			Set();
			UnsetLoading();

			void Set()
			{
				if (cls == null)
					return;

				_maxPageIndex = (int)Math.Ceiling((cls.TotalResults + 1) / (float)_pageSize) - 1;
				_totalResults = cls.TotalResults;
				_pageIndex = Math.Clamp(_pageIndex, 0, _maxPageIndex);

				int y = 128;
				foreach (GetCustomLeaderboardForOverview cl in cls.Results)
				{
					const int height = 16;
					_leaderboardComponents.Add(new(Rectangle.At(0, y, Bounds.Size.X, height), cl) { Depth = Depth + 2 });
					y += height;
				}

				foreach (LeaderboardListEntry leaderboardComponent in _leaderboardComponents)
					NestingContext.Add(leaderboardComponent);
			}
		}
	}

	private void UnsetLoading()
	{
		_isLoading = false;
		_prevButton.IsDisabled = false;
		_nextButton.IsDisabled = false;
	}

	private void SetLoading()
	{
		_isLoading = true;
		_prevButton.IsDisabled = true;
		_nextButton.IsDisabled = true;
	}

	public override void Render(Vector2i<int> parentPosition)
	{
		base.Render(parentPosition);

		const int border = 1;
		Vector2i<int> center = Bounds.TopLeft + Bounds.Size / 2;
		Root.Game.RectangleRenderer.Schedule(Bounds.Size, center + parentPosition, Depth, Color.Green);
		Root.Game.RectangleRenderer.Schedule(Bounds.Size - new Vector2i<int>(border * 2), center, Depth + 1, Color.Black);

		Root.Game.MonoSpaceFontRenderer12.Schedule(new(2), parentPosition + Bounds.TopLeft + new Vector2i<int>(4, 36), Depth + 2, Color.Yellow, $"{_category} leaderboards", TextAlign.Left);

		string text;
		Color color;
		if (_isLoading)
		{
			text = "Loading...";
			color = Color.Red;
		}
		else
		{
			text = $"Page {_pageIndex + 1} of {_maxPageIndex + 1} ({_pageIndex * _pageSize + 1} - {Math.Min(_totalResults, (_pageIndex + 1) * _pageSize)} of {_totalResults})";
			color = Color.Yellow;
		}

		Root.Game.MonoSpaceFontRenderer12.Schedule(new(1), parentPosition + Bounds.TopLeft + new Vector2i<int>(4, 96), Depth + 2, color, text, TextAlign.Left);
	}
}
