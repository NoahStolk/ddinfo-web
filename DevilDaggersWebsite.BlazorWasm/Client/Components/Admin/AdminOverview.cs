using DevilDaggersWebsite.BlazorWasm.Shared.Dto;

namespace DevilDaggersWebsite.BlazorWasm.Client.Components.Admin
{
	public partial class AdminOverview<TGetDto, TKey>
		where TGetDto : IGetDto<TKey>
	{
	}
}
