namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Spawnsets;

public class GetSpawnsetName : IGetDto<int>
{
	public int Id { get; init; }

	public string Name { get; init; } = null!;
}
