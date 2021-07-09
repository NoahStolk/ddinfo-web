namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto
{
	public interface IGetDto<out TKey>
	{
		public TKey Id { get; }
	}
}
