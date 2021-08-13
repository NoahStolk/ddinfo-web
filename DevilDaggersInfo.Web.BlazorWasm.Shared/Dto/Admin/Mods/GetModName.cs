namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Mods
{
	public class GetModName : IGetDto<int>
	{
		public int Id { get; init; }

		public string Name { get; init; } = null!;
	}
}
