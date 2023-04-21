namespace DevilDaggersInfo.App.Engine;

public interface IContentContainer<TContent>
	where TContent : class
{
	static abstract void Initialize(IReadOnlyDictionary<string, TContent> content);
}
