namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public;

public interface IGetLeaderboardDto<TGetEntryDto> : IGetLeaderboardGlobalDto
	where TGetEntryDto : IGetEntryDto
{
	List<TGetEntryDto> Entries { get; set; } // Use setter for client-side sorting.
}
