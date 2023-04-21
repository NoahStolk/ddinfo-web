namespace Warp.NET;

public interface IContentContainer<TContent>
	where TContent : class
{
	static abstract void Initialize(IReadOnlyDictionary<string, TContent> content);
}
