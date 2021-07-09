namespace DevilDaggersWebsite.BlazorWasm.Shared
{
	public interface IGetDto<out TKey>
	{
		public TKey Id { get; }
	}
}
