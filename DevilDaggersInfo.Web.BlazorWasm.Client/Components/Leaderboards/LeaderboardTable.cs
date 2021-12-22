using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Components.Leaderboards;

public partial class LeaderboardTable<TGetLeaderboardDto, TGetEntryDto>
	where TGetLeaderboardDto : class, IGetLeaderboardDto<TGetEntryDto>
	where TGetEntryDto : class, IGetEntryDto
{
}
