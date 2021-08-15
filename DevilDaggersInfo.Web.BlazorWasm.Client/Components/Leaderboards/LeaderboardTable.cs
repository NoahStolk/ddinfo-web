using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Components.Leaderboards;

public partial class LeaderboardTable<TGetLeaderboardDto, TGetEntryDto>
	where TGetLeaderboardDto : IGetLeaderboardDto<TGetEntryDto>
	where TGetEntryDto : IGetEntryDto
{
}
